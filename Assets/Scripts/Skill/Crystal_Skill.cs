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
    [SerializeField] private Transform crystalPoint;
    [SerializeField] private Transform closestTraget;
    [SerializeField] private CrystalType type;
    [SerializeField] private Vector2 explodeAmplify;
    [SerializeField] private float growSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float normalCrystalSkillCooldown;

    [Header("Teleprot Info")]
    public int canTeleportTimes;
    [SerializeField] private float teleprotCrystalskillDuration;
    [SerializeField] private float teleprotCrystalSkillCooldown;


    [Header("Pierce Info")]
    [SerializeField] private float pierceCrystalAttackCD;
    [SerializeField] private float pierceCrystalskillDuration;
    [SerializeField] private float pierceCrystalSkillCooldown;

    [Header("Crystal Air Float & Fin Funne Info")]
    [SerializeField] public List<Crystal> crystalList;
    
    [SerializeField] private float startAngle;
    [SerializeField] private float endAngle;
    [SerializeField] private float followSpeed;

    [Header("Air Float Info")]
    [SerializeField] private float airFloatCheckRadius;
    [SerializeField] private int airFloatCrystalCount;
    [SerializeField] private float airFloatCrystalskillDuration;
    [SerializeField] private float crystalRiseDuration;
    [SerializeField] private Vector2 crystalRiseSpeed;
    [SerializeField] private float traceSpeed;
    [SerializeField] private float airFloatCrystalSkillCooldown;

    [Header("Fin Funne Info")]
    [SerializeField] private float finFunneCheckRadius;
    [SerializeField] private int finFunneCrystalCount;
    [SerializeField] private float finFunneCrystalskillDuration;
    [SerializeField] private float finFunneCrystalSkillCooldown;
    [SerializeField] private int attackAmounts;
    [SerializeField] private float rayAttackCD;

    public Crystal CreateCryStal() {
        GameObject crystal = Instantiate(crystalPrefab, player.transform.position + (int)player.faceDirection * new Vector3(1, 0, 0), Quaternion.identity);
        closestTraget = FindClosestEnemy(crystal.transform);
        switch (type) {
            case CrystalType.Teleport:
                crystal.GetComponent<Crystal>().SetUpCrystal(teleprotCrystalskillDuration,explodeAmplify,growSpeed,moveSpeed,type);
                crystal.GetComponent<Crystal>().SetUpTeleport(canTeleportTimes);
                cooldown = teleprotCrystalSkillCooldown;
                break;
             case CrystalType.Pierce:
                crystal.GetComponent<Crystal>().SetUpCrystal(pierceCrystalskillDuration, explodeAmplify, growSpeed,moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpPierce(pierceCrystalAttackCD,closestTraget);
                cooldown = pierceCrystalSkillCooldown;
                break;
            case CrystalType.Normal:
                crystal.GetComponent<Crystal>().SetUpCrystal(skillDuration, explodeAmplify, growSpeed,moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpNormal(closestTraget);
                cooldown = normalCrystalSkillCooldown;
                break;
            case CrystalType.AirFloat:
                crystal.GetComponent<Crystal>().SetUpCrystal(airFloatCrystalskillDuration, explodeAmplify, growSpeed,moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpAirFloat(airFloatCheckRadius, startAngle, endAngle,crystalPoint,followSpeed,crystalRiseDuration,crystalRiseSpeed,traceSpeed);
                cooldown = airFloatCrystalSkillCooldown;
                break;
            case CrystalType.FinFunne:
                crystal.GetComponent<Crystal>().SetUpCrystal(finFunneCrystalskillDuration, explodeAmplify, growSpeed,moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpFinFunne(finFunneCheckRadius, startAngle, endAngle, crystalPoint, followSpeed, rayAttackCD);
                cooldown  = finFunneCrystalSkillCooldown;
                break;
        }
        skillTimer = cooldown;
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
                cooldown = teleprotCrystalSkillCooldown;
                break;
            case CrystalType.Pierce:
                crystal.GetComponent<Crystal>().SetUpCrystal(pierceCrystalskillDuration, explodeAmplify, growSpeed, moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpPierce(pierceCrystalAttackCD, closestTraget);
                cooldown = pierceCrystalSkillCooldown;
                break;
            case CrystalType.Normal:
                crystal.GetComponent<Crystal>().SetUpCrystal(skillDuration, explodeAmplify, growSpeed, moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpNormal(closestTraget);
                cooldown = normalCrystalSkillCooldown;
                break;
            case CrystalType.AirFloat:
                crystal.GetComponent<Crystal>().SetUpCrystal(airFloatCrystalskillDuration, explodeAmplify, growSpeed, moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpAirFloat(airFloatCheckRadius, startAngle, endAngle,crystalPoint,followSpeed,crystalRiseDuration, crystalRiseSpeed,traceSpeed);
                cooldown = airFloatCrystalSkillCooldown;
                break;
            case CrystalType.FinFunne:
                crystal.GetComponent<Crystal>().SetUpCrystal(finFunneCrystalskillDuration, explodeAmplify, growSpeed, moveSpeed, type);
                crystal.GetComponent<Crystal>().SetUpFinFunne(finFunneCheckRadius, startAngle, endAngle, crystalPoint, followSpeed,rayAttackCD);
                cooldown = finFunneCrystalSkillCooldown;
                break;
        }
        return crystal.GetComponent<Crystal>();
    }

    public override bool CkeckSkill()
    {
        //检查技能是否可用
        if (type != CrystalType.Teleport)
        {
            if (cooldownTimer <= 0)
            {
                if (type != CrystalType.Teleport && closestTraget != null)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
        }
        else {
            if (cooldownTimer <= 0 || currentCrystal.canTeleportTimes > 0) {
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
                cooldownTimer = cooldown;
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
        else if (type == CrystalType.AirFloat)
        {
            crystalList.Clear();
            for (int i = 0; i < airFloatCrystalCount; i++)
            {
                Vector3 offset;
                if (i % 2 != 0)
                {
                    offset = new Vector3((int)player.faceDirection * (-1f) * 0.8f, 1f - 0.15f * i, 0);
                }
                else
                {
                    offset = new Vector3((int)player.faceDirection * (-1f) * 1f, 1f - 0.15f * i, 0);
                }
                Crystal crystal = CreateCryStal(offset);
                crystal.crystalNumber = i;
                crystalList.Add(crystal);
            }

            List<Crystal> tempList = new List<Crystal>();
            foreach (Crystal tempCrystal in crystalList)
            {
                tempList.Add(tempCrystal);
            }
            StartCoroutine("CrystalLock", tempList);
        }
        else if (type == CrystalType.FinFunne) {
            crystalList.Clear();
            for (int i = 0; i < finFunneCrystalCount; i++)
            {
                Vector3 offset;
                if (i % 2 != 0)
                {
                    offset = new Vector3((int)player.faceDirection * (-1f) * 0.8f, 1f - 0.15f * i, 0);
                }
                else
                {
                    offset = new Vector3((int)player.faceDirection * (-1f) * 1f, 1f - 0.15f * i, 0);
                }
                Crystal crystal = CreateCryStal(offset);
                crystal.crystalNumber = i;
                crystalList.Add(crystal);
            }

            List<Crystal> tempList = new List<Crystal>();
            foreach (Crystal tempCrystal in crystalList)
            {
                tempList.Add(tempCrystal);
            }
            StartCoroutine("CrystalLock", tempList);
        }

        if (type != CrystalType.Teleport) {
            base.UseSkill();

        }
    }

    public override bool CkeckAndUseSkill()
    {
        return base.CkeckAndUseSkill();
    }
    public bool PlayerNeedChangeState()
    {
        if (type == CrystalType.Teleport && currentCrystal != null)
        {
            return false;
        }
        return true;
    }
    public override void Update()
    {
        base.Update(); 

    }

    public override void Start()
    {
        base.Start();
        crystalPoint = player.transform.Find("CrystalPoint");
    }

    public IEnumerator CrystalLock (List<Crystal> crystalList)
    {
        if (type == CrystalType.AirFloat)
        {
            int crystalIndex = 0;
            while (crystalIndex < crystalList.Count)
            {

                if (crystalList[crystalIndex] != null)
                {
                    crystalList[crystalIndex].crystalLock = false;
                }
                else
                {
                    crystalIndex++;
                }
                yield return new WaitForFixedUpdate();
            }

        } else if (type == CrystalType.FinFunne) {
            int crystalIndex = 0;
            if (crystalList.Count % 2 == 0) {
                crystalIndex = crystalList.Count / 2 - 1;
            }
            else if (crystalList.Count % 2 == 1) {
                crystalIndex = (crystalList.Count - 1) / 2;
            }
            crystalList[crystalIndex].crystalLock = false;
        }
    }
}
