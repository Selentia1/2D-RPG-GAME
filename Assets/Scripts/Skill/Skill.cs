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
        player = PlayerManager.instance.player;
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
        return false;
    }


    public virtual void UseSkill()
    {
        cooldownTimer = cooldown;
        //�������ʹ�ü��ܣ���Ч���߶���չʾ
    }

    public virtual bool CkeckAndUseSkill()
    {
        //��鼼���Ƿ���ã���ʹ�ü���
        if (CkeckSkill())
        {
            UseSkill();
            return true;
        }
        return false;
    }

    public virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector3.Distance(_checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }
}
