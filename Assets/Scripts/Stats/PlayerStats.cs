using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    public override void Start()
    {
        base.Start();
        player = (Player)entity;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void DoDamage(CharacterStats targetStats, string attackType, Direction.Dir dir, bool konckback)
    {
        base.DoDamage(targetStats, attackType, dir, konckback);
    }

    protected override void Die()
    {
        player.Die();
    }

    public override void TakeDamage(int _damage, string attackType, Direction.Dir dir, bool konckback)
    {
        base.TakeDamage(_damage, attackType, dir, konckback);
    }
}
