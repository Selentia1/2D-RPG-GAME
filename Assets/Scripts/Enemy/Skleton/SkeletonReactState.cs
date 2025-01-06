using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonReactState : SkeletonGuardState
{
    public SkeletonReactState(Enemy enemy, EnemyStateMachine stateMachine, string animParameterName) : base(enemy, stateMachine, animParameterName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        skeleton.SetVelocity(0, 0);
        triggerCalled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(skeleton.traceState);
        }
    }
}
