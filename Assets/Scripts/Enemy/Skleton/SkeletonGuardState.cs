using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGuardState : SkeletonState
{
    //玩家追踪模式，如果看见玩家则会开启
    protected bool playerTracking;
    public SkeletonGuardState(Enemy enemy, EnemyStateMachine stateMachine, string animParameterName) : base(enemy, stateMachine, animParameterName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        skeleton.GuardTimer = skeleton.GuardTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (skeleton.IsPLayerDetected()) {
            playerTracking = true;
            skeleton.hitInfo = skeleton.GetPLayerDetected();
            skeleton.GuardTimer = skeleton.GuardTime;
        }else if (skeleton.GuardTimer > 0) { 
            skeleton.GuardTimer -= Time.deltaTime;
        }
    }
}
