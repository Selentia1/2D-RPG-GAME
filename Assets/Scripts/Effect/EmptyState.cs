using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : EffectState
{
    public EmptyState(EffectAnimation effectAnimation, EffectStateMachine stateMachine, string animParameterName) : base(effectAnimation, stateMachine, animParameterName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
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
    }
}
