using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //���������Ĵ�����Ӧ�ڶ�����ʼǰ����Ϊfalse
        triggerCalled = false;
        //���ù�����������������ʱ��
        player.combooResetTimer = player.combooResetDuration;
    }

    public override void Exit()
    {
        base.Exit();
        //�������Ž����󽫹���������+1
        player.attackComboo = (player.attackComboo + 1) % 3;
    }

    public override void Update()
    {
        base.Update();
        //������Ҫ��������ʱĬ��X�����ٶ�Ϊ0
        player.SetVelocity(0, rb.velocity.y);
        //������������ʱ��������Ϊtrue���ı�״̬Ϊ ����״̬
        if (triggerCalled)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
