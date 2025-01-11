using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class BlackHole : MonoBehaviour
{
    #region BlackHole Info
    [Header("BlackHole Info")]
    [SerializeField] public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject hotKeyPrefab;
    private BlackHoleType type;
    private float maxSize;
    private float growSpeed;
    private float backSpeed;
    public bool canGrow;
    public float exsitTimer;
    private float exsitDuration;
    [SerializeField] private List<Transform> targets;
    [SerializeField] private List<GameObject> targets_HotKey;
    private CircleCollider2D circleCollider;

   
    [SerializeField] private List<HotKeyCode> QTE_List;
    private List<HotKeyCode> temp_List = new List<HotKeyCode>();

    #endregion

    [Header("BlackHole Sword Dance Info")]
    [SerializeField] private GameObject playerClonePrefab;
    [SerializeField] private List<Transform> tempTargets = new List<Transform>();
    public bool canAttack;
    public bool attackEnd;
    private int amountOfAttacks;
    private float cloneAttackCooldown;
    // Start is called before the first frame update

    [Header("BlackHole Explode Info")]
    public BlackHoleAnimation animationScrpit;
    private GameObject cameraPoint;
    [SerializeField] private float slowTimeScale;
    [SerializeField] private float TimeScaleRecoverSpeed;
    public bool explodeEnd;
    public bool readyToExplode;
    public bool isExplode;
    public bool isDamged;

    void Start()
    {
        exsitTimer = exsitDuration;
        circleCollider = GetComponent<CircleCollider2D>();
        animationScrpit = GetComponentInChildren<BlackHoleAnimation>();
        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimers();
        UpdateBlackHole();
        HotKeySetAndCheck();

    }

    private void HotKeySetAndCheck()
    {
        if (type == BlackHoleType.SwordDance)
        {
            //按下按钮就销毁按钮，并记录接下来攻击的目标
            for (int i = 0; i < targets_HotKey.Count; i++)
            {
                if (targets_HotKey[i] != null)
                {
                    HotKey hotKeyScrpit = targets_HotKey[i].GetComponent<HotKey>();
                    if (Input.GetKeyDown(hotKeyScrpit.SwitchHotKeyCodeToKeyCode()))
                    {
                        if (!targets.Contains(hotKeyScrpit.GetComponentInParent<Enemy>().transform)) {
                            targets.Add(hotKeyScrpit.GetComponentInParent<Enemy>().transform);
                        }
                        targets_HotKey[i].SetActive(false);
                    }
                }
            }
        }
        else if (type == BlackHoleType.Explode) {
            Transform player = PlayerManger.instance.player.transform;
            while(temp_List.Count > 0) {
                Direction.Dir dir = player.GetComponent<Player>().faceDirection;
                GameObject hotKey = Instantiate(hotKeyPrefab, player.position + new Vector3(-0.3f * (int)dir, 1.5f), Quaternion.identity);
                hotKey.transform.parent = player;
                targets_HotKey.Add(hotKey);

                HotKeyCode RandomKey = temp_List[Random.Range(0, temp_List.Count)];
                hotKey.GetComponent<HotKey>().SetHotKey(RandomKey);
                temp_List.Remove(RandomKey);
                hotKey.SetActive(false);
            }

            if (targets_HotKey.Count > 0)
            {
                if (targets_HotKey[0] != null) {
                    targets_HotKey[0].SetActive(true);
                    HotKey hotKeyScrpit = targets_HotKey[0].GetComponent<HotKey>();

                    if (Input.GetKeyDown(hotKeyScrpit.SwitchHotKeyCodeToKeyCode()))
                    {
                        targets_HotKey.RemoveAt(0);
                        Destroy(hotKeyScrpit.gameObject);
                    }
                }
            }
            else if (targets_HotKey.Count <= 0 && !isExplode)
            {
                readyToExplode = true;
            }
        }
    }

    private IEnumerator CloneAttack()
    {
        if (targets.Count > 0) {
            int count = 0;
            while (amountOfAttacks > 0)
            {
                count = (count + 1) % targets.Count;
                amountOfAttacks--;
                int random = Random.Range(0, 2);    
                GameObject clone;
                if (random == 0)
                {
                    clone = SkillManager.instance.clone.CreateClone(targets[count], "Attack", Direction.Dir.Left, new Vector3(1.1f, 0));
                }
                else
                {
                    clone = SkillManager.instance.clone.CreateClone(targets[count], "Attack", Direction.Dir.Right, new Vector3(-1.1f, 0));
                }

                if (virtualCamera != null && clone != null)
                {
                    PlayerManger.instance.player.gameObject.SetActive(false);
                    virtualCamera.Follow = clone.transform; // 改变跟随目标
                    virtualCamera.LookAt = clone.transform; // 改变观察目标（可选）
                }

                yield return new WaitForSeconds(cloneAttackCooldown);
            }
        }
        attackEnd  = true;
        if (virtualCamera != null)
        {
            PlayerManger.instance.player.gameObject.SetActive(true);
            virtualCamera.Follow = PlayerManger.instance.player.transform; // 改变跟随目标
            virtualCamera.LookAt = PlayerManger.instance.player.transform; // 改变观察目标（可选）
        }
    }

    private void UpdateBlackHole()  
    {
        if (type == BlackHoleType.SwordDance)
        {
            if (canGrow)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            }
            else
            {
                for (int i = 0; i < targets_HotKey.Count; i++)
                {
                    Destroy(targets_HotKey[i]);
                }

                if (canAttack) {
                    StartCoroutine("CloneAttack");
                    canAttack = false;
                }

                if (attackEnd) {
                    transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0, 0), backSpeed * Time.deltaTime);
                    if (transform.localScale.x < 0.01f)
                    {
                        Destroy(gameObject); // 销毁黑洞
                    }
                }

            }
        }
        else if (type == BlackHoleType.Explode) {
            if (canGrow)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
                if (readyToExplode)
                {
                    readyToExplode = false;
                    isExplode = true;
                    animationScrpit.StartCoroutine("StartEffect");
                }
                else if (explodeEnd)
                {
                    Destroy(gameObject); // 销毁黑洞
                }
            }
            else if (!canGrow)
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

            if (isDamged) {
                isDamged = false;
                Time.timeScale = slowTimeScale;
                
                if (targets.Count > 0) {
                    cameraPoint = new GameObject("cameraPoint");
                    cameraPoint.transform.position = transform.position * 0.3f;
                    foreach (Transform target in targets)
                    {
                        Direction.Dir dir;
                        dir = (target.position.x - transform.position.x > 0 ? Direction.Dir.Right : Direction.Dir.Left);
                        circleCollider.enabled = false;
                        target.GetComponent<Enemy>()._FreezeTime(false);
                        target.GetComponent<Enemy>().canBeStunned = true;
                        target.GetComponent<Enemy>().UnderAttack("stunned", dir, true);
                        cameraPoint.transform.position += target.position * 0.7f/(float)targets.Count;
                    }
                    virtualCamera.LookAt = cameraPoint.transform;
                    virtualCamera.Follow = cameraPoint.transform;
                }
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
        else if(exsitTimer<=0 && !isExplode)
        {
            canGrow = false;
        }

        if (type == BlackHoleType.Explode) {
            if (Time.timeScale <= 1)
            {
                Time.timeScale += Time.deltaTime * TimeScaleRecoverSpeed;
            }
            else if (Time.timeScale >= 1)
            {
                Time.timeScale = 1;
                virtualCamera.LookAt = PlayerManger.instance.player.transform;
                virtualCamera.Follow = PlayerManger.instance.player.transform;
                Destroy(cameraPoint);
            }
        }
    }

    private void UpdateRadius() {
        circleCollider.radius = 0.5f * maxSize/transform.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>()._FreezeTime(true);
            if (type == BlackHoleType.SwordDance)
            {
                if (!tempTargets.Contains(collision.transform)) {
                    GameObject hotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 1.5f), Quaternion.identity);
                    hotKey.transform.parent = collision.transform;

                    if (temp_List.Count > 0)
                    {
                        HotKeyCode RandomKey = temp_List[Random.Range(0, temp_List.Count)];
                        Debug.Log(RandomKey);
                        targets_HotKey.Add(hotKey);
                        hotKey.GetComponent<HotKey>().SetHotKey(RandomKey);
                        temp_List.Remove(RandomKey);
                        }
                    else if (temp_List.Count <= 0)
                    {
                        foreach (var keyCode in QTE_List)
                        {
                            temp_List.Add(keyCode);
                        }
                        HotKeyCode RandomKey = temp_List[Random.Range(0, temp_List.Count)];
                        targets_HotKey.Add(hotKey);
                        hotKey.GetComponent<HotKey>().SetHotKey(RandomKey);
                        temp_List.Remove(RandomKey);
                    }
                    tempTargets.Add(collision.transform);
                }
            }
            else if (type == BlackHoleType.Explode) {
               if(!targets.Contains(collision.transform))
                targets.Add(collision.transform);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>()._FreezeTime(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null) {
            collision.GetComponent<Enemy>()._FreezeTime(false);
        }
    }

    public void Init(float maxSize, float exsitDuration, float growSpeed, float backSpeed,int amountOfAttacks,float cloneAttackCooldown,BlackHoleType type) {
        this.maxSize = maxSize;
        this.exsitDuration = exsitDuration;
        this.exsitTimer = exsitDuration;
        this.growSpeed = growSpeed;
        this.backSpeed = backSpeed;
        this.amountOfAttacks = amountOfAttacks;
        this.cloneAttackCooldown = cloneAttackCooldown;
        this.type = type;
        foreach (var keyCode in QTE_List)
        {
            temp_List.Add(keyCode);
        }
    }
}
