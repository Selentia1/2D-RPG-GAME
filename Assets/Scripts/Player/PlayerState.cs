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
    }
    public virtual void Exit() 
    {
        player.animator.SetBool(animParameterName, false);
    }
    public virtual void Update() 
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.animator.SetFloat("YVlocity", rb.velocity.y);
        
        
        if (Input.GetKeyDown(KeyCode.Space) && player.jumpTimes < 2)
        {
            stateMachine.ChangeState(player.riseState);
            player.jumpTimes++;
        }

        if (player.dashCDTimer > 0)
        {
            player.dashCDTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Z) && player.dashCDTimer <= 0) {
            stateMachine.ChangeState(player.dashState);
        }
    }


}
