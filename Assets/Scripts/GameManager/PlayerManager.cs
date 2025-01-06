using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManger : MonoBehaviour
{
    public static PlayerManger instance;
    [SerializeField]public Player player; 

    public void Awake()
    {
        if(PlayerManger.instance != null ){ 
            Destroy(instance.gameObject);
        }else { 
            instance = this;
        }    
    }
}
