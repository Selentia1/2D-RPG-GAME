using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Skill : MonoBehaviour
{
    [Header("Skill Info")]
    [SerializeField] public float cooldown;
    [HideInInspector] public float cooldownTimer;
    [SerializeField] public float skillDuration;
    [HideInInspector] public float skillTimer;

    protected Player player;
    public virtual void Start()
    {
        player = PlayerManger.instance.player;
    }
    public virtual void Update() {
        if(cooldownTimer >= 0){
            cooldownTimer -= Time.deltaTime;
        }
    }

    public virtual bool CkeckSkill()
    {
        //检查技能是否可用
        if (cooldownTimer <= 0)
        {
            return true;
        }
        Debug.Log("Skill is cooldown!");
        return false;
    }


    public virtual void UseSkill()
    {
        //具体如何使用技能，特效或者动做展示
    }

    public virtual bool CkeckAndUseSkill()
    {
        //检查技能是否可用，并使用技能
        if (CkeckSkill())
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }
}
