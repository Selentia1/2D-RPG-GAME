using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRiseState : PlayerAirState
{
    public PlayerRiseState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
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
        //y������ٶ�С����ʱ�л�������״̬
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        base.Update();
    }
}

