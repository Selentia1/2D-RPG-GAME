using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPoint : MonoBehaviour
{
    public EntityFX fx;

    private void Start()
    {
        fx = GetComponent<EntityFX>();
    }

    public void AimTargetEffect(bool start) {
        if (start)
        {
            if (fx != null)
            fx.StartCoroutine("FlashFX");
        }
        else { 
        }
    }
}
