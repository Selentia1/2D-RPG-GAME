using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : Decritions
{

    protected override void Start()
    {
        base.Update();
        animator.SetBool("Light",false);
    }
    protected override void Update()
    {
        animator.SetBool("Light", true);
    }
}
