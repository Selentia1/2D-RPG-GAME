using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Skill : Skill
{
    //³å´ÌËÙ¶È
    public float dashSpeed;
    public override bool CkeckAndUseSkill()
    {
        return base.CkeckAndUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        skillTimer = skillDuration;
    }

    public override bool CkeckSkill()
    {
        return base.CkeckSkill();
    }
}
