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
        
        //任何空中状态 检测到地面 切换到闲置状态
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        //任何空中状态 检测到墙壁，检测不到地面，且x方向输入与面向方向一致时切入到爬墙状态
        if (player.IsWallDetected() && !player.IsGroundDetected() && xInput ==(int)player.faceDirection) 
        {
            stateMachine.ChangeState(player.wallSlideDownState);
        }

        //任何空中状态 检测到X按键输入时切换到攻击状态
        if (Input.GetKeyDown(KeyCode.X))
        {
            //如果连击数重置时间已到，则重置连击数
            if (player.combooResetTimer <= 0)
            {
                player.attackComboo = 0;
            }

            switch (player.attackComboo)
            {
                case 0:
                    player.stateMachine.ChangeState(player.attack_01_State);
                    break;
                case 1:
                    player.stateMachine.ChangeState(player.attack_02_State);
                    break;
                case 2:
                    player.stateMachine.ChangeState(player.attack_03_State);
                    break;
            }
        }
    }
}
