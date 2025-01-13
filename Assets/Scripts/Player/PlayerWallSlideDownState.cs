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
        //����Ѿ�������ǽ״̬��������Ծ����
        player.jumpTimes = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        //�޸���ǽ״̬�³�̷��������
        if (Input.GetKeyDown(KeyCode.Z) && SkillManager.instance.dash.CkeckSkill())
        {
            
        }

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

        //�����⵽���� ���� ��ұ����ƶ�ǽ�� �������Զ����µ��ٶȶ�����ǽ�� ��ص�����״̬
        if (player.IsGroundDetected() || (xInput != 0 && (int)player.faceDirection != xInput) || !player.IsWallDetected()) {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
