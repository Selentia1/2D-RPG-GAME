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

        //��⵽y������ٶ�С�������л�������״̬
        if (rb.velocity.y < 0) 
        {
            stateMachine.ChangeState(player.fallState);
        }

        //�κε���״̬ ��⵽X��������ʱ�л�������״̬
        if (Input.GetKeyDown(KeyCode.X))
        {
            //�������������ʱ���ѵ���������������
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
