using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    //���״̬���������Ҫ�����
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

        //����״̬��������Ծ 
        if (Input.GetKeyDown(KeyCode.Space) && player.jumpTimes < 2)
        {
            stateMachine.ChangeState(player.riseState);
            //���뵽����״̬ʱ���һ��y������ٶ�=��ҵ���Ծ����ע������ٶ�ֻ�ܷ����⣬����״̬�������������״̬�л�������
            player.SetVelocity(rb.velocity.x, player.jumpForce);
            player.jumpTimes++;
        }

        //����״̬�����Գ��
        if (Input.GetKeyDown(KeyCode.Z) && SkillManager.instance.dash.CkeckAndUseSkill())
        {
            stateMachine.ChangeState(player.dashState);
        }

        //����״̬�����Է���
        if (Input.GetKeyDown(KeyCode.C) && player.defenceCDTimer <= 0)
        {
            stateMachine.ChangeState(player.defenceState);
        }

        //����״̬������ʹ�ü��� ��Ӱ����(��¡)
        if (Input.GetKeyDown(KeyCode.F) && SkillManager.instance.clone.CkeckAndUseSkill()) {
            stateMachine.ChangeState(player.useSkill_Clone);
        }

        //����״̬������ʹ��Ͷ���ɽ�
        if (Input.GetKeyDown(KeyCode.V) && SkillManager.instance.throwSword.CkeckSkill())
        {
            stateMachine.ChangeState(player.catchSwordState);
        }

        //����״̬������ʹ�úڶ�����
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
