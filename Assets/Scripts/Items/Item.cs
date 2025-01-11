using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Item : MonoBehaviour
{
    [Header("Item Info")]
    protected Animator anim;
    protected Rigidbody2D rb;
    protected CircleCollider2D circleCollider;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float exsitDuration;
    protected float existTimer;

    [Header("Mirage Info")]
    [SerializeField] protected bool hasMirage;
    protected List<GameObject> mirages = new List<GameObject>();
    protected float mirageSetTimer;
    [SerializeField] protected float mirageSetDuration;
    [SerializeField] protected float fadeSpped;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update() {
        TimerController();
        AnimationController();
        SetUpMirage(hasMirage);
        MirageFadeFX(hasMirage);
    }

    protected virtual void OnDestroy() {
        foreach (GameObject obj in mirages)
        {
            Destroy(obj);
        }
    }
    public virtual void SetUpItem() { 
        //设置物品初始属性
    }

    public virtual void SetUpItem(Vector2 direction, float gravityScale)
    {
        //设置物品初始属性
    }

    public virtual void SetUpItem(float existDuration)
    {
        //设置物品初始属性
    }

    public virtual void AnimationController()
    {


    }

    public virtual void TimerController()
    {
        if (mirageSetTimer > 0) {
            mirageSetTimer -= Time.deltaTime;
        }
    }

    public virtual void ChangeState(string state)
    {

    }

    public virtual void SetUpMirage(bool hasMirage) { 
        if (hasMirage && mirageSetTimer<=0)
        {
            mirageSetTimer = mirageSetDuration;
            GameObject mirage = new GameObject(gameObject.name);

            mirage.transform.position = spriteRenderer.transform.position;
            mirage.transform.rotation = spriteRenderer.transform.rotation;
            mirage.transform.localScale = spriteRenderer.transform.localScale;

            mirage.AddComponent<SpriteRenderer>();
            mirage.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            mirage.GetComponent<SpriteRenderer>().sortingLayerID = spriteRenderer.sortingLayerID;
            
            mirages.Add(mirage);
        }
    }

    public void MirageFadeFX(bool hasMirage)
    {
        if (mirages.Count > 0) {
            List <GameObject> destroyedGameObjects = new List<GameObject>();
            foreach (GameObject mirage in mirages) {
                if (mirage.GetComponent<SpriteRenderer>().color.a > 0)
                {
                    mirage.GetComponent<SpriteRenderer>().color -= new UnityEngine.Color(0, 0, 0, Time.deltaTime * fadeSpped);
                }
                else if (mirage.GetComponent<SpriteRenderer>().color.a <= 0)
                {
                    destroyedGameObjects.Add(mirage);
                }
            }

            foreach (GameObject destroyedGameObject in destroyedGameObjects) {
                mirages.Remove(destroyedGameObject);
                Destroy(destroyedGameObject);
            }
        }
    }

}
