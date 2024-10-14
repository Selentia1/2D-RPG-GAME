using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : PlayerState
{
    public AirState(Player player, PlayerStateMachine stateMachine, string animStateName) : base(player, stateMachine, animStateName)
    {

    }

    public override void Enter()
    {
        player.jumpTimes = 0;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (rb.velocity.y == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.IsWallDetected() && !player.IsGroundDetected() && xInput ==(int)player.faceDirection) 
        {
            stateMachine.ChangeState(player.wallSlideDownState);
        }
    }
}
