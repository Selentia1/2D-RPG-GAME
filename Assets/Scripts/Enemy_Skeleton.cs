using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eneny_Skeleton : Entity
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 2;
    }

    protected override void Update()
    {
        base.Update();
        FlipController();
        Movement();
    }

    protected override void Movement()
    {
        if (direction == FaceDirection.LEFT)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else if (direction == FaceDirection.RIGHT)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }
    protected override void FlipController()
    {
        if (!isGround || isDetectedWall)
        {
            Filp();
        }
    }


}
