using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlackHoleExplosionEffectState : EffectState
{
    BlackHoleAnimation blackHoleEffectAnimation;
    public BlackHoleExplosionEffectState(EffectAnimation effectAnimation, EffectStateMachine stateMachine, string animParameterName) : base(effectAnimation, stateMachine, animParameterName)
    {
        this.blackHoleEffectAnimation = (BlackHoleAnimation)effectAnimation;
        this.stateMachine = stateMachine;
        this.animParameterName = animParameterName;
    }
    public override void Enter()
    {
        base.Enter();
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
            stateMachine.ChangeState(effectAnimation.emptyState);
            blackHoleEffectAnimation.blackHole.explodeEnd = true;
        }
    }

}
