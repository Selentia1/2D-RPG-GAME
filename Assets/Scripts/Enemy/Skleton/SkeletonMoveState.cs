using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SkeletonMoveState : SkeletonPatrolState
{
    public SkeletonMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animParameterName) : base(enemy, stateMachine, animParameterName)
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
        skeleton.SetVelocity(skeleton.moveSpeed * (int)skeleton.faceDirection, rb.velocity.y);
        if (skeleton.patrolTimer > skeleton.patrolTime / 2)
        {
            stateMachine.ChangeState(skeleton.idleState);
        }
        else if (skeleton.patrolTimer < 0) {
            stateMachine.ChangeState(skeleton.idleState);
        }
    }
}
