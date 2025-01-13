using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private int DashAfterImageNum = 3;
    private float DashAfterImageInterval = 0.1f;
    private float DashAfterImageTimer = 0f;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {

    }

    public override void Enter()
    {
        base.Enter();
      
        DashAfterImageTimer = DashAfterImageInterval;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.velocity.y);
        DashAfterImageNum = 3;
    }

    public override void Update()
    {
        
        SkillManager.instance.dash.skillTimer -= Time.deltaTime;
        DashAfterImageTimer -= Time.deltaTime;

        //生成DashAfterImageNum 个残影，残影消失间隔时间为DashAfterImageInterval
        if (DashAfterImageTimer < 0 && DashAfterImageNum > 0)
        {
            DashAfterImageTimer = DashAfterImageInterval;
            SkillManager.instance.clone.CreateClone(player.transform, "DashAfterImage", player.faceDirection);
        }

        if (SkillManager.instance.dash.skillTimer > 0) {
            if (xInput != 0)
            {
                player.SetVelocity(SkillManager.instance.dash.dashSpeed * xInput, 0);
            }
            else {
                if (player.faceDirection == Direction.Dir.Right)
                {
                    player.SetVelocity(SkillManager.instance.dash.dashSpeed, 0);
                }
                else if (player.faceDirection == Direction.Dir.Left)
                {
                    player.SetVelocity(-SkillManager.instance.dash.dashSpeed, 0);
                }
            }
        } else if (SkillManager.instance.dash.skillTimer <= 0) {
            player.stateMachine.ChangeState(player.idleState);
        }

        //注意一定要将base.Update();放在最下方，不然跳跃给的速度会被重置
        base.Update();
    }
}
