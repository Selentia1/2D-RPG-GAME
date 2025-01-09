using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleAnimationTrigger : MonoBehaviour
{
    private BlackHoleAnimation blackHoleAnimation;
    // Start is called before the first frame update
    void Start()
    {
        blackHoleAnimation = GetComponent<BlackHoleAnimation>();
    }

    public void AnimationTrigger() {
        blackHoleAnimation.AnimationTrigger();
    }
    // Update is called once per frame
}
