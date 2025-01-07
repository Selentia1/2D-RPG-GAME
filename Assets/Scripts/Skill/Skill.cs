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
        //��鼼���Ƿ����
        if (cooldownTimer <= 0)
        {
            return true;
        }
        Debug.Log("Skill is cooldown!");
        return false;
    }


    public virtual void UseSkill()
    {
        //�������ʹ�ü��ܣ���Ч���߶���չʾ
    }

    public virtual bool CkeckAndUseSkill()
    {
        //��鼼���Ƿ���ã���ʹ�ü���
        if (CkeckSkill())
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }
}
