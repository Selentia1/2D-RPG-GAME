using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UIElements;

public class Crystal_Skill : Skill
{

    [Header("Crystal Info")]
    public GameObject crystalPrefab;
    public Crystal currentCrystal;
    [SerializeField] private Transform closestTraget;
    [SerializeField] private CrystalType type;
    [SerializeField] private Vector2 explodeAmplify;
    [SerializeField] private float growSpeed;
    [SerializeField] private float moveSpeed;

    [Header("Teleprot Info")]
    public int canTeleportTimes;
    [SerializeField] private float teleprotCrystalskillDuration;


    [Header("Pierce Info")]
    [SerializeField] private float pierceCrystalAttackCD;
    [SerializeField] private float pierceCrystalskillDuration;

    [Header("Air Float Info")]
    [SerializeField] int crystalCount;
    [SerializeField] List<Crystal> crystalList;
    [SerializeField] private float airFloatCrystalskillDuration;
    [SerializeField] private float radius;
    [SerializeField] private float startAngle;
    [SerializeField] private float endAngle;

    [Header("Fin Funne Info")]
    [SerializeField] private float finFunneCrystalskillDuration;
    public Crystal CreateCryStal() {
        GameObject crystal = Instantiate(crystalPrefab, player.transform.position + (int)player.faceDirection * new Vector3(1, 0, 0), Quaternion.identity);
        closestTraget = FindClosestEnemy(crystal.transform);
        switch (type) {
            case CrystalType.Teleport:
                crystal.GetComponent<Crystal>().SetUpCrystal(teleprotCrystalskillDuration,explodeAmplify,growSpeed,moveSpeed,type);
                crystal.GetComponent<Crystal>().SetUpTeleport(canTeleportTimes);
                break;
             case CrystalType.Pierce:
                crystal.GetComponent<Crystal>().SetUpCrystal(pierceCrystalskillDuration, explodeAmplify, growSpeed,moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpPierce(pierceCrystalAttackCD,closestTraget);
                break;
            case CrystalType.Normal:
                crystal.GetComponent<Crystal>().SetUpCrystal(teleprotCrystalskillDuration, explodeAmplify, growSpeed,moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpNormal(closestTraget);
                break;
            case CrystalType.AirFloat:
                crystal.GetComponent<Crystal>().SetUpCrystal(airFloatCrystalskillDuration, explodeAmplify, growSpeed,moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpAirFloat(radius, startAngle, endAngle);
                break;
            case CrystalType.FinFunne:
                crystal.GetComponent<Crystal>().SetUpCrystal(finFunneCrystalskillDuration, explodeAmplify, growSpeed,moveSpeed, type);
                break;
                
        }
        return crystal.GetComponent<Crystal>();
    }

    public Crystal CreateCryStal(Vector3 offset)
    {
        GameObject crystal = Instantiate(crystalPrefab, player.transform.position + offset, Quaternion.identity);
        closestTraget = FindClosestEnemy(crystal.transform);
        switch (type)
        {
            case CrystalType.Teleport:
                crystal.GetComponent<Crystal>().SetUpCrystal(teleprotCrystalskillDuration, explodeAmplify, growSpeed, moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpTeleport(canTeleportTimes);
                break;
            case CrystalType.Pierce:
                crystal.GetComponent<Crystal>().SetUpCrystal(pierceCrystalskillDuration, explodeAmplify, growSpeed, moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpPierce(pierceCrystalAttackCD, closestTraget);
                break;
            case CrystalType.Normal:
                crystal.GetComponent<Crystal>().SetUpCrystal(teleprotCrystalskillDuration, explodeAmplify, growSpeed, moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpNormal(closestTraget);
                break;
            case CrystalType.AirFloat:
                crystal.GetComponent<Crystal>().SetUpCrystal(airFloatCrystalskillDuration, explodeAmplify, growSpeed, moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpAirFloat(radius, startAngle, endAngle);
                break;
            case CrystalType.FinFunne:
                crystal.GetComponent<Crystal>().SetUpCrystal(finFunneCrystalskillDuration, explodeAmplify, growSpeed, moveSpeed, type);
                break;

        }
        return crystal.GetComponent<Crystal>();
    }

    public override bool CkeckSkill()
    {
        //检查技能是否可用
        if (cooldownTimer <= 0)
        {
            if (type != CrystalType.Teleport && closestTraget != null) { 
                return true;
            }
            else{
                return true;
            }
        }
        return false;
    }
        
    public override void UseSkill()
    {
        if (type == CrystalType.Teleport)
        {
            if (currentCrystal == null)
            {
                currentCrystal = CreateCryStal();
                currentCrystal.GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.identity;
            }
            else if (currentCrystal != null && currentCrystal.canTeleport && currentCrystal.canTeleportTimes > 0)
            {
                Vector2 position = player.transform.position;
                player.transform.position = currentCrystal.transform.position;
                currentCrystal.transform.position = position;
                currentCrystal.canTeleportTimes--;
            }
        }
        else if (type == CrystalType.Normal)
        {
            closestTraget = null;
            Crystal crystal = CreateCryStal();
        }
        else if (type == CrystalType.Pierce)
        {
            Crystal crystal = CreateCryStal();
        }
        else if (type == CrystalType.AirFloat) { 
            for (int i = 0;i<crystalCount;i++)
            {
                Vector3 offset = (int)player.faceDirection * (-1f) * new Vector3(1f, 1f - 0.1f * i, 0);
                crystalList.Add(CreateCryStal(offset));
            }
        }
    }

    public override bool CkeckAndUseSkill()
    {
        return base.CkeckAndUseSkill();
    }

    public override void Update()
    {
        base.Update();
    }

}
