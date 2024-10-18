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
        //如果已经进入爬墙状态，且紧贴墙壁则重置跳跃次数
        if (player.IsWallDetected())
        {
            player.jumpTimes = 0;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        

        //设置爬墙下落的速度
        if (yInput == 0)
        {
            //设置爬墙自然下落的速度
            player.SetVelocity(rb.velocity.x, player.WallSlideFallSpeed);
        }
        else if(yInput == 1)
        {
            //设置爬墙快速下落的速度
            player.SetVelocity(rb.velocity.x, player.WallSlideFallSpeed * 2);
        }

        base.Update();

        //如果玩家背离移动墙面，则回到闲置状态
        if (xInput != 0 && (int)player.faceDirection != xInput) {
            player.stateMachine.ChangeState(player.idleState);
        }

        //如果检测到地面或者检测不到墙面则回到闲置状态
        if (player.IsGroundDetected() || !player.IsWallDetected()) {
            player.stateMachine.ChangeState(player.idleState);
        }

    }
}
