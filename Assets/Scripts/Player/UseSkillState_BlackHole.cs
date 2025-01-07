using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UseSkillState_BlackHole : UseSkillState
{
    float tempPositionY;
    bool isGroundBeforeUsingSkill;
    bool isUsedSkill;
    public UseSkillState_BlackHole(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        tempPositionY = player.transform.position.y;
        player.blackHoleFlyTimer = player.blackHoleFlyDuration;
        if (player.IsGroundDetected()) { 
            isGroundBeforeUsingSkill = true;
        }
        rb.gravityScale = 0;
        isUsedSkill = false;
    }

    public override void Exit()
    {
        base.Exit();
        isGroundBeforeUsingSkill = false;
        rb.gravityScale = player.defaultGravityScale;
    }

    public override void Update()
    {
        player.blackHoleFlyTimer -= Time.deltaTime;
        if (!isGroundBeforeUsingSkill || player.blackHoleFlyTimer < 0)
        {
            player.SetVelocity(0, player.blackHoleDropSpeed);
            if (!isUsedSkill) {
                SkillManager.instance.blackHole.UseSkill();
                isUsedSkill = true;
            }
        } else if (player.blackHoleFlyTimer > 0) {
            player.SetVelocity(0, player.blackHoleFlySpeed);
        }

        if (SkillManager.instance.blackHole.blackHoleScript != null && SkillManager.instance.blackHole.blackHoleScript.exsitTimer <= 0)
        {
            stateMachine.ChangeState(player.idleState);
            Debug.Log(SkillManager.instance.blackHole.blackHoleScript.exsitTimer);
        } else if ( Input.GetKeyUp(KeyCode.G)) {
            if (SkillManager.instance.blackHole.blackHoleScript != null) {
                SkillManager.instance.blackHole.blackHoleScript.exsitTimer = 0;
            }
            stateMachine.ChangeState(player.idleState);
        }
    }
}
