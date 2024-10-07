using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Player Ablity
    [Header("Player Ablity")]
    [SerializeField] public float jumpForce;
    [SerializeField] public float moveSpeed;

    #endregion
    #region Componets
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Collider2D collider2D { get; private set; }
    #endregion

    #region PlayerStates
    public PlayerStateMachine stateMachine { get; private set; }
    public IdleState idleState { get; private set; }
    public MoveState moveState { get; private set; }

    #endregion

    private void Awake()
    {

        stateMachine = new PlayerStateMachine();
        idleState = new IdleState(this,stateMachine,"Idle");
        moveState = new MoveState(this, stateMachine, "Move");
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }
    public void SetVelocity(float xVelocity,float yVelocity) { 
        rb.velocity = new Vector2 (xVelocity,yVelocity);
    }
}
