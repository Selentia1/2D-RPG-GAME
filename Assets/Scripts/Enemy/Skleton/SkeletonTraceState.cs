using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class SkeletonTraceState : SkeletonGuardState
{
    private Transform player;
    
    public SkeletonTraceState(Enemy enemy, EnemyStateMachine stateMachine, string animParameterName) : base(enemy, stateMachine, animParameterName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (skeleton.GuardTimer > 0) {
            player = skeleton.hitInfo.transform;
            
            //在追踪玩家的过程中如果遇上墙体或者没有继续向前的路则会关闭玩家追踪
            if (!skeleton.IsGroundDetected() || skeleton.IsWallDetected()) {
                playerTracking = false;
            }

            //当玩家在左侧则向左追踪，反之在右侧则向右追踪
            if (playerTracking)
            {
                if (skeleton.transform.position.x > player.transform.position.x && skeleton.faceDirection != Direction.Dir.Left)
                {
                    skeleton.Flip();
                }
                else if (skeleton.transform.position.x < player.transform.position.x && skeleton.faceDirection != Direction.Dir.Right)
                {
                    skeleton.Flip();
                }
            }

            skeleton.SetVelocity(skeleton.traceSpeed * (int)skeleton.faceDirection, rb.velocity.y);

            //在攻击范围内则切换至攻击状态
            if (skeleton.IsAttackDetected())
            {
                stateMachine.ChangeState(skeleton.attackState);
            }
        }

        else if(skeleton.GuardTimer < 0 || Vector2.Distance(skeleton.transform.position,skeleton.hitInfo.transform.position) > skeleton.GuardDistance)
        {
            stateMachine.ChangeState(skeleton.idleState);
        }
    }
}
