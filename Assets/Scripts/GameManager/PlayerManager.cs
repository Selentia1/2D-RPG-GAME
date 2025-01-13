using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [SerializeField]public Player player;
    [SerializeField]public PlayerStats stats;

    public void Awake()
    {
        if(PlayerManager.instance != null ){ 
            Destroy(instance.gameObject);
        }else { 
            instance = this;
        }    
    }
}
