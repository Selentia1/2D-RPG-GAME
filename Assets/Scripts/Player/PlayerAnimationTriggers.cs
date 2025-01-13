using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour 
{
    private Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }
    private void AnimationTrigger() {
        player.AnimationTrigger();
    }

    private void Attack_01_Trigger() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attack_01_Check.position, player.attack_01_Check_Radius);
        foreach (var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null) {
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                PlayerStats playerStats = PlayerManager.instance.stats;
                playerStats.DoDamage(enemyStats,"damaged", player.faceDirection,true);
            }
        }
    }

    private void Attack_02_Trigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attack_02_Check.position,player.attack_02_Check_Radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                PlayerStats playerStats = PlayerManager.instance.stats;
                playerStats.DoDamage(enemyStats, "damaged", player.faceDirection, true);
            }
        }
    }

    private void Attack_03_Trigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attack_03_Check.position, player.attack_03_Check_Radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                PlayerStats playerStats = PlayerManager.instance.stats;
                playerStats.DoDamage(enemyStats, "damaged", player.faceDirection, true);
            }
        }
    }

    private void CounterAttackTrigger() {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(player.counterAttackCheck.position, player.counterAttackBoxSize, player.layerMask_Enemy);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().UnderAttack("damaged", player.faceDirection, true);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.throwSword.CkeckAndUseSkill();
    }
}
