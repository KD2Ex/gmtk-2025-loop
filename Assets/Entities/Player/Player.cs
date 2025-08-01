using Attacks;
using Damage;
using Entities;
using Health;
using Knockback;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerDashType
{
    Mouse,
    Movement
}

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private CircleCollider2D hitbox;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private KnockbackComponent knockback;
    [SerializeField] private Attack attack;
    [SerializeField] private Transform attackPivot;
    [SerializeField] private RangedWeapon rangedWeapon;
    [SerializeField] private float damage;
    [SerializeField] private float attackCooldown = 0.25f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float iTime = .5f;
    [SerializeField] private Color dashColor;
    
    [Space(5)]
    
    [SerializeField] private TMP_Text hp;
    [SerializeField] private TMP_Text currentAmmo;
    [SerializeField] private TMP_Text statsText;

    [Space(5)] [Header("Stats")] [SerializeField]
    private StatsSO stats;
    
    private Rigidbody2D rb;
    private Dash dash;
    private PlayerInput input;

    private Vector2 moveInput;
    private Vector2 lastMoveDir;

    private Timer attackTimer;
    private Timer iFramesTimer;
    private Timer dashCooldownTimer;
    
    private bool isAttackReady = true;
    private bool isDashReady = true;
    private bool invincible = false;
    
    private Color ogColor;

    private float ogMaxHealth; 

    private PlayerDashType dashType = PlayerDashType.Movement;
    
    public HealthComponent Health => healthComponent;

    private void Awake()
    {
        ogColor = sprite.color;
        ogMaxHealth = healthComponent.MaxValue;
        
        rb = GetComponent<Rigidbody2D>();
        dash = GetComponent<Dash>();
        input = GetComponent<PlayerInput>();

        knockback.velocitySetter = SetVelocity;

        attackTimer = new Timer(attackCooldown, true);
        attackTimer.Timeout += OnAttackTimerTimeout;
        
        iFramesTimer = new Timer(iTime, true);
        iFramesTimer.Timeout += OnIFramesTimerTimeout;
        
        dashCooldownTimer = new Timer(dashCooldown, true);
        dashCooldownTimer.Timeout += OnDashCooldown;
    }

    private void OnEnable()
    {
        var moveAction = input.currentActionMap.FindAction("Move");
        var attackAction = input.currentActionMap.FindAction("Attack");
        var dashAction = input.currentActionMap.FindAction("Dash");
        var shootAction = input.currentActionMap.FindAction("Shoot");

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        attackAction.started += OnAttack;
        attackAction.canceled += OnAttack;

        dashAction.started += OnDash;

        shootAction.started += OnShoot;

        dash.Finished += OnDashFinished;

        healthComponent.OnValueChanged += OnHpChanged;
        
        attack.OnHit += OnAttackHit;
        rangedWeapon.OnAmmoChanged += OnAmmoUpdated;
    }

    private void OnDisable()
    {
        var moveAction = input.currentActionMap.FindAction("Move");
        var attackAction = input.currentActionMap.FindAction("Attack");
        var dashAction = input.currentActionMap.FindAction("Dash");
        var shootAction = input.currentActionMap.FindAction("Shoot");
        
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        
        attackAction.started -= OnAttack;
        attackAction.canceled -= OnAttack;
        
        dashAction.started -= OnDash;
        
        dash.Finished -= OnDashFinished;
        
        shootAction.started -= OnShoot;
        
        healthComponent.OnValueChanged -= OnHpChanged;
        
        attack.OnHit -= OnAttackHit;
        rangedWeapon.OnAmmoChanged -= OnAmmoUpdated;
    }

    private void OnHpChanged(float current, float max)
    {
        hp.text = current.ToString();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
        if (moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput;
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled) return;

        if (!isAttackReady) return;
        if (dash.IsDashing && dash.TimeRemain < 0.8f) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = (mousePos - transform.position).normalized;
        if (Gamepad.current != null)
        {
            dir = lastMoveDir;
        }
        var angle = Mathf.Atan2(dir.y, dir.x);
        attackPivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle - 90f);

        var totalDamage = damage + damage * (stats.damage * 0.01f);
        
        attack.Execute(totalDamage);

        isAttackReady = false;

        var totalAttackCD = this.attackCooldown - this.attackCooldown * (stats.attackDelay * 0.01f);
        attackTimer.UpdateWaitTime(totalAttackCD);
        attackTimer.Start();
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isDashReady) return;
        
        Dash();
    }

    private void Dash()
    {
        Vector2 dir = lastMoveDir;
        switch (dashType)
        {
            case PlayerDashType.Movement:
                dir = lastMoveDir;
                break;
            case PlayerDashType.Mouse:
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dir = (mousePos - transform.position).normalized;
                break;
        }
        
        hitbox.excludeLayers = LayerMask.GetMask("Enemy");
        sprite.color = dashColor; // new Color(ogColor.r, ogColor.g, ogColor.b, .5f);
        
        dash.SetSpeed(dash.Speed + dash.Speed * (stats.moveSpeed * 0.01f * 0.5f));
        dash.Execute(dir);

        isDashReady = false;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = (mousePos - transform.position).normalized;
        rangedWeapon.Shoot(dir);
    }

    private void OnDashFinished()
    {
        hitbox.excludeLayers = 0;
        sprite.color = ogColor;

        dashCooldownTimer.Start();
    }

    private void OnAttackTimerTimeout()
    {
        isAttackReady = true;
    }

    private void OnIFramesTimerTimeout()
    {
        invincible = false;
        sprite.color = ogColor;
    }

    private void OnAttackHit(Collider2D _)
    {
        rangedWeapon.GenerateAmmo();
    }

    private void OnAmmoUpdated(int value)
    {
        currentAmmo.text = value.ToString();
    }

    private void OnDashCooldown()
    {
        isDashReady = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer.Tick(Time.deltaTime);
        iFramesTimer.Tick(Time.deltaTime);
        dashCooldownTimer.Tick(Time.deltaTime);

        healthComponent.MaxValue = ogMaxHealth + ogMaxHealth * (stats.health * 0.01f);

        statsText.text = $"Stats:\n" +
                         $"Damage: {GetStatValue(PlayerStats.Damage)}\n" +
                         $"Move Speed: {GetStatValue(PlayerStats.MoveSpeed)}\n" +
                         $"Attack Cooldown: {GetStatValue(PlayerStats.AttackDelay)}\n" +
                         $"Max Health: {GetStatValue(PlayerStats.Health)}\n";
    }

    private void FixedUpdate()
    {
        //moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        if (knockback.IsRunning) return;
        if (dash.IsDashing) return;
        Move();
        //print(rb.velocity);
    }


    public void Move()
    {
        var totalMS = moveSpeed + moveSpeed * (stats.moveSpeed * 0.01f);
        rb.velocity = moveInput * totalMS;
    }

    public void TakeDamage(DamageMessage message)
    {
        if (dash.IsDashing) return;
        if (invincible) return;
        
        healthComponent.Remove(message.damage);
        
        invincible = true;
        iFramesTimer.Start();
        sprite.color = Color.white;

        if (message.knockbackForce > 0)
        {
            knockback.Execute(message.dir, message.knockbackForce);
        }
        
        //print("Player's Heath's: " + healthComponent.Value);
    }

    private void SetVelocity(Vector2 dir, float force)
    {
        rb.velocity = dir * force;
        //print(rb.velocity);
    }

    public float GetStatValue(PlayerStats stat)
    {
        switch (stat)
        {
            case PlayerStats.Damage:
                return damage + damage * (stats.damage * 0.01f);
            case PlayerStats.Health:
                return healthComponent.MaxValue;
            case PlayerStats.MoveSpeed:
                return moveSpeed + moveSpeed * (stats.moveSpeed * 0.01f);
            case PlayerStats.AttackDelay:
                return attackCooldown - attackCooldown * (stats.attackDelay * 0.01f);
        }

        return -1f;
    }
}

public enum PlayerStats
{
    Damage,
    MoveSpeed,
    AttackDelay,
    Health
}