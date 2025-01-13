using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowSwordState : PlayerThrowAttackState
{
    public PlayerThrowSwordState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
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
