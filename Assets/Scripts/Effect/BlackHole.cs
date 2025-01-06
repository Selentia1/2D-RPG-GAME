using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float maxSize;
    public float growSpeed;
    public bool canGrow;
    public float exsitTimer;
    public float exsitDuration;


    public List<Transform> targets;

    // Start is called before the first frame update
    void Start()
    {
        exsitTimer = exsitDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (exsitTimer > 0) {
            exsitTimer -= Time.deltaTime;
            canGrow = true;
        }
        else 
        {
            canGrow = false;            
        }


        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        else {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0, 0), growSpeed * Time.deltaTime *5);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null) {
            collision.GetComponent<Enemy>()?.FreezedTime(exsitTimer);
            targets.Add(collision.transform);
        }

    }
}
