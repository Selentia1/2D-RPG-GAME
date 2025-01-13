using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class BlueAttackRay : MonoBehaviour
{
    private BoxCollider2D boxCd;
    public Animator animator;
    [SerializeField] private float attackCDTimer;
    public float attackCD;
    public int attackAmounts = 0;

    // Start is called before the first frame update
    void Start()
    {
        boxCd = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Attack", true);
    }

    private void Update()
    {
        if (attackCDTimer > 0) {
            attackCDTimer -= Time.deltaTime;
        }
        
        if (attackCDTimer <= 0)
        {
            attackCDTimer = attackCD;

            Vector3 dirction = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));
            float distance = boxCd.size.magnitude * transform.localScale.x;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dirction, distance);
            foreach (var hit in hits)
            {
                if (hit.collider.GetComponent<Enemy>() != null)
                {
                    Direction.Dir dir = hit.collider.transform.position.x - transform.position.x > 0 ? Direction.Dir.Right : Direction.Dir.Left;

                    EnemyStats enemyStats = hit.collider.GetComponent<EnemyStats>();
                    PlayerStats playerStats = PlayerManager.instance.stats;
                    playerStats.DoDamage(enemyStats,"damaged", dir, false);

                    hit.collider.GetComponent<Enemy>().StartCoroutine("FreezedTime", 0.1f);
                    attackAmounts++;
                    
                }
            }
        }
    }

}
