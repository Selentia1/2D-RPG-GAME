using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Physical Component")]
    //获取组件
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Collider2D collider2D;
    [SerializeField] protected Animator animator;

    [Header("Move And Jump")]
    //角色的移动速度和跳跃力
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpForce;
    //是否在移动
    [SerializeField] protected bool isMoving;


    [Header("Collider Check")]
    //图层遮罩用于检测是否接触地面，将需要检测的物体设置到Ground图层
    [SerializeField] protected LayerMask layerMask_Ground;
    //检测射线最大距离
    [SerializeField] protected float GroundCheckDistance;
    //判断玩家是否在地面
    [SerializeField] protected bool isGround;

    [Header("direction")]
    //实体朝向
    [SerializeField] protected FaceDirection direction;
    [SerializeField] protected enum FaceDirection
    {
        RIGHT = -1,
        LEFT = 1
    }
    

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ColliderController();
        FlipController();
    }

    //移动
    protected virtual void Movement() { }

    //跳跃
    protected virtual void Jump() { }

    //攻击
    protected virtual void Attack() { }

    //翻转实体
    protected virtual void Filp()
    {
        direction = (direction == FaceDirection.LEFT ? FaceDirection.RIGHT : FaceDirection.LEFT);
        transform.Rotate(0, 180, 0);
    }

    //碰撞检测
    protected virtual void ColliderController()
    {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, layerMask_Ground);
    }

    //动画控制器
    protected virtual void AnimatorControllers() { }

    //翻转控制器
    protected virtual void FlipController()
    {
        if (rb.velocity.x > 0 && direction != FaceDirection.RIGHT)
        {
            Filp();
        }
        else if (rb.velocity.x < 0 && direction != FaceDirection.LEFT)
        {
            Filp();
        }
    }

}
