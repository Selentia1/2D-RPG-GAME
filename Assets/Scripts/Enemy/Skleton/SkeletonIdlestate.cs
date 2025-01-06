using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonPatrolState
{
    public SkeletonIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animParameterName) : base(enemy, stateMachine, animParameterName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        skeleton.patrolTimer = skeleton.patrolTime;
        skeleton.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (skeleton.patrolTimer < skeleton.patrolTime/2)
        {
            stateMachine.ChangeState(skeleton.moveState);
        }
    }
}
