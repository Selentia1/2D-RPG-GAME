using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    #region BlackHole Info
    [Header("BlackHole Info")]
    private float maxSize;
    private float growSpeed;
    private float backSpeed;
    public bool canGrow;
    public float exsitTimer;
    private float exsitDuration;
    [SerializeField] private List<Transform> targets;
    [SerializeField] private List<GameObject> targets_HotKey;
    private CircleCollider2D circleCollider;

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<HotKeyCode> QTE_List;
    #endregion

    [Header("BlackHole Clone Info")]
    private int amountOfAttacks;
    private float cloneAttackCooldown;
    [SerializeField] private GameObject playerClonePrefab;
    // Start is called before the first frame update
    void Start()
    {
        exsitTimer = exsitDuration;
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimers();
        UpdateScale();
        HotKeyCheck();

    }

    private void HotKeyCheck()
    {
        //按下按钮就销毁按钮，并生成克隆体攻击目标
        for (int i =0;i< targets_HotKey.Count;i++) {
            if (targets_HotKey[i] != null) {
                HotKey hotKeyScrpit = targets_HotKey[i].GetComponent<HotKey>();
                if (Input.GetKeyDown(hotKeyScrpit.SwitchHotKeyCodeToKeyCode()))
                {
                    if (targets_HotKey[i].active == true)
                    {
                        StartCoroutine("CloneAttack", targets[i].transform);
                    }
                    targets_HotKey[i].active = false;
                }
            }
        }
    }

    private IEnumerator CloneAttack(Transform target)
    {
        Transform cloneTransform = target;
        cloneTransform.position = new Vector2(target.position.x,target.position.y);


        for (int i = 0;i< amountOfAttacks;i++) {
            if (i % 2 == 0)
                SkillManager.instance.clone.CreateClone(cloneTransform, "Attack", Direction.Dir.Left,new Vector3(1.1f,0));
            else
                SkillManager.instance.clone.CreateClone(cloneTransform, "Attack", Direction.Dir.Right, new Vector3(-1.1f,0));
            yield return new WaitForSeconds(cloneAttackCooldown);
        }
    }

    private void UpdateScale()  
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0, 0), backSpeed * Time.deltaTime);

            for (int i = 0; i < targets_HotKey.Count; i++)
            {
                Destroy(targets_HotKey[i]);
            }

            if (transform.localScale.x < 0.01f)
            {
                Destroy(gameObject); // 销毁黑洞
            }
        }
    }

    private void UpdateTimers()
    {
        if (exsitTimer > 0)
        {
            exsitTimer -= Time.deltaTime;
            canGrow = true;
        }
        else
        {
            canGrow = false;
        }
    }

    private void UpdateRadius() {
        circleCollider.radius = 0.5f * maxSize/transform.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null && !targets.Contains(collision.transform)) {
                collision.GetComponent<Enemy>()._FreezeTime(true);
                targets.Add(collision.transform);
                GameObject hotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 1.5f), Quaternion.identity);
                hotKey.transform.parent = collision.transform;
            
                HotKeyCode RandomKey = QTE_List[Random.Range(0, QTE_List.Count)];
                targets_HotKey.Add(hotKey);
                hotKey.GetComponent<HotKey>().SetHotKey(RandomKey);
                QTE_List.Remove(RandomKey);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Enemy>() != null && targets.Contains(other.transform))
        {
            other.GetComponent<Enemy>()._FreezeTime(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null && targets.Contains(collision.transform)) {
            collision.GetComponent<Enemy>()._FreezeTime(false);
        }
    }

    public void Init(float maxSize, float exsitDuration, float growSpeed, float backSpeed,int amountOfAttacks,float cloneAttackCooldown) {
        this.maxSize = maxSize;
        this.exsitDuration = exsitDuration;
        this.exsitTimer = exsitDuration;
        this.growSpeed = growSpeed;
        this.backSpeed = backSpeed;
        this.amountOfAttacks = amountOfAttacks;
        this.cloneAttackCooldown = cloneAttackCooldown;
        
    }
}
