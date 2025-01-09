using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectStateMachine 
{
    public EffectState currentState { get; private set; }

    public void Initialize(EffectState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EffectState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public EffectState GetCurrentState()
    {
        return currentState;
    }
}
