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

        //检测到y方向的速度小于零则切换到下落状态
        if (rb.velocity.y < 0) 
        {
            stateMachine.ChangeState(player.fallState);
        }

        //任何地面状态 检测到X按键输入时切换到攻击状态
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
