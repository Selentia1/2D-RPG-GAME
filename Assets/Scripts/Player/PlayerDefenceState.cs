using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenceState : PlayerState
{
    public PlayerDefenceState(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isDenfend = true;
        player.defenceCDTimer = player.defenceCDDuration;
    }

    public override void Exit()
    {
        base.Exit();
        player.isDenfend = false;
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, rb.velocity.y);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(player.counterAttackCheck.position, player.counterAttackBoxSize,player.layerMask_Enemy);
        bool counterAttackSuccessful = false;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if(Input.GetKeyUp(KeyCode.C) && hit.GetComponent<Enemy>().canBeStunned && player.faceDirection != hit.GetComponent<Enemy>().faceDirection) {
                    counterAttackSuccessful = true;
                    stateMachine.ChangeState(player.counterAttackState);
                    EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                    player.characterStats.DoDamage(enemyStats,"stunned", player.faceDirection, true);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.C) && !counterAttackSuccessful) {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
