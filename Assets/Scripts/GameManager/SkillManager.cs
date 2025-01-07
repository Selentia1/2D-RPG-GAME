using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash_Skill dash;
    public Clone_Skill clone;
    public ThrowSword_Skill throwSword;
    public BlackHole_Skill blackHole;

    public void Awake()
    {
        if (SkillManager.instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Start()
    {
        dash = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
        throwSword = GetComponent<ThrowSword_Skill>();
        blackHole = GetComponent<BlackHole_Skill>();
    }
}
