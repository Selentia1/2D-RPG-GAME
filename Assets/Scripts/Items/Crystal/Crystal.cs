using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor;
using UnityEngine;

public enum CrystalType {
    Normal = 1,
    Teleport = 2,
    Pierce = 3,
    AirFloat = 4,
    FinFunne = 5,
}
public class Crystal : Item
{
    [Header("Ctystal Info")]
    [SerializeField] GameObject aimPointPrefab;
    private GameObject aimPoint;
    private CircleCollider2D circleCd;
    private Transform closestTarget;
    private string currentState;
    private Vector2 explodeAmplify;
    private float growSpeed;
    private bool explodeCanGrow;
    private bool canMove;
    private float moveSpeed;
    private CrystalType type;


    [Header("Teleport Info")]
    public bool canTeleport;
    public int canTeleportTimes;

    [Header("Pierce Info")]
    private float pierceCrystalAttackCDTimer;
    private float pierceCrystalAttackCD;

    [Header("AirFloat Info")]
    private float findTarget;
    private float radius;
    private float startAngle;
    private float endAngle;
    protected override void Awake()
    {
        circleCd = GetComponent<CircleCollider2D>();
        base.Awake();
        currentState = "Idle";
        this.existTimer = exsitDuration;
    }
    protected override void Update()
    {
        base.Update();
        if (type == CrystalType.Teleport)
        {
            CrystalLogic_Teleport();
        }
        else if (type == CrystalType.Normal)
        {
            CrystalLogic_Normal();
        }
        else if (type == CrystalType.Pierce)
        {
            CrystalLogic_Pierce();
        }
        else if (type == CrystalType.AirFloat)
        {
            CrystalTypeLogic_AirFloat();
        }
        else if (type == CrystalType.FinFunne) {
            CrystalTypeLogic_FinFunne();
        }
        
    }

    private void CrystalTypeLogic_FinFunne()
    {
        
    }

    private void CrystalTypeLogic_AirFloat()
    {
        CrystalUseTimesCheck();
        CrystalExplodeAmplifyEffect();
    }

    private void CrystalLogic_Pierce()
    {
        CrystalUseTimesCheck();
        CrystalExplodeAmplifyEffect();
        SetFlyDirection();
        CrystalMove();
        PierceAttack();

    }



    private void CrystalLogic_Normal()
    {
        CrystalUseTimesCheck();
        CrystalExplodeAmplifyEffect();
        SetFlyDirection();
        CrystalMove();
    }

    private void SetFlyDirection()
    {
        if(closestTarget)
        transform.right = closestTarget.transform.position - transform.position;
    }
    private void PierceAttack()
    {
        if (pierceCrystalAttackCDTimer <= 0)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);
            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    Direction.Dir dir = hit.transform.position.x - transform.position.x > 0 ? Direction.Dir.Right : Direction.Dir.Left;
                    hit.GetComponent<Enemy>().UnderAttack("damaged", dir, true);
                    pierceCrystalAttackCDTimer = pierceCrystalAttackCD;
                }
            }
        }
    }
    private void CrystalLogic_Teleport()
    {
        CrystalUseTimesCheck();
        CrystalExplodeAmplifyEffect();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type != CrystalType.Pierce)
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                ChangeState("Explode");
            }
        }
    }

    public void CrystalDestroy() {
        Destroy(this.gameObject);
    }
    public override void TimerController() {
        base.TimerController();
        if (existTimer > 0)
        {
            existTimer -= Time.deltaTime;
        }
        else if(existTimer <= 0){
            ChangeState("Explode");
        }

        if (pierceCrystalAttackCDTimer > 0) {
            pierceCrystalAttackCDTimer -= Time.deltaTime;
        } 
    }
    public override void AnimationController() {

        if (currentState == "Idle")
        {
            canTeleport = true;
            explodeCanGrow = false;
            canMove = true;
        }   
        else if (currentState == "Explode") {
            hasMirage = false;
            canTeleport = false;
            explodeCanGrow = true;
            canMove = false;
        }
    }

    public override void ChangeState(string state) {
        anim.SetBool(currentState, false);
        currentState = state;
        anim.SetBool(currentState, true);
    }

    public void CrystalUseTimesCheck() {
        if (canTeleportTimes <= 0) {
            ChangeState("Explode");
        }
    }
    private void CrystalMove()
    {
        if(canMove)
        {
            if(closestTarget != null)
            transform.position = Vector2.Lerp(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
        }
    }
    public void CrystalExplodeAmplifyEffect() {
        if (explodeCanGrow) {
            transform.localScale = Vector2.Lerp(transform.localScale, explodeAmplify,growSpeed * Time.deltaTime);
        }
    }
    
    public void sectorCheckClosestTarget() {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject target in targetObjects)
        {
            //获取离水晶最近的目标
            Vector2 relativeVector = target.transform.position - transform.position;
            //如果最近的距离的模长小于检测半径
            if (relativeVector.magnitude <= radius)
            {
                //继续检测角度是否正确，Mathf.Atan2返回的是以弧度制表示的该向量与 x 轴正方向所成夹角的角度值。
                //Mathf.Rad2Deg 弧度转化为角度
                float angle = Mathf.Atan2(relativeVector.y, relativeVector.x) * Mathf.Rad2Deg;
                if (angle < 0)
                {
                    //如果角度为负数转化为正值
                    angle += 360;
                }

                if (angle >= startAngle && angle <= endAngle)
                {
                    closestTarget = target.transform;
                    aimPoint = Instantiate(aimPointPrefab,target.transform.position,Quaternion.identity);
                }
            }
        }
    }
    public void SetUpCrystal(float exsitDuration, Vector2 explodeAmplify, float growSpeed, float moveSpeed, CrystalType type)
    {
        this.exsitDuration = exsitDuration;
        this.existTimer = exsitDuration;
        this.explodeAmplify = explodeAmplify;
        this.growSpeed = growSpeed;
        this.moveSpeed = moveSpeed;
        this.type = type;
    }
    public void SetUpTeleport(int canTeleportTimes) {
        this.canTeleportTimes = canTeleportTimes;
    }

    public void SetUpAirFloat(float radius, float startAngle, float endAngle)
    {
        this.radius = radius;
        this.startAngle = startAngle;
        this.endAngle = endAngle;
    }

    public void SetUpPierce(float pierceCrystalAttackCD,Transform closestTarget)
    {
        this.pierceCrystalAttackCD = pierceCrystalAttackCD;
    }

    public void SetUpNormal(Transform closestTarget)
    {
        this.closestTarget = closestTarget;
    }

    public void SetUpFinFunne(float radius, float startAngle, float endAngle)
    {
        this.radius = radius;
        this.startAngle = startAngle;
        this.endAngle = endAngle;
    }
}
