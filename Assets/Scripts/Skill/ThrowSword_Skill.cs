using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum SwordType 
{ 
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class ThrowSword_Skill : Skill
{
    #region Sword Throw Info
    [Header("Sword Throw Info")]
    [SerializeField] private GameObject SwordPrefab;
    [SerializeField] private Vector2 luachSpeed;
    [SerializeField] private float SwordGravity;
    [SerializeField] private SwordType swordType;
    private Vector2 finalSpeed;
    private Vector2 startPosition;
    #endregion

    #region luanchDegree
    [Header("luanchDegree")]
    [SerializeField] private float startAngleInDegrees;
    [SerializeField] private float endAngleInDegrees;
    [SerializeField] public float currentAngleInDegrees;
    #endregion

    #region SwordNumb
    [Header("SwordNumb")]
    [SerializeField] public int SowrdMaxSize;
    [SerializeField] public int SowrdNum;
    #endregion

    //µ¯Ìø½£
    #region Bounce Sword Info
    [Header("Bounce Sword Info")]
    [SerializeField] private float bounceSwordGravity;
    [SerializeField] private int canBounceTimes;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceRadius;
    #endregion

    //´©´Ì½£
    #region Pierce Sword Info
    [Header("pierce Sword Info")]
    [SerializeField] private float pierceSwordGravity;
    [SerializeField] private float pierceSwordExsitTime;

    //Ðý×ª½£
    #endregion
    //Ðý×ª½£
    #region Spin Sword Info
    [Header("Spin Sword Info")]
    [SerializeField] private float spinSwordGravity;
    [SerializeField] private float swordMoveMaxDistance;
    [SerializeField] private float spinSwordExsitTime;
    #endregion

    #region Aim dots
    [Header("Aim dots")]
    [SerializeField] private int numberofDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    private GameObject[] dots;
    #endregion


    public void CreateSword() {
        GameObject sword = Instantiate(SwordPrefab);
        Sword swordScript = sword.GetComponent<Sword>();

        if (swordType == SwordType.Bounce)
        {
            swordScript.SetUpBounceSword(true, canBounceTimes, bounceSpeed, bounceRadius);
        }
        else if (swordType == SwordType.Pierce)
        {
            swordScript.SetUpPierceSword(true,pierceSwordExsitTime);
        }
        else if (swordType == SwordType.Spin) {
            swordScript.SetUpSpainSword(true, spinSwordExsitTime, swordMoveMaxDistance, player.transform.position);
        }

        switch (swordType)
        {
            case SwordType.Regular:
                swordScript.SetUpItem(finalSpeed, SwordGravity);
                break;
            case SwordType.Bounce:
                swordScript.SetUpItem(finalSpeed, bounceSwordGravity);
                break;
            case SwordType.Pierce:
                swordScript.SetUpItem(finalSpeed, pierceSwordGravity);
                break;
            case SwordType.Spin:
                swordScript.SetUpItem(finalSpeed, spinSwordGravity);
                break;
        }
        sword.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
    }
    public void SetStartPoint() {
        startPosition = new Vector2(player.transform.position.x, player.transform.position.y);
    }


    public override void Start()
    {
        base.Start();
        GenereateDots();
        currentAngleInDegrees = startAngleInDegrees; 
        cooldownTimer = cooldown;
    }

    public override void Update()
    {
        base.Update();
        if (SowrdNum < SowrdMaxSize) {
            if (cooldownTimer <= 0)
            {
                cooldownTimer = cooldown;
                SowrdNum++;
            }
        }

        if(CkeckSkill()){
            if (Input.GetKey(KeyCode.V))
            {

                if (currentAngleInDegrees >= startAngleInDegrees && endAngleInDegrees >= currentAngleInDegrees)
                {
                    currentAngleInDegrees++;
                }
                for (int i = 0; i < dots.Length; i++)
                {
                    dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
                }
            }

            if (Input.GetKeyUp(KeyCode.V))
            {
                Vector2 aimDirection = AimDirection(currentAngleInDegrees);
                finalSpeed = new Vector2(aimDirection.x * luachSpeed.x, aimDirection.y * luachSpeed.y);
                currentAngleInDegrees = startAngleInDegrees;
            }
        }
    }

    private void GenereateDots() {
        SetStartPoint();
        dots = new GameObject[numberofDots];
        for (int i = 0; i < numberofDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    public void DotsActive(bool isActive) {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private Vector2 DotsPosition(float t) {
        SetStartPoint();
        Vector2 aimDirection = AimDirection(currentAngleInDegrees);
        finalSpeed = new Vector2(aimDirection.x * luachSpeed.x, aimDirection.y * luachSpeed.y);

        float Gravity = 0;
        switch (swordType)
        {
            case SwordType.Regular:
                Gravity = SwordGravity;
                break;
            case SwordType.Bounce:
                Gravity = bounceSwordGravity;
                break;
            case SwordType.Pierce:
                Gravity = pierceSwordGravity;
                break;
            case SwordType.Spin:
                Gravity = spinSwordGravity;
                break;
        }

        Vector2 postion = (Vector2)startPosition + finalSpeed * t + 0.5f * (Physics2D.gravity * Gravity) * t * t;
        return postion;
    }

    private Vector2 AimDirection(float currentAngleInDegrees) {
        Vector2 resultVector = new Vector2(1,0);
        if (player.faceDirection == Direction.Dir.Right){
            float angleInRadians = currentAngleInDegrees * Mathf.Deg2Rad;
            resultVector = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

        }else if (player.faceDirection == Direction.Dir.Left) {
            float angleInRadians = (180 - currentAngleInDegrees) * Mathf.Deg2Rad;
            resultVector = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
        }
        return resultVector;
    }

    public override bool CkeckSkill()
    {
        if (SowrdNum > 0)
        {
            return true;
        }
        return false;
    }

    public override void UseSkill()
    {
        CreateSword();
    }

    public override bool CkeckAndUseSkill()
    {
        if (SowrdNum > 0) { 
            UseSkill();
            SowrdNum--;
            return true;
        }
        return false;
    }
}
