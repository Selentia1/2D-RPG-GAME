using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    //玩家状态所需包含必要的组件
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected Rigidbody2D rb;

    protected string animStateName;
    protected float xInput;

    public PlayerState(Player player, PlayerStateMachine stateMachine,string animStateName) 
    { 
        this.player = player;
        this.stateMachine = stateMachine;
        this.animStateName = animStateName;
    }

    public virtual void Enter() 
    {
        rb = player.rb;
        player.animator.SetBool(animStateName, true);
    }
    public virtual void Exit() 
    {
        player.animator.SetBool(animStateName, false);
    }
    public virtual void Update() 
    {
        GetInput();
    }

    private void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
    }
}
