using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchSowrdState : ThrowAttackState
{
    public CatchSowrdState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.instance.throwSword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.instance.throwSword.DotsActive(false);
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.V))
        {
            stateMachine.ChangeState(player.throwSwordState);
        }
    }
}
