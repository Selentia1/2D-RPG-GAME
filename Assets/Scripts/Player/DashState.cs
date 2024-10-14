using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DashState : PlayerState
{
    public DashState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.dashTimer = player.dashDuration;
        player.dashCDTimer = player.dashCoolDown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.dashTimer -= Time.deltaTime;
        if (player.dashTimer > 0) {
            if (player.faceDirection == Player.FaceDirection.Right)
            {
                player.SetVelocity(player.dashSpeed, 0);
            }
            else if (player.faceDirection == Player.FaceDirection.Left)
            {
                player.SetVelocity(-player.dashSpeed, 0);
            }
        } else if (player.dashTimer <= 0) {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
