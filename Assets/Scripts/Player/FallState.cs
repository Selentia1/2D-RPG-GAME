using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : AirState
{
    public FallState(Player player, PlayerStateMachine stateMachine, string animStateName) : base(player, stateMachine, animStateName)
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
        
        //检测到y方向的速度大于零时切换到上升状态
        if (rb.velocity.y>0) {
           stateMachine.ChangeState(player.riseState);
        }
    }
}
