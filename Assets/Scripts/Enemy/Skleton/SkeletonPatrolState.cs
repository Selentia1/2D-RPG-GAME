using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPatrolState : SkeletonState
{

    public SkeletonPatrolState(Enemy enemy, EnemyStateMachine stateMachine, string animParameterName) : base(enemy, stateMachine, animParameterName)
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
        if (skeleton.IsPLayerDetected())
        {
            skeleton.hitInfo = skeleton.GetPLayerDetected();
            if (!skeleton.IsAttackDetected())
            {
                stateMachine.ChangeState(skeleton.reactState);
            }
            else if (skeleton.IsAttackDetected())
            {
                stateMachine.ChangeState(skeleton.attackState);
            }
        }
    }
}
