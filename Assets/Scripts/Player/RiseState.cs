using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseState : AirState
{
    public RiseState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(rb.velocity.x, player.jumpForce);
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < 0 ){
            stateMachine.ChangeState(player.fallState);
        }
    }
}

