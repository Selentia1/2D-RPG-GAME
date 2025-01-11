using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAnimationTrigger : MonoBehaviour
{
    private Crystal crystal;
    private CircleCollider2D circleCollider;
    void Start()
    {
        crystal = GetComponentInParent<Crystal>();
        circleCollider = GetComponentInParent<CircleCollider2D>();
    }

    public void DestroyCrystalTrigger()
    {
        crystal.CrystalDestroy();
    }

    public void ExplodeTrigger() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,circleCollider.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Direction.Dir dir = hit.transform.position.x - transform.position.x > 0 ? Direction.Dir.Right : Direction.Dir.Left;
                hit.GetComponent<Enemy>().UnderAttack("damaged", dir, true);
            }
        }
    }   
}
