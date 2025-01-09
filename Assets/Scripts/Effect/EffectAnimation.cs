using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimation : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public EffectStateMachine stateMachine;
    public EmptyState emptyState { get; private set; }
    public bool isUsingEffect;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateMachine = new EffectStateMachine();
        emptyState = new EmptyState(this,stateMachine,"isEmpty");
    }
    protected virtual void Start()
    {
        stateMachine.Initialize(emptyState);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        stateMachine.currentState.Update();
    }

    public virtual IEnumerator StartEffect() {
        isUsingEffect = true;
        yield return null;
    }

    public virtual void ExitEffect()
    {

    }
    public virtual void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
