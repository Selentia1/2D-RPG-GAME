using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerState
{
    public GroundedState(Player player, PlayerStateMachine stateMachine, string animStateName) : base(player, stateMachine, animStateName)
    {
    }

    public override void Enter()
    {
        if (player.IsGroundDetected()) 
        {
            player.jumpTimes = 0;
        }
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0) 
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
