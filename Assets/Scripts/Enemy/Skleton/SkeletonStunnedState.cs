using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : SkeletonState
{
    public SkeletonStunnedState(Enemy enemy, EnemyStateMachine stateMachine, string animParameterName) : base(enemy, stateMachine, animParameterName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        skeleton.canBeStunned = false;
        skeleton.fx.InvokeRepeating("StunnedBlinkFX", 0, skeleton.fx.stunnedFlashCD);
        skeleton.stunnedTimer = skeleton.stunnedDuration;
    }

    public override void Exit()
    {
        base.Exit();
        skeleton.fx.CancelStunnedBlinkFX();
    }

    public override void Update()
    {
        base.Update();
        skeleton.stunnedTimer -= Time.deltaTime;
        if (skeleton.stunnedTimer <= 0) {
            stateMachine.ChangeState(skeleton.idleState);
        }
    }
}
