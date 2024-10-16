using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    //玩家状态所需包含必要的组件
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected Rigidbody2D rb;

    protected string animParameterName;
    protected float xInput;
    protected float yInput;
    public bool triggerCalled;
    public PlayerState(Player player, PlayerStateMachine stateMachine,string animParameterName) 
    { 
        this.player = player;
        this.stateMachine = stateMachine;
        this.animParameterName = animParameterName;
    }

    public virtual void Enter() 
    {
        rb = player.rb;
        player.animator.SetBool(animParameterName, true);
        player.animator.SetFloat("YVlocity", rb.velocity.y);
        player.animator.SetInteger("AttackComboo", player.attackComboo);
    }
    public virtual void Exit() 
    {
        player.animator.SetBool(animParameterName, false);
    }
    public virtual void Update() 
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        //任意状态都可以跳跃 
        if (Input.GetKeyDown(KeyCode.Space) && player.jumpTimes < 2)
        {
            stateMachine.ChangeState(player.riseState);
            player.jumpTimes++;
        }

        //任意状态都可以冲刺
        if (Input.GetKeyDown(KeyCode.Z) && player.dashCDTimer <= 0)
        {
            stateMachine.ChangeState(player.dashState);
        }

        if (player.dashCDTimer > 0)
        {
            player.dashCDTimer -= Time.deltaTime;
        }

        if (player.combooResetTimer > 0)
        {
            player.combooResetTimer -= Time.deltaTime;
        }
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

}
