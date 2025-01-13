using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class Clone_Skill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    public GameObject CreateClone(Transform clonePosition,string cloneState,Direction.Dir faceDirection) 
    { 
        GameObject clone = Instantiate(clonePrefab);
        clone.GetComponent<PlayerClone>().InitClone(clonePosition,this,cloneState,faceDirection);
        return clone;
    }

    public GameObject CreateClone(Transform clonePosition, string cloneState, Direction.Dir faceDirection,Vector3 offset)
    {
        GameObject clone = Instantiate(clonePrefab);
        clone.GetComponent<PlayerClone>().InitClone(clonePosition, this, cloneState, faceDirection,offset);
        return clone;
    }
    public override bool CkeckAndUseSkill()
    {
        return base.CkeckAndUseSkill();
    }
    public override void UseSkill()
    {
        base.UseSkill();
        CreateClone(player.transform,"DashAttack",player.faceDirection);
    }
}
