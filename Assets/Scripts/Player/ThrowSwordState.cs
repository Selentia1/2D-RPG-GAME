using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSwordState : ThrowAttackState
{
    public ThrowSwordState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        triggerCalled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled) {
            stateMachine.ChangeState(player.idleState);
        }
    }

}
