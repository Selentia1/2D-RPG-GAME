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
        //����Ѿ�������ǽ״̬���ҽ���ǽ����������Ծ����
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
        

        //������ǽ������ٶ�
        if (yInput == 0)
        {
            //������ǽ��Ȼ������ٶ�
            player.SetVelocity(rb.velocity.x, player.WallSlideFallSpeed);
        }
        else if(yInput == 1)
        {
            //������ǽ����������ٶ�
            player.SetVelocity(rb.velocity.x, player.WallSlideFallSpeed * 2);
        }

        base.Update();

        //�����ұ����ƶ�ǽ�棬��ص�����״̬
        if (xInput != 0 && (int)player.faceDirection != xInput) {
            player.stateMachine.ChangeState(player.idleState);
        }

        //�����⵽������߼�ⲻ��ǽ����ص�����״̬
        if (player.IsGroundDetected() || !player.IsWallDetected()) {
            player.stateMachine.ChangeState(player.idleState);
        }

    }
}
