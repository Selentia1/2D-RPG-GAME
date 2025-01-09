using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareUseSkillState : PlayerState
{
    public PlayerState useSkillState;
    public PrepareUseSkillState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.fx.useSkillEffectAnimation.StartCoroutine("StartEffect");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        player.SetVelocity(0, 0);
        if (!player.fx.useSkillEffectAnimation.isUsingEffect) {
            stateMachine.ChangeState(useSkillState);
        }
    }

}
