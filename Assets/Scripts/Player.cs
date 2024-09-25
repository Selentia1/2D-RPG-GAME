using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Accessibility;

public class Player : MonoBehaviour
{
    //角色的移动速度和跳跃力
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    //获取组件
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;

    //是否在移动
    [SerializeField] private bool isMoving;

    //各个方向速度
    [SerializeField] private float velocityX;
    [SerializeField] private float velocityY;

    //图层遮罩用于检测是否接触地面，将需要检测的物体设置到Ground图层
    [SerializeField] private LayerMask layerMask_Ground;
    //检测射线最大距离
    [SerializeField] private float MaxDistanceToGround;
    //判断玩家是否在地面
    private bool isGround;

    //角色朝向
    private enum FaceDirection {
        RIGHT = -1,
        LEFT = 1
    }
    private FaceDirection direction;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        
        rb.velocity = Vector2.zero;
        moveSpeed = 5;
        jumpForce = 15;
        direction = FaceDirection.RIGHT;
        MaxDistanceToGround = 100;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        GetInPut();

        JumpController();
        AnimatorControllers();
        FlipController();

    }

    private void GetInPut()
    {
        velocityX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        //按下空格人物跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    //人物水平移动
    private void Movement()
    {
        rb.velocity = new Vector2(velocityX, rb.velocity.y);
    }

    //动画控制器
    private void AnimatorControllers() {
        //人物移动状态控制
        //isMoving = (rb.velocity.x != 0);
        if (rb.velocity.x != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        animator.SetBool("isMoving", isMoving);
    }

    //跳跃
    private void Jump() {
        //在地面上才能起跳
        if (isGround) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }
        
    }
    
    //翻转人物
    private void Filp(){
        direction = (direction == FaceDirection.LEFT ? FaceDirection.RIGHT: FaceDirection.LEFT);
        transform.Rotate(0, 180, 0);
    }

    //翻转控制器
    private void FlipController() {
        if (rb.velocity.x > 0 && direction != FaceDirection.RIGHT) {
            Filp();
        } else if (rb.velocity.x < 0 && direction != FaceDirection.LEFT) {
            Filp();
        }
    }

    //控制玩家能否跳跃
    private void JumpController() {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, MaxDistanceToGround, layerMask_Ground);
    }

}
