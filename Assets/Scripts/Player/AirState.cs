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
        
        //�κο���״̬ ��⵽���� �л�������״̬
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        //�κο���״̬ ��⵽ǽ�ڣ���ⲻ�����棬��x����������������һ��ʱ���뵽��ǽ״̬
        if (player.IsWallDetected() && !player.IsGroundDetected() && xInput ==(int)player.faceDirection) 
        {
            stateMachine.ChangeState(player.wallSlideDownState);
        }

        //�κο���״̬ ��⵽X��������ʱ�л�������״̬
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
