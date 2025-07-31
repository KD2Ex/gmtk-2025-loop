using System;
using System.Collections;
using System.Collections.Generic;
using Attacks;
using Damage;
using Health;
using Knockback;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private KnockbackComponent knockback;
    [SerializeField] private Attack attack;
    [SerializeField] private Transform attackPivot;
    [SerializeField] private float damage;
    [SerializeField] private float attackCooldown = 0.25f;
    
    private Rigidbody2D rb;
    private PlayerInput input;

    private Vector2 moveInput;

    private Timer attackTimer;
    private bool attackReady = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();

        knockback.velocitySetter = SetVelocity;

        attackTimer = new Timer(attackCooldown, true);
        attackTimer.Timeout += OnAttackTimerTimeout;
    }

    private void OnEnable()
    {
        var moveAction = input.currentActionMap.FindAction("Move");
        var attackAction = input.currentActionMap.FindAction("Attack");


        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        attackAction.started += OnAttack;
        attackAction.canceled += OnAttack;
    }

    private void OnDisable()
    {
        var moveAction = input.currentActionMap.FindAction("Move");
        var attackAction = input.currentActionMap.FindAction("Attack");
        
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        
        attackAction.started -= OnAttack;
        attackAction.canceled -= OnAttack;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled) return;

        if (!attackReady) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = (mousePos - transform.position).normalized;
        var angle = Mathf.Atan2(dir.y, dir.x);
        attackPivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle - 90f);
        attack.Execute(damage);

        attackReady = false;

        attackTimer.Start();
    }

    private void OnAttackTimerTimeout()
    {
        attackReady = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer.Tick(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        if (knockback.IsRunning) return;
        Move();
        //print(rb.velocity);
    }


    public void Move()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    public void TakeDamage(DamageMessage message)
    {
        healthComponent.Remove(message.damage);

        if (message.knockbackForce > 0)
        {
            knockback.Execute(message.dir, message.knockbackForce);
        }
        print("Player's Heath's: " + healthComponent.Value);
    }

    private void SetVelocity(Vector2 dir, float force)
    {
        rb.velocity = dir * force;
        //print(rb.velocity);
    }
}