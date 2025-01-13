using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideDownState : PlayerState
{
    public PlayerWallSlideDownState(Player player, PlayerStateMachine stateMachine, string animStateName) : base(player, stateMachine, animStateName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //如果已经进入爬墙状态，重置跳跃次数
        player.jumpTimes = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        //修复爬墙状态下冲刺方向的问题
        if (Input.GetKeyDown(KeyCode.Z) && SkillManager.instance.dash.CkeckSkill())
        {
            
        }

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

        //如果检测到地面 或者 玩家背里移动墙面 或者因自动向下的速度而脱离墙面 则回到闲置状态
        if (player.IsGroundDetected() || (xInput != 0 && (int)player.faceDirection != xInput) || !player.IsWallDetected()) {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
