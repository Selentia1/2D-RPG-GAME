using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decritions : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected Animator animator;
    
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
