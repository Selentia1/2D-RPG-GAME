using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Item : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected CircleCollider2D circleCollider;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public virtual void SetUpItem() { 
        //设置物品初始属性
    }

    public virtual void SetUpItem(Vector2 direction, float gravityScale)
    {
        //设置物品初始属性
    }
}
