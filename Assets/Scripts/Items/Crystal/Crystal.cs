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

    [Header("AirFloat && FinFunne Info")]
    [SerializeField] GameObject aimPointPrefab;
    [SerializeField] private GameObject aimPoint;
    public int crystalNumber = -1;
    public bool crystalLock = true;
    [SerializeField] private float startAngle;
    [SerializeField] private float endAngle;
    private float startAngleR;
    private float startAngleL;
    private float endAngleR;
    private float endAngleL;
    private Transform crystalPoint;
    private bool crystalFollow = true;
    public bool crystalFindEnemy = false;

    [Header("AirFloat Info")]
    private float airFloatCheckRadius;
    private float crystalRiseTimer;
    private float crystalRiseDuration;
    private Vector2 crystalRiseSpeed;
    private float followSpeed;
    private float traceSpeed;

    [Header("FinFunne Info")]
    private float finFunneCheckRadius;
    [SerializeField] private GameObject rayAttackPrefab;
    [SerializeField] private GameObject attackRay;
    [SerializeField] private LayerMask mask_Enemy;
    private float rayAttackCD;
    

    protected override void Awake()
    {
        circleCd = GetComponent<CircleCollider2D>();
        base.Awake();
        currentState = "Idle";
        this.existTimer = exsitDuration;
        mask_Enemy = 1 << 7;
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

        //优化幻影效果
        if (type == CrystalType.FinFunne || type == CrystalType.AirFloat) {
            if (crystalFollow) {
                if (Mathf.Abs(crystalPoint.transform.position.x - transform.position.x) < 1f)
                {
                    hasMirage = false;
                }
                else
                {
                    hasMirage = true;
                }
            }
        }
    }
    private void CrystalLogic_Teleport()
    {
        CrystalUseTimesCheck();
        isExplode();
    }
    private void CrystalTypeLogic_FinFunne()
    {
        isExplode();
        CrystalFollowPlayer();
        DirectionChangeCheck();
        SetAimDirection();
        SectorCheckClosestTarget();
        if (!crystalLock) {
            FinFunneAttack();
        }
    }

    private void CrystalTypeLogic_AirFloat()
    {

        isExplode();
        CrystalFollowPlayer();
        DirectionChangeCheck();
        CrystalAttack();
        SetAimDirection();
        SectorCheckClosestTarget();
        if (!crystalLock)
        {
            CrystalTraceEnemy();
        }

    }

    private void CrystalLogic_Pierce()
    {
        isExplode();
        SetAimDirection();
        CrystalMove();
        PierceAttack();
    }



    private void CrystalLogic_Normal()
    {
        isExplode();
        SetAimDirection();
        CrystalMove();
        CrystalAttack();
    }

    private void SetAimDirection()
    {
        if (closestTarget)
        {
            transform.right = closestTarget.transform.position - transform.position;
        }
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
                    EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                    PlayerStats playerStats = PlayerManager.instance.stats;
                    playerStats.DoDamage(enemyStats,"damaged", dir, true);
                    pierceCrystalAttackCDTimer = pierceCrystalAttackCD;
                }
            }
        }
    }

    private void CrystalAttack() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (Mathf.Abs(hit.transform.position.x - transform.position.x) < 0.3f)
                {
                    if (type == CrystalType.AirFloat)
                    {
                        crystalFindEnemy = false;
                        crystalFollow = false;

                        if (aimPoint != null)
                        {
                            aimPoint.GetComponent<AimPoint>().AimTargetEffect(false);
                            Destroy(aimPoint.gameObject);
                        }
                    }
                    ChangeState("Explode");
                }
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

        if (crystalRiseTimer > 0 && crystalFindEnemy) { 
            crystalRiseTimer -= Time.deltaTime;
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
    public void isExplode() {
     
        if (currentState == "Explode") {
            //爆炸时放大特效
            if (type == CrystalType.FinFunne) {
                
                if (aimPoint != null)
                {
                    Destroy(aimPoint);
                }

                if (attackRay != null)
                {
                    attackRay.SetActive(false); 
                }
            }
            if (explodeCanGrow)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, explodeAmplify, growSpeed * Time.deltaTime);

            }

        }

    }
    
    public void SectorCheckClosestTarget() {
        if (type == CrystalType.AirFloat)
        {
            //悬浮魔晶
            if (crystalFollow)
            {
                GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject target in targetObjects)
                {
                    //获取离水晶最近的目标
                    Vector2 relativeVector = target.transform.position - transform.position;
                    //如果最近的距离的模长小于检测半径
                    if (relativeVector.magnitude <= airFloatCheckRadius)
                    {
                        //继续检测角度是否正确，Mathf.Atan2返回的是以弧度制表示的该向量与 x 轴正方向所成夹角的角度值。
                        //Mathf.Rad2Deg 弧度转化为角度
                        float angle = Mathf.Atan2(relativeVector.y, relativeVector.x) * Mathf.Rad2Deg;
                        angle = NormalizeAngle(angle);
                        if (startAngle <= endAngle)
                        {
                            if (startAngle <= angle && angle <= endAngle)
                            {
                                closestTarget = target.transform;
                                if (!crystalLock)
                                {
                                    crystalFindEnemy = true;
                                    crystalFollow = false;
                                }
                            }
                        }
                        else if (startAngle <= angle || angle <= endAngle)
                        {

                            closestTarget = target.transform;
                            if (!crystalLock)
                            {
                                crystalFindEnemy = true;
                                crystalFollow = false;
                            }
                        }
                    }
                }
            }
        }
        else if (type == CrystalType.FinFunne) {
            //浮游炮
            if (crystalFollow)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, finFunneCheckRadius,mask_Enemy);

                if (colliders.Length == 0) {
                    closestTarget = null;
                    crystalFindEnemy = false;
                    Destroy(aimPoint);
                    Destroy(attackRay);
                }

                float minDistance = Mathf.Infinity;
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        float distance = Vector2.Distance(transform.position, hit.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestTarget = hit.transform;
                            crystalFindEnemy = true;
                        }
                    }
                }
            }
        }
    }

    private void DirectionChangeCheck()
    {
        if (type == CrystalType.AirFloat)
        {
            if (PlayerManager.instance.player.faceDirection == Direction.Dir.Right && !crystalFindEnemy)
            {
                startAngle = startAngleR;
                endAngle = endAngleR;
                spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                startAngle = startAngleL;
                endAngle = endAngleL;
                spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
        }
        else if (type == CrystalType.FinFunne){
            if (PlayerManager.instance.player.faceDirection == Direction.Dir.Right)
            {
                spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
        }

    }
    public void CrystalFollowPlayer() {
        if (crystalFollow) {
            if (crystalNumber % 2 != 0) {
                transform.position = Vector2.Lerp(transform.position, crystalPoint.position - new Vector3(-0.2f * (int)PlayerManager.instance.player.faceDirection, crystalNumber * 0.15f), Time.deltaTime * followSpeed);
            }
            else {
                transform.position = Vector2.Lerp(transform.position, crystalPoint.position - new Vector3(0,  crystalNumber * 0.15f),Time.deltaTime * followSpeed);
            }

        }
    }
    public void CrystalTraceEnemy()
    {
        if (crystalFindEnemy) {
            if (crystalRiseTimer > 0)
            {
                rb.velocity = new Vector2(crystalRiseSpeed.x * (int)PlayerManager.instance.player.faceDirection, crystalRiseSpeed.y);
                transform.right = rb.velocity;
            }
            else {
                rb.velocity = new Vector2(0, 0);
                InitAimPoint();
                if (aimPoint != null)
                {
                    aimPoint.GetComponent<AimPoint>().AimTargetEffect(true);
                }
                transform.position = Vector2.Lerp(transform.position, closestTarget.position, traceSpeed * Time.deltaTime);
            }
        }
    }

    private void FinFunneAttack()
    {
        if (crystalFindEnemy)
        {
            FinFunneSetAim();
            RaySet();
        }
    }

    private void FinFunneSetAim()
    {
        //设置目标
        InitAimPoint();
        if (closestTarget != null)
        {
            aimPoint.transform.position = closestTarget.position;
            aimPoint.transform.parent = closestTarget;
            if (aimPoint != null)
            {
                aimPoint.GetComponent<AimPoint>().AimTargetEffect(true);
            }
        }
        

    }

    private void RaySet()
    {

        if (closestTarget != null && attackRay == null)
        {
            attackRay = Instantiate(rayAttackPrefab,transform.position,transform.rotation);
            attackRay.transform.parent = transform;
            attackRay.GetComponent<BlueAttackRay>().attackCD = rayAttackCD;
        }

        if (closestTarget != null)
        {
           
            float rayLength = attackRay.GetComponent<BoxCollider2D>().size.x;
            attackRay.transform.localScale = new Vector3(Vector2.Distance(transform.position, closestTarget.position) / rayLength, 1.5f, 1);
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

    public void SetUpAirFloat(float airFloatCheckRadius, float startAngle, float endAngle,Transform crystalPoint,float followSpeed, float crystalRiseDuration, Vector2 crystalRiseSpeed,float traceSpeed)
    {
        this.airFloatCheckRadius = airFloatCheckRadius;
        this.startAngle = startAngle;
        this.endAngle = endAngle;
        this.crystalPoint = crystalPoint;
        this.followSpeed = followSpeed;
        this.startAngleR = startAngle;
        this.endAngleR = endAngle;
        this.crystalRiseDuration = crystalRiseDuration;
        this.crystalRiseTimer = this.crystalRiseDuration;
        this.crystalRiseSpeed = crystalRiseSpeed;
        this.traceSpeed = traceSpeed;

        startAngleL = 180 - startAngleR;
        endAngleL = 180 - endAngleR;
        startAngleL = NormalizeAngle(startAngleL);
        endAngleL = NormalizeAngle(endAngleL);
        startAngleR = NormalizeAngle(startAngleR);
        endAngleR = NormalizeAngle(endAngleR);
        if (startAngleL > endAngleL) {
            float temp = startAngleL;
            startAngleL = endAngleL;
            endAngleL = temp;
        }

    }

    public void SetUpPierce(float pierceCrystalAttackCD,Transform closestTarget)
    {
        this.pierceCrystalAttackCD = pierceCrystalAttackCD;
        this.closestTarget = closestTarget;
    }

    public void SetUpNormal(Transform closestTarget)
    {
        this.closestTarget = closestTarget;
    }

    public void SetUpFinFunne(float finFunneCheckRadius, float startAngle, float endAngle, Transform crystalPoint, float followSpeed,float rayAttackCD)
    {
        this.finFunneCheckRadius = finFunneCheckRadius;
        this.crystalPoint = crystalPoint;
        this.followSpeed = followSpeed;
        this.rayAttackCD = rayAttackCD;
    }

    public static float NormalizeAngle(float angle)
    {
        while (angle < 0)
        {
            angle += 360;
        }
        while (angle >= 360)
        {
            angle %= 360;
        }
        return angle;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (aimPoint != null) { 
            Destroy(aimPoint.gameObject);
        }
    
    }

    private void InitAimPoint() {
        if (closestTarget != null && aimPoint == null)
        {
            aimPoint = Instantiate(aimPointPrefab, closestTarget.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            aimPoint.transform.parent = closestTarget;
        }
    }
}
