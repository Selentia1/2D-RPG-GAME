using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlackHoleType { 
    SwordDance,
    Explode
}
public class BlackHole_Skill : Skill
{
    [Header("BlackHole Info")]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] public float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float backSpeed;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] public BlackHoleType type;
    public BlackHole blackHoleScript;

    public void CreateBlackHole() {
        GameObject blackHole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);
        blackHole.GetComponent<BlackHole>().Init(maxSize,skillDuration,growSpeed,backSpeed,amountOfAttacks, cloneAttackCooldown,type);
        this.blackHoleScript = blackHole.GetComponent<BlackHole>();
    }

    public override void UseSkill()
    {
        CreateBlackHole();
        cooldownTimer = cooldown;
    }

    public override bool CkeckSkill()
    {
        return base.CkeckSkill();
    }

    public override bool CkeckAndUseSkill()
    {
        return base.CkeckAndUseSkill();
    }
}
