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
            //进入到上升状态时获得一个y方向的速度=玩家的跳跃力，注意这个速度只能放在这，放在状态内如果从其他在状态切换到上身
            player.SetVelocity(rb.velocity.x, player.jumpForce);
            player.jumpTimes++;
        }

        //任意状态都可以冲刺
        if (Input.GetKeyDown(KeyCode.Z) && SkillManager.instance.dash.CkeckAndUseSkill())
        {
            stateMachine.ChangeState(player.dashState);
        }

        //任意状态都可以防御
        if (Input.GetKeyDown(KeyCode.C) && player.defenceCDTimer <= 0)
        {
            stateMachine.ChangeState(player.defenceState);
        }

        //任意状态都可以使用技能 残影攻击(克隆)
        if (Input.GetKeyDown(KeyCode.F) && SkillManager.instance.clone.CkeckAndUseSkill()) {
            stateMachine.ChangeState(player.useSkill_Clone);
        }

        //任意状态都可以使用投掷飞剑
        if (Input.GetKeyDown(KeyCode.V) && SkillManager.instance.throwSword.CkeckSkill())
        {
            stateMachine.ChangeState(player.catchSwordState);
        }

        //任意状态都可以使用黑洞技能
        if (Input.GetKeyDown(KeyCode.G) && SkillManager.instance.blackHole.CkeckSkill()) {
            stateMachine.ChangeState(player.useSkillState_BlackHole);
        }

        if (player.combooResetTimer > 0)
        {
            player.combooResetTimer -= Time.deltaTime;
        }

        if (player.defenceCDTimer > 0) {
            player.defenceCDTimer -= Time.deltaTime;
        }
        
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

}
