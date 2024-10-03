using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Physical Component")]
    //��ȡ���
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Collider2D collider2D;
    [SerializeField] protected Animator animator;

    [Header("Move And Jump")]
    //��ɫ���ƶ��ٶȺ���Ծ��
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpForce;
    //�Ƿ����ƶ�
    [SerializeField] protected bool isMoving;

    [Header("Collider Check")]
    //ͼ���������ڼ���Ƿ�Ӵ����棬����Ҫ�����������õ�Groundͼ��
    [SerializeField] protected LayerMask layerMask_Ground;
    //�������������
    [SerializeField] protected float groundCheckDistance;
    //�ж�����Ƿ��ڵ���
    [SerializeField] protected bool isGround;
    [SerializeField] public Transform groundCheck;

    //ǽ����ײ���߼��
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected bool isDetectedWall;
    [SerializeField] public Transform wallCheck;



    [Header("direction")]
    //ʵ�峯��
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

    //�ƶ�
    protected virtual void Movement() { }

    //��Ծ
    protected virtual void Jump() { }

    //����
    protected virtual void Attack() { }

    //��תʵ��
    protected virtual void Filp()
    {
        direction = (direction == FaceDirection.LEFT ? FaceDirection.RIGHT : FaceDirection.LEFT);
        transform.Rotate(0, 180, 0);
        wallCheckDistance = -wallCheckDistance;
    }

    //��ײ���
    protected virtual void ColliderController()
    {
        isGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, layerMask_Ground);
        isDetectedWall = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, layerMask_Ground);
    }

    //����������
    protected virtual void AnimatorControllers() { }

    //��ת������
    protected virtual void FlipController(){ }

        //�����������
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y ));
    }

}
