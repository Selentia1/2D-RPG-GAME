using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideDownState : PlayerState
{
    public WallSlideDownState(Player player, PlayerStateMachine stateMachine, string animStateName) : base(player, stateMachine, animStateName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(rb.velocity.x, player.WallSlideFallSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (yInput == 0)
        {
            player.SetVelocity(rb.velocity.x, player.WallSlideFallSpeed);
        }
        else if(yInput == 1)
        {
            player.SetVelocity(rb.velocity.x, player.WallSlideFallSpeed * 2);
        }

        if (xInput != 0 && (int)player.faceDirection != xInput) {
            player.stateMachine.ChangeState(player.idleState);
        }

        if (player.IsGroundDetected() || !player.IsWallDetected() ) {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
