using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectState 
{
    protected EffectAnimation effectAnimation;
    protected EffectStateMachine stateMachine;
    protected string animParameterName;
    protected bool triggerCalled;
    protected float stateTimer;
    public EffectState(EffectAnimation effectAnimation, EffectStateMachine stateMachine, string animParameterName)
    {
        this.effectAnimation = effectAnimation;
        this.stateMachine = stateMachine;
        this.animParameterName = animParameterName;
    }

    public virtual void Enter()
    {
        effectAnimation.animator.SetBool(animParameterName, true);
    }
    public virtual void Exit()
    {
        effectAnimation.animator.SetBool(animParameterName, false);
    }

    public virtual void Update() { 
    
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
