using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHurtState : SkeletonState
{
    public SkeletonHurtState(Enemy enemy, EnemyStateMachine stateMachine, string animParameterName) : base(enemy, stateMachine, animParameterName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        triggerCalled = false;
        skeleton.fx.StartCoroutine("FlashFX");
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
