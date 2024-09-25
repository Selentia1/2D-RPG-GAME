using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Accessibility;

public class Player : MonoBehaviour
{
    //��ɫ���ƶ��ٶȺ���Ծ��
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    //��ȡ���
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;

    //�Ƿ����ƶ�
    [SerializeField] private bool isMoving;

    //���������ٶ�
    [SerializeField] private float velocityX;
    [SerializeField] private float velocityY;

    //ͼ���������ڼ���Ƿ�Ӵ����棬����Ҫ�����������õ�Groundͼ��
    [SerializeField] private LayerMask layerMask_Ground;
    //�������������
    [SerializeField] private float MaxDistanceToGround;
    //�ж�����Ƿ��ڵ���
    private bool isGround;

    //��ɫ����
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

        //���¿ո�������Ծ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    //����ˮƽ�ƶ�
    private void Movement()
    {
        rb.velocity = new Vector2(velocityX, rb.velocity.y);
    }

    //����������
    private void AnimatorControllers() {
        //�����ƶ�״̬����
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

    //��Ծ
    private void Jump() {
        //�ڵ����ϲ�������
        if (isGround) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }
        
    }
    
    //��ת����
    private void Filp(){
        direction = (direction == FaceDirection.LEFT ? FaceDirection.RIGHT: FaceDirection.LEFT);
        transform.Rotate(0, 180, 0);
    }

    //��ת������
    private void FlipController() {
        if (rb.velocity.x > 0 && direction != FaceDirection.RIGHT) {
            Filp();
        } else if (rb.velocity.x < 0 && direction != FaceDirection.LEFT) {
            Filp();
        }
    }

    //��������ܷ���Ծ
    private void JumpController() {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, MaxDistanceToGround, layerMask_Ground);
    }

}
