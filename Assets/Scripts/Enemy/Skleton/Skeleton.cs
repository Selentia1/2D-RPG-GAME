using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Skeleton : Enemy
{
    #region Skeleton State
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonReactState reactState { get; private set; }
    public SkeletonTraceState traceState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonHurtState hurtState { get; private set;}
    public SkeletonStunnedState stunnedState { get; private set;}
    #endregion

    [Header("Patrol Info")]
    #region Patrol Info
    //巡逻状态的骷髅，巡逻包含两个状态 闲置和移动
    //两个状态的切换的计时器，大于2秒时为闲置状态，小于2秒时为移动状态
    public float patrolTime;
    public float patrolTimer;
    #endregion

    [Header("Guard Info")]
    #region Guard Info
    //警戒状态下骷髅追踪玩家的速度,触发警戒后的持续时间
    public float traceSpeed;
    public float defaultTraceSpeed;
    public float GuardTime;
    public float GuardTimer;
    public float GuardDistance;
    #endregion

    [Header("Attack Info")]
    #region Attack Info
    //骷髅进入攻击状态下的条件为与玩家间隔相差小于Xm
    public float attackDistance;
    #endregion
    public override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle");
        moveState = new SkeletonMoveState(this, stateMachine, "Move");
        reactState = new SkeletonReactState(this, stateMachine, "React");
        traceState = new SkeletonTraceState(this, stateMachine, "Trace");
        attackState = new SkeletonAttackState(this, stateMachine, "Attack");
        hurtState = new SkeletonHurtState(this, stateMachine, "Hurt");
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned");
    }

    public override void Start() {
        base.Start();
        stateMachine.Initialize(idleState);
        defaultTraceSpeed = traceSpeed;
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FlipController()
    {
        base.FlipController();

        if ((!IsGroundDetected() || IsWallDetected()) && (rb.velocity.y < 0.001 && rb.velocity.y > -0.001))
        {
            Flip();
        }
    }

    public override void Damaged(Direction.Dir attackDirection)
    {
        if (stunnedTimer > 0)
        {
            base.Damaged(attackDirection);
        }
        else {
            stateMachine.ChangeState(hurtState);
            StartCoroutine(HitKnockback(attackDirection));
        }
    }

    public async override void Damaged(Direction.Dir attackDirection, float time)
    {
        await Task.Delay((int)(time * 1000));
        if (stunnedTimer > 0)
        {
            base.Damaged(attackDirection);
        }
        else
        {
            stateMachine.ChangeState(hurtState);
            StartCoroutine(HitKnockback(attackDirection));
        }

    }

    public override bool CheckAndTurnStunned()
    {
        if (base.CheckAndTurnStunned()) { 
            return true;
        }
        return false;
    }

    public override void TurnStunned()
    {
        if(canBeStunned)
        stateMachine.ChangeState(stunnedState);
    }

    public override IEnumerator FreezedTime(float freezedSeconds)
    {
        return base.FreezedTime(freezedSeconds);
    }

    public override void _FreezeTime(bool isFreezing)
    {
        if (isFreezing)
        {
            animator.speed = 0;
            moveSpeed = 0;
            traceSpeed = 0;
        }
        else
        {
            animator.speed = 1;
            moveSpeed = deafultMoveSpeed;
            traceSpeed = defaultTraceSpeed;
        }
    }
}
