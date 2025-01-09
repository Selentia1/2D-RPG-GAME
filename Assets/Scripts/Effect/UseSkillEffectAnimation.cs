using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseSkillEffectAnimation : EffectAnimation
{
    public UseSkillEffectState useSkillEffectState;
    public Image useSkillMask;
    public float useSkillEffectDuration;
    public float effectfadeSpeed;
    public RectTransform maskTransform;
    public float MaskfadeDurationCD;

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    protected override void Awake()
    {
        base.Awake();
        useSkillMask = GetComponentInChildren<Image>();
        maskTransform = useSkillMask.GetComponent<RectTransform>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Start()
    {
        base.Start();
        useSkillEffectState = new UseSkillEffectState(this, stateMachine, "isUsedSkill");
    }
    public override IEnumerator StartEffect()
    {
        stateMachine.ChangeState(useSkillEffectState);
        yield return null;
    }
}
