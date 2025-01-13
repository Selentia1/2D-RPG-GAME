using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseSkillState_Clone : PlayerUseSkillState
{
    public PlayerUseSkillState_Clone(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {

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
        player.SetVelocity(0, 0);
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
