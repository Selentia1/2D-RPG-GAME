using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.isDead = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.isDead = false;
    }

    public override void Update()
    {
        player.SetVelocity(0, rb.velocity.y);
    }
}
