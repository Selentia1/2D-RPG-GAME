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
        //动画结束的触发回应在动画开始前先置为false
        triggerCalled = false;
        //设置攻击动画连击数重置时间
        player.combooResetTimer = player.combooResetDuration;
    }

    public override void Exit()
    {
        base.Exit();
        //动画播放结束后将攻击连击数+1
        player.attackComboo = (player.attackComboo + 1) % 3;
    }

    public override void Update()
    {
        base.Update();
        //播放主要攻击动画时默认X方向速度为0
        player.SetVelocity(0, rb.velocity.y);
        //攻击动画结束时触发器变为true，改变状态为 闲置状态
        if (triggerCalled)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
