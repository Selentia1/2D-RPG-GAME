using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowAttackState : PlayerUseSkillState
{
    public PlayerThrowAttackState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
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
        player.SetVelocity(0, rb.velocity.y);
    }
}
