using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkillEffectState : EffectState
{
    protected UseSkillEffectAnimation useSkillEffectAnimation;
    public UseSkillEffectState(EffectAnimation effectAnimation, EffectStateMachine stateMachine, string animParameterName) : base(effectAnimation, stateMachine, animParameterName)
    {
        useSkillEffectAnimation = (UseSkillEffectAnimation)effectAnimation;
        this.stateMachine = stateMachine;
        this.animParameterName = animParameterName;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        useSkillEffectAnimation.spriteRenderer.color = new Color(useSkillEffectAnimation.spriteRenderer.color.r, useSkillEffectAnimation.spriteRenderer.color.g, useSkillEffectAnimation.spriteRenderer.color.b, 0);
        useSkillEffectAnimation.useSkillMask.color = new Color(0, 0, 0, 1);
        useSkillEffectAnimation.isUsingEffect = true;
        stateTimer = useSkillEffectAnimation.MaskfadeDurationCD;
    }

    public override void Exit()
    {
        base.Exit();
        useSkillEffectAnimation.isUsingEffect = false;
    }

    public override void Update()
    {
        stateTimer -= Time.deltaTime;
        base.Update();        

        if (stateTimer <= 0) {
            useSkillEffectAnimation.spriteRenderer.color = new Color(useSkillEffectAnimation.spriteRenderer.color.r, useSkillEffectAnimation.spriteRenderer.color.g, useSkillEffectAnimation.spriteRenderer.color.b, useSkillEffectAnimation.spriteRenderer.color.a + useSkillEffectAnimation.MaskfadeDurationCD);
            useSkillEffectAnimation.useSkillMask.color = new Color(0, 0, 0, useSkillEffectAnimation.useSkillMask.color.a - useSkillEffectAnimation.MaskfadeDurationCD);
            stateTimer = useSkillEffectAnimation.MaskfadeDurationCD;
        }

        if (useSkillEffectAnimation.spriteRenderer.color.a >= 1 && useSkillEffectAnimation.useSkillMask.color.a <=0) {
            stateMachine.ChangeState(useSkillEffectAnimation.emptyState);
        }
    }
}
