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
        player.Jump();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
        if (rb.velocity.y == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if(rb.velocity.y < 0 ){
            stateMachine.ChangeState(player.fallState);
        }
        base.Update();
    }
}

