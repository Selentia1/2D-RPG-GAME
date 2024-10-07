using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionEvents : MonoBehaviour
{
    private OldPlayer player;
    void Start()
    {
        player = GetComponentInParent<OldPlayer>();
    }

    private void AnimationTigger() { 
        player.AttackOver();
    }
}
