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
    [SerializeField] protected float groundCheckDistance;
    //判断玩家是否在地面
    [SerializeField] protected bool isGround;
    [SerializeField] public Transform groundCheck;

    //墙面碰撞射线检测
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected bool isDetectedWall;
    [SerializeField] public Transform wallCheck;



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
        groundCheck = transform.Find("GroundCheck");
        wallCheck = transform.Find("WallCheck");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ColliderController();
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
        wallCheckDistance = -wallCheckDistance;
    }

    //碰撞检测
    protected virtual void ColliderController()
    {
        isGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, layerMask_Ground);
        isDetectedWall = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, layerMask_Ground);
    }

    //动画控制器
    protected virtual void AnimatorControllers() { }

    //翻转控制器
    protected virtual void FlipController(){ }

        //画出检测射线
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y ));
    }

}
