using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material whiteMaterial;
    [SerializeField] public float flashCD;
    [SerializeField] private float flashTime;


    [Header("Stunned Blink FX")]
    [SerializeField] public float stunnedFlashCD;
    private Material orignalMaterial;

    [Header("EffectAnimation")]
    private Transform effectAnimator;
    public UseSkillEffectAnimation useSkillEffectAnimation;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        orignalMaterial = sr.material;
        effectAnimator = transform.Find("EffectAnimator");
        if (effectAnimator != null) {
            useSkillEffectAnimation = effectAnimator.GetComponentInChildren<UseSkillEffectAnimation>();
        }
    }

    public IEnumerator FlashFX()
    {
        sr.material = whiteMaterial;
        yield return new WaitForSeconds(flashCD);
        sr.material = orignalMaterial;
    }

    public IEnumerator DefenceFX()
    {

        sr.color = new Color(1f, 1f, 0f, 0.8f);
        yield return new WaitForSeconds(flashCD);
        sr.color = Color.white;
    }

    public void BlinkFX()
    {
        if (sr.material.Equals(orignalMaterial))
        {
            sr.material = whiteMaterial;
        }
        else 
        {
            sr.material = orignalMaterial;
        }
    }

    public void StunnedBlinkFX() {
        if (sr.color == Color.white)
        {
            sr.color = Color.red;
        }
        else {
            sr.color = Color.white;
        }
    }

    public void CancelStunnedBlinkFX()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void CancelBlinkFX()
    {
        CancelInvoke();
        sr.material = orignalMaterial;
    }
}