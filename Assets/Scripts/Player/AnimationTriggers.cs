using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour 
{
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
    private void AnimationTrigger() {
        player.AnimationTrigger();
    }
}
