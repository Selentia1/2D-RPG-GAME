using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseState : AirState
{
    public RiseState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //进入到上升状态时获得一个y方向的速度=玩家的跳跃力
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        //y方向的速度小于零时切换到下落状态
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        base.Update();
    }
}

