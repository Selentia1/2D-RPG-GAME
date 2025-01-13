using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Direction;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected Entity entity;
    public Stat heath;
    public Stat energy;
    public Stat armor;
    public Stat damage;
    public Stat Strength;

    [SerializeField] private int currentHeath;
    [SerializeField] private int currentArmor;
    [SerializeField] private int currentEnergy;

    [Header("Buff")]
    public List<Buff> currentbuffs;

    public virtual void Start()
    { 
        entity = GetComponent<Entity>();
        currentHeath = heath.getValue();
        currentEnergy = energy.getValue();
        currentArmor = armor.getValue();
    }

    public virtual void Update()
    {

    }

    protected virtual void Die()
    {

    }

    public virtual void DoDamage(CharacterStats targetStats,string attackType,Direction.Dir dir,bool konckback) { 
        int totalDamage = damage.getValue() + Strength.getValue();
        targetStats.TakeDamage(totalDamage,attackType,dir, konckback);
    }
    public virtual void TakeDamage(int _damage, string attackType, Direction.Dir dir, bool konckback) 
    {
        Debug.Log(gameObject.name + " was damage :" +  _damage);
        currentHeath -= _damage;
        currentHeath = currentHeath > 0 ? currentHeath : 0;
        if (currentHeath <= 0)
        {
            Die();
        }
        entity.UnderAttack(attackType, dir, konckback);
    }
}
