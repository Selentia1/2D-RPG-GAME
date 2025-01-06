using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonState : EnemyState
{
    protected Skeleton skeleton;
    public SkeletonState(Enemy enemy, EnemyStateMachine stateMachine, string animParameterName) : base(enemy, stateMachine, animParameterName)
    {
        skeleton = (Skeleton)enemy;
        this.stateMachine = stateMachine;
        this.animParameterName = animParameterName;
    }

    public override void Enter()
    {   
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (skeleton.patrolTimer > 0)
        {
            skeleton.patrolTimer -= Time.deltaTime;
        }
    }
}
