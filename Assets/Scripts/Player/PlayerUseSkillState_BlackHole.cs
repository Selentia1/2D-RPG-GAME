using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerUseSkillState_BlackHole : PlayerUseSkillState
{
    bool isUsedSkill;
    public PlayerUseSkillState_BlackHole(Player player, PlayerStateMachine stateMachine, string animParameterName) : base(player, stateMachine, animParameterName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.blackHoleFlyTimer = player.blackHoleFlyDuration;
        rb.gravityScale = 0;
        isUsedSkill = false;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = player.defaultGravityScale;
    }

    public override void Update()
    {
        player.blackHoleFlyTimer -= Time.deltaTime;
        if (player.blackHoleFlyTimer < 0)
        {
            player.SetVelocity(0, player.blackHoleDropSpeed);
            if (!isUsedSkill)
            {
                SkillManager.instance.blackHole.UseSkill();
                isUsedSkill = true;
            }
        }
        else if (player.blackHoleFlyTimer > 0)
        {
            player.SetVelocity(0, player.blackHoleFlySpeed);
        }

        if (SkillManager.instance.blackHole.blackHoleScript != null && SkillManager.instance.blackHole.blackHoleScript.exsitTimer <= 0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            if (SkillManager.instance.blackHole.blackHoleScript != null)
            {
                SkillManager.instance.blackHole.blackHoleScript.exsitTimer = 0;
            }

            if (SkillManager.instance.blackHole.blackHoleScript.attackEnd) {
                stateMachine.ChangeState(player.fallState);
            }

        } else if (SkillManager.instance.blackHole.blackHoleScript != null && SkillManager.instance.blackHole.blackHoleScript.isDamged) {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
