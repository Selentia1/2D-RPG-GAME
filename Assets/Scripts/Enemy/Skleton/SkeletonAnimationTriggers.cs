using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelentonAnimationTriggers : MonoBehaviour 
{
    private Skeleton skeleton;

    private void Start()
    {
        skeleton = GetComponentInParent<Skeleton>();
    }
    private void AnimationTrigger() {
        skeleton.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(skeleton.attackCheck.position, skeleton.attackCheckRadius);
        foreach (var hit in colliders){
            if (hit.GetComponent<Player>() != null) {
                EnemyStats enemyStats = skeleton.GetComponent<EnemyStats>();
                PlayerStats playerStats = hit.GetComponent<PlayerStats>();
                enemyStats.DoDamage(playerStats,"damaged", skeleton.faceDirection,true);
            }
        }
    }

    private void OpenAttackCounterWindow() => skeleton.OpenAttackCounterWindow();
    private void CloseAttackCounterWindow() => skeleton.CloseAttackCounterWindow();

}
