using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    [Header("Sword Info")]
    private Entity luancher;
    private SpriteRenderer sr;
    [SerializeField] private float fadeSpeed;
    public bool canRotate = true;



    #region Sword Bounce Info
    [SerializeField] private bool isBouncing;
    private float canBounceTimes;
    public List<Transform> enemyTarget;
    private float bounceSpeed;
    private float bounceRadius;
    private int targetIndex = 0;
    #endregion

    #region Sword Pierce Info
    [SerializeField] private bool isPiercing;
    #endregion

    #region Sword Spin Info
    [SerializeField]private bool isSpining;
    private Vector2 luanchPoint;
    private float SwordMoveMaxDistance;
    #endregion

    public override void SetUpItem(Vector2 luachSpeed, float gravityScale)
    {
        rb.velocity = luachSpeed;
        rb.gravityScale = gravityScale;
        anim.SetBool("Rotation", true);
    }

    public void SetUpBounceSword(bool isBouncing, int canBounceTimes, float bounceSpeed, float bounceRadius) {
        this.isBouncing = isBouncing;
        this.canBounceTimes = canBounceTimes;
        this.bounceSpeed = bounceSpeed;
        this.bounceRadius = bounceRadius;
    }

    public void SetUpPierceSword(bool isPiercing,float pierceSwordExsitTime)
    {
        this.isPiercing = isPiercing;
        this.exsitDuration = pierceSwordExsitTime;
        existTimer = exsitDuration;
    }
    public void SetUpSpainSword(bool isSpining, float spinSwordExsitTime, float SwordMoveMaxDistance, Vector2 luanchPoint)
    {
        this.isSpining = isSpining;
        this.exsitDuration = spinSwordExsitTime;
        this.SwordMoveMaxDistance = SwordMoveMaxDistance;
        this.luanchPoint = luanchPoint;
        existTimer = exsitDuration;
    }
    protected override void Awake()
    {
        base.Awake();
        sr = GetComponentInChildren<SpriteRenderer>();

    }

    private void OnDrawGizmos()
    {
        if (isBouncing) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, bounceRadius);
        }
    }
    protected void Start()
    {
        existTimer = exsitDuration;
    }

    protected override void Update()
    {
        SwordLogic();
        BounceSwordLogic();
        PierceSwordLogic(); 
        SapinSwordLogic();
    }

    private void SwordLogic()
    {
        existTimer -= Time.deltaTime;
        //�����������ʱ�佣����ʧ
        if (existTimer > 0)
        {
            //�����������ת״̬�򽣻ᱻ����
            if (canRotate)
            {
                transform.right = rb.velocity;
            }
            else
            {
                sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * fadeSpeed);
                if (sr.color.a <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * fadeSpeed);
            if (sr.color.a <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void BounceSwordLogic()
    {
        //���ʹ�õ��ǵ�������������Χ��Ŀ�����һ���ҵ������������㣬����ظ�ִ�е�����������ֱ�������������Ĵ�����Ȼ�����ٽ�
        if (isBouncing && enemyTarget.Count > 1 && canBounceTimes > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                canBounceTimes--;
                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
        else if (isBouncing && canBounceTimes <= 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * fadeSpeed * 5);
            if (sr.color.a <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void PierceSwordLogic()
    {
        if (isPiercing)
        {

        }
    }

    private void SapinSwordLogic() { 
        if (isSpining)
        {
            if (Vector2.Distance(luanchPoint, transform.position) > SwordMoveMaxDistance) {
                rb.velocity = new Vector2(0, 0);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null && isBouncing)
        {
            //��ӵ�����Ŀ��
            if (enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bounceRadius);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null && !enemyTarget.Contains(hit.transform))
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }

        //���Կ�����Ŀ������˺�����˺�
        if (transform.position.x - collision.transform.position.x > 0)
        {
            collision.GetComponent<Enemy>()?.UnderAttack("damaged",Direction.Dir.Left, true);
        }
        else
        {
            collision.GetComponent<Enemy>()?.UnderAttack("damaged",Direction.Dir.Right, true);
        }

        //��ͣ������ת��������ס��
        StuckSowrd(collision);
    }

    private void StuckSowrd(Collider2D collision)
    {
        if (isSpining && collision.GetComponent<Enemy>() == null || !isSpining && !isPiercing || isPiercing && collision.GetComponent<Enemy>() == null)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            if (!isBouncing || isBouncing && enemyTarget.Count <= 1)
            {
                circleCollider.enabled = false;
                canRotate = false;
                anim.SetBool("Rotation", false);
                transform.parent = collision.transform;
            }
        }
        else if (isSpining && collision.GetComponent<Enemy>() != null) {
            rb.velocity = new Vector2(0, 0);
        }
    }
}
