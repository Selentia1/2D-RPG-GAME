using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkillState_Crystal : UseSkillState
{
    public UseSkillState_Crystal(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
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
        player.SetVelocity(0, rb.velocity.y);
        base.Update();
        if (triggerCalled) {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
