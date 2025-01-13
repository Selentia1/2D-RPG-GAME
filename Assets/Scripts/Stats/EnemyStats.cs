using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

        
    public override void Start()
    {
        base.Start();
        enemy = (Enemy)entity;
    }



    public override void Update()
    {
        base.Update();
    }

    protected override void Die()
    {
        base.Die();
    }

    public override void TakeDamage(int _damage, string attackType, Direction.Dir dir, bool konckback)
    {
        base.TakeDamage(_damage, attackType, dir, konckback);
    }

    public override void DoDamage(CharacterStats targetStats, string attackType, Direction.Dir dir, bool konckback)
    {
        base.DoDamage(targetStats, attackType, dir, konckback);
    }
}
