using System.Collections;
using System.Collections.Generic;
using Attacks;
using Damage;
using Entities;
using Entities.Modifiers;
using Health;
using Knockback;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum PlayerDashType
{
    Mouse,
    Movement
}

public enum PlayerAttackAction
{
    None,
    Melee,
    Ranged
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
    [SerializeField] private float meleeKnockbackForce = 10f;
    [SerializeField] private float attackCooldown = 0.25f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float iTime = .5f;
    [SerializeField] private Color dashColor;
    [SerializeField] private Animator slashAnim;
    
    
    [Space(5)]
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
    private float ogMoveSpeed;
    private Vector3 ogAttackScale;
    private float ogDashCooldown;

    private PlayerDashType dashType = PlayerDashType.Movement;
    
    public MeleeModifierController meleeModifiers;
    public PlayerModifiersController playerModifiers;
    public RangedModifierController rangedModifiers;
    public DashModifiersController dashModifiers;

    public Animator animator;
    
    public HealthComponent Health => healthComponent;

    public RangedWeapon RangedWeapon => rangedWeapon;

    [Header("UI Elements")] 
    [SerializeField] private Image ammoImage;
    [SerializeField] private Image hpImage;
    [SerializeField] private GameObject pausePanel;

    
    public bool pauseOpen;

    private bool shootInput = false;
    private bool attackInput = false;

    public Camera mainCamera;

    private PlayerAttackAction currentAttack = PlayerAttackAction.None;

    private Dictionary<Attack, Animator> slashes = new();

    [Space(5)]
    [SerializeField] private float slashRadiusFromPlayer = 1.226f;

    [SerializeField] private List<Attack> slashObjects;
    
    private void Awake()
    {
        ogColor = sprite.color;
        ogMaxHealth = healthComponent.MaxValue;
        ogMoveSpeed = moveSpeed;
        ogDashCooldown = dashCooldown;

        ogAttackScale = attack.transform.parent.localScale;

        UpdateAttackStats();
        
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
        var pauseAction = input.currentActionMap.FindAction("Pause");

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        attackAction.started += OnAttack;
        attackAction.canceled += OnAttack;

        pauseAction.started += OnPause; 
        dashAction.started += OnDash;

        shootAction.started += OnShoot;
        shootAction.canceled += OnShoot;

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
        var pauseAction = input.currentActionMap.FindAction("Pause");
        
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        
        attackAction.started -= OnAttack;
        attackAction.canceled -= OnAttack;
        
        dashAction.started -= OnDash;

        pauseAction.started -= OnPause;
        
        dash.Finished -= OnDashFinished;
        
        shootAction.started -= OnShoot;
        shootAction.canceled -= OnShoot;
        
        healthComponent.OnValueChanged -= OnHpChanged;
        
        attack.OnHit -= OnAttackHit;
        rangedWeapon.OnAmmoChanged -= OnAmmoUpdated;
    }

    private void OnHpChanged(float current, float max)
    {
        hpImage.fillAmount = current / max;
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
        //if (context.performed || context.canceled) return;


        if (context.started)
        {
            attackInput = true;
            currentAttack = PlayerAttackAction.Melee;
        } 
        else if (context.canceled)
        {
            attackInput = false;
            currentAttack = shootInput ? PlayerAttackAction.Ranged : PlayerAttackAction.None;
        }

        //Attack();
    }

    private void Attack()
    {
        if (!isAttackReady) return;
        if (dash.IsDashing && dash.TimeRemain < 0.8f) return;
        
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        var dir = (mousePos - transform.position).normalized;
        if (Gamepad.current != null)
        {
            dir = lastMoveDir;
        }
        var angle = Mathf.Atan2(dir.y, dir.x);

        var angleOffset = 0f;

        if (slashes.Count is 2)
        {
            angleOffset = 30;
        }

        if (slashes.Count is 4)
        {
            
            angleOffset = -30;
        }
        
        
        attackPivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle - 90f + angleOffset);

        //var totalDamage = damage + damage * (stats.damage * 0.01f);

        foreach (var slash in slashes)
        {
            slash.Key.Execute(meleeModifiers.TotalDamage, 0, false);
            slash.Value.Play("Slash", 0, 0);
        }
        //attack.Execute(meleeModifiers.TotalDamage, 0, false);

        isAttackReady = false;

        var totalAttackCD = this.attackCooldown - this.attackCooldown * (stats.attackDelay * 0.01f);
        totalAttackCD = Mathf.Clamp(totalAttackCD, 0.12f, 1f);
        attackTimer.UpdateWaitTime(totalAttackCD);
        attackTimer.Start();
        
        //slashAnim.Play("Slash", 0, 0);
    }

    public void UpdateAttackStats()
    {
        var totalDamage = meleeModifiers.GetTotalValue(MeleeModifierType.Damage, damage, stats.damage);
        var totalKnockback = meleeModifiers.GetTotalValue(MeleeModifierType.Knockback, meleeKnockbackForce);

        meleeModifiers.TotalDamage = totalDamage;
        //attack.damage = totalDamage;
        attack.knockbackForce = totalKnockback;
        
        var radius = meleeModifiers.GetTotalValue(MeleeModifierType.Radius, 1);
        attack.transform.parent.localScale = ogAttackScale * radius;
        
        var ammoGen = meleeModifiers.GetTotalValue(MeleeModifierType.AmmoGeneration, rangedWeapon.OgAmmoGen);
        rangedWeapon.generatePerHit = ammoGen;

        print(totalDamage + " " + totalKnockback + " " + radius + " " + ammoGen);
    }

    public void UpdateRangedStats()
    {
        var totalDamage = rangedModifiers.GetTotalValue(RangedModifierType.Damage, rangedWeapon.OgDamage);
        rangedWeapon.TotalDamage = totalDamage;

        var totalCooldown = rangedModifiers.GetTotalValue(RangedModifierType.Cooldown, rangedWeapon.OgCooldown);
        rangedWeapon.TotalCooldown = totalCooldown;
        //print(totalCooldown);

        //rangedWeapon.firDot = rangedModifiers.fireDot;
    }

    public void UpdateDashStats()
    {
        var totalSpeed = dashModifiers.GetTotalValue(DashModifierType.Speed, dash.OgSpeed, stats.moveSpeed * .5f);
        dash.SetSpeed(totalSpeed);
        
        var totalCooldown = dashModifiers.GetTotalValue(DashModifierType.Cooldown, ogDashCooldown);
        dashCooldown = totalCooldown;
    }

    public void UpdatePlayerStats()
    {
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
                mousePos.z = 0f;
                dir = (mousePos - transform.position).normalized;
                break;
        }
        
        hitbox.excludeLayers = LayerMask.GetMask("Enemy");
        sprite.color = dashColor; // new Color(ogColor.r, ogColor.g, ogColor.b, .5f);
        
        //dash.SetSpeed(dash.OgSpeed + dash.OgSpeed * (stats.moveSpeed * 0.01f * 0.5f));
        dash.Execute(dir);
        
        animator.Play("PlayerDash");

        isDashReady = false;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shootInput = true;
            currentAttack = PlayerAttackAction.Ranged;
            //Shoot();
        } 
        else if (context.canceled)
        {
            shootInput = false;
            currentAttack = attackInput ? PlayerAttackAction.Melee : PlayerAttackAction.None;
        }
    }

    public void Shoot()
    {
        //var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        var dir = (mousePos - transform.position).normalized;
        // print(mousePos);
        // print(dir);
        rangedWeapon.Shoot(dir.normalized);
    }

    private void OnDashFinished()
    {
        hitbox.excludeLayers = 0;
        sprite.color = ogColor;

        dashCooldownTimer.UpdateWaitTime(dashCooldown);
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
    {;
        switch (value)
        {
            case 0:
                ammoImage.fillAmount = 0f;
                break;
            case 1:
                ammoImage.fillAmount = 0.23f;
                break;
            case 2:
                ammoImage.fillAmount = 0.36f;
                break;
            case 3:
                ammoImage.fillAmount = 0.52f;
                break;
            case 4:
                ammoImage.fillAmount = 0.67f;
                break;
            case 5:
                ammoImage.fillAmount = 0.81f;
                break;
            case 6:
                ammoImage.fillAmount = 1f;
                break;
        }
    }

    private void OnDashCooldown()
    {
        isDashReady = true;
    }


    private void OnPause(InputAction.CallbackContext context)
    {
        if (pauseOpen)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            pauseOpen = false;
        }
        else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            pauseOpen = true;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        GameManager.instance.Player = this;
        
        slashes.Add(attack, slashAnim);
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer.Tick(Time.deltaTime);
        iFramesTimer.Tick(Time.deltaTime);
        dashCooldownTimer.Tick(Time.deltaTime);

        //healthComponent.MaxValue = ogMaxHealth + ogMaxHealth * (stats.health * 0.01f);
        UpdateHP();
        statsText.text = $"Stats:\n" +
                         $"Damage: {GetStatValue(PlayerStats.Damage)}\n" +
                         $"Move Speed: {GetStatValue(PlayerStats.MoveSpeed)}\n" +
                         $"Attack Cooldown: {GetStatValue(PlayerStats.AttackDelay)}\n" +
                         $"Max Health: {GetStatValue(PlayerStats.Health)}\n";

        // if (shootInput)
        // {
        //     if (rangedWeapon.IsReady)
        //     {
        //         Shoot();
        //     }
        // }
        //
        // if (attackInput)
        // {
        //     Attack();
        // }

        switch (currentAttack)
        {
            case PlayerAttackAction.Melee:
                Attack();
                break;
            case PlayerAttackAction.Ranged:
                if (rangedWeapon.IsReady)
                {
                    Shoot();
                }
                break;
        }
        
    }

    private void UpdateHP()
    {
        var baseHP = ogMaxHealth;
        var hp = playerModifiers.GetTotalValue(PlayerModifierType.HP, baseHP, stats.health);
        healthComponent.MaxValue = hp;
        
        OnHpChanged(healthComponent.Value, healthComponent.MaxValue);
    }

    private void FixedUpdate()
    {
        if (healthComponent.isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        //moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        if (knockback.IsRunning) return;
        if (dash.IsDashing) return;
        Move();

        sprite.flipX = lastMoveDir.x < 0;
        //print(rb.velocity);
    }


    public void Move()
    {
        var totalMoveSpeed =
            playerModifiers.GetTotalValue(PlayerModifierType.MoveSpeed, ogMoveSpeed, stats.moveSpeed);
        //var totalMS = moveSpeed + moveSpeed * (stats.moveSpeed * 0.01f);
        rb.velocity = moveInput * totalMoveSpeed;

        if (moveInput == Vector2.zero)
        {
            animator.Play("PlayerIdle");
        }
        else
        {
            animator.Play("PlayerRun");
        }
    }

    public void TakeDamage(DamageMessage message)
    {
        if (healthComponent.isDead) return;
        if (dash.IsDashing) return;
        if (invincible) return;
        
        healthComponent.Remove(message.damage);

        if (healthComponent.isDead)
        {
            Die();
            return;
        }

        StartCoroutine(Flash());
        
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

    public void Die()
    {
        animator.Play("PlayerDeath");
        sprite.color = ogColor;
        input.currentActionMap.Disable();
    }
    
    protected IEnumerator Flash()
    {
        sprite.material.SetFloat("_Amount", 1);
        yield return new WaitForSeconds(0.3f);
        sprite.material.SetFloat("_Amount", 0);
    }

    public void AddSlash()
    {
        if (slashes.Count == slashObjects.Count) return;

        var newSlash = slashObjects[slashes.Count];
        
        
        // var angle = slashes.Count * 30f;
        // var radius = slashRadiusFromPlayer;//(attack.transform.parent.position - attack.transform.position).magnitude;
        // var x = attack.transform.parent.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        // var y = attack.transform.parent.position.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        //
        // var pos = new Vector2(x, y);
        //
        // var newSlash = Instantiate(attack, pos, Quaternion.identity, attack.transform.parent);
        //
        // var angleDir = slashes.Count % 2 == 0 ? 1 : -1;
        //
        // newSlash.transform.eulerAngles = new Vector3(0, 0, angle * angleDir);
        
        slashes.Add(newSlash, newSlash.GetComponent<Animator>());
    }
    
    
}

public enum PlayerStats
{
    Damage,
    MoveSpeed,
    AttackDelay,
    Health
}