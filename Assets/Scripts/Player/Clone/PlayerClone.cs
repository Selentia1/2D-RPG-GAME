using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    private SpriteRenderer sr;
    private Clone_Skill cloneSkill;
    private Animator animator;

    [Header("Clone Info")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private float existDuration;
    [SerializeField] private float existTimer;
    
    [SerializeField] private string cloneState;
    [SerializeReference] private Direction.Dir faceDirection = Direction.Dir.Right;

    [Header("Clone Dash Info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTimer;

    [Header("Clone After Image Info")]
    [SerializeField] private float AfterImageLosingSpeed;

    [Header("Clone Attack Check")]

    //普通攻击检测范围
    [SerializeField] public LayerMask layerMask_Enemy;
    [SerializeField] public Transform attack_01_Check;
    [SerializeField] public float attack_01_Check_Radius;

    [SerializeField] public Transform attack_02_Check;
    [SerializeField] public float attack_02_Check_Radius;

    [SerializeField] public Transform attack_03_Check;
    [SerializeField] public float attack_03_Check_Radius;
    public virtual void Awake() {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attack_01_Check = transform.Find("Attack01Check");
        attack_02_Check = transform.Find("Attack02Check");
        attack_03_Check = transform.Find("Attack03Check");
    }

    public virtual void Start()
    {
        Filp();
        ChangeState(cloneState);
    }

    public virtual void Update()
    {
        TimerController();
        AnimationController();

        //克隆体消失效果
        if (existTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * AfterImageLosingSpeed);
            if (sr.color.a <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attack_01_Check.position, attack_01_Check_Radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attack_02_Check.position, attack_02_Check_Radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attack_03_Check.position, attack_03_Check_Radius);
    }

    public void InitClone(Transform cloneTransform,Clone_Skill cloneSkill,string cloneState,Direction.Dir faceDirection) {
        this.faceDirection = faceDirection;
        transform.position = cloneTransform.position;
        this.cloneState = cloneState;
        this.cloneSkill = cloneSkill;
        existDuration = cloneSkill.skillDuration;
        existTimer = cloneSkill.skillTimer;
        existTimer = existDuration;
        ChangeState(cloneState);
    }

    public void InitClone(Transform cloneTransform, Clone_Skill cloneSkill, string cloneState, Direction.Dir faceDirection,Vector3 offset)
    {
        this.faceDirection = faceDirection;
        transform.position = cloneTransform.position + offset;
        this.cloneState = cloneState;
        this.cloneSkill = cloneSkill;
        existDuration = cloneSkill.skillDuration;
        existTimer = cloneSkill.skillTimer;
        existTimer = existDuration;
        ChangeState(cloneState);
    }

    private void AnimationController() {
        SetVelocity(0, 0);
        if (this.cloneState == "DashAttack")
        {
            if (dashTimer > 0)
            {
                if (isDetectedEnemy())
                {
                    animator.SetBool("Dash", false);
                }
                else {
                    animator.SetBool("Dash", true);
                    SetVelocity(dashSpeed * (int)faceDirection, rb.velocity.y);
                }
            }
            else if (dashTimer < 0)
            {
                animator.SetBool("Dash", false);
            }
        }
        else if (this.cloneState == "Dash")
        {
            if (dashTimer > 0)
            {
                SetVelocity(dashSpeed * (int)faceDirection, rb.velocity.y);
            }
        }
    }
    private void TimerController() {
        existTimer -= Time.deltaTime;
        if(dashTimer > 0)
        dashTimer -= Time.deltaTime;
    }


    private void ResetAnimParameters(string cloneState)
    {
        if (this.cloneState == "Idle")
        {
            animator.SetBool("Idle", false);
        }
        else if (this.cloneState == "Attack")
        {
            animator.SetBool("Attack", false);
            animator.SetInteger("AttackComboo", Random.Range(0, 2));
        }
        else if (this.cloneState == "DashAttack")
        {
            animator.SetBool("Dash", false);
            animator.SetBool("DashAttack", false);

        }
        else if (this.cloneState == "Dash")
        {
            animator.SetBool("Dash", false);
        }
        else if (this.cloneState == "DashAfterImage")
        {
            animator.SetBool("Dash", false);
        }
    }

    private void ChangeState(string state) {
        ResetAnimParameters(cloneState);
        this.cloneState = state;
        if (this.cloneState == "Idle")
        {
            animator.SetBool("Idle", true);
        }
        else if (this.cloneState == "Attack")
        {
            animator.SetBool("Attack", true);
            animator.SetInteger("AttackComboo", Random.Range(0, 2));
        }
        else if (this.cloneState == "DashAttack")
        {
            dashTimer = dashDuration;
            animator.SetBool("Dash", true);
            animator.SetBool("DashAttack", true);
            
        }
        else if (this.cloneState == "Dash")
        {
            dashTimer = dashDuration;
            animator.SetBool("Dash", true);
        }
        else if (this.cloneState == "DashAfterImage")
        {
            animator.SetBool("Dash", true);
            existTimer = -0.1f;
        }
    }

    public void SetVelocity(float x,float y) {
        rb.velocity = new Vector2(x, y);
    }

    private void Filp()
    {
        if (faceDirection == Direction.Dir.Left)
        {
            transform.Rotate(0, 180, 0);
        }
    }
    private void Attack_01_Trigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attack_01_Check.position, attack_01_Check_Radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damaged(faceDirection);
            }
        }
    }

    private void Attack_02_Trigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attack_02_Check.position, attack_02_Check_Radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damaged(faceDirection);
            }
        }
    }

    private void Attack_03_Trigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attack_03_Check.position, attack_03_Check_Radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damaged(faceDirection);
            }
        }
    }

    private void AnimationTrigger()
    {
        existTimer = -0.1f;
        ChangeState("Idle");
    }

    public bool isDetectedEnemy() => Physics2D.OverlapCircle(attack_03_Check.position, attack_03_Check_Radius,layerMask_Enemy);
}
