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
    [SerializeField] protected float GroundCheckDistance;
    //�ж�����Ƿ��ڵ���
    [SerializeField] protected bool isGround;

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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ColliderController();
        FlipController();
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
    }

    //��ײ���
    protected virtual void ColliderController()
    {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, layerMask_Ground);
    }

    //����������
    protected virtual void AnimatorControllers() { }

    //��ת������
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
