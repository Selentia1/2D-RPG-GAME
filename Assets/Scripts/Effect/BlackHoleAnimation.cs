using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleAnimation : EffectAnimation
{
    // Start is called before the first frame update
    public BlackHoleExplosionEffectState blackHoleExplosionEffectState { get; private set; }
    public BlackHole blackHole { get; private set; }

    public override IEnumerator StartEffect()
    {
        stateMachine.ChangeState(blackHoleExplosionEffectState);
        yield return null;
    }

    protected override void Awake()
    {
        base.Awake();
        blackHoleExplosionEffectState = new BlackHoleExplosionEffectState(this,stateMachine,"isExplode");
    }

    protected override void Start()
    {
        base.Start();
        blackHole = GetComponentInParent<BlackHole>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void isDamged() {
        blackHole.isDamged = true;
    }
}
