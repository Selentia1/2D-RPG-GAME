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
    }
}
