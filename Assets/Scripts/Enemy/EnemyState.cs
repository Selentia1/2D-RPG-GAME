using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected Rigidbody2D rb;

    public bool triggerCalled;
    protected string animParameterName;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animParameterName)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.animParameterName = animParameterName;
    }

    public virtual void Enter()
    {
        rb = enemy.rb;
        enemy.animator.SetBool(animParameterName, true);
    }
    public virtual void Exit()
    {
        enemy.animator.SetBool(animParameterName, false);
    }
    public virtual void Update()
    {

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}