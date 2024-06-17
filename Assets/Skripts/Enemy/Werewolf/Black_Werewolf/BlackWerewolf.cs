using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class BlackWerewolf : Enemy
{
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider2D;

    [Header("Characteristics of enemy")]
    [SerializeField] private float visionRadius = 2f;

    [Space]
    [SerializeField] private int positionOfPatrol;
    [SerializeField] private Transform point;

    [Space]
    [Header("Enemy Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 7f;
    //[SerializeField] private float jumpForce = 8f;
    //[SerializeField] private float jumpSpeed = 2f;
    [SerializeField] private float walkingTime = 3f;
    [SerializeField] private float currentWalkingTime;
    private Vector2 targetPosition;

    [Space]
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float knockbackVerticalForce = 5f;

    [Space]
    [Header("Enemy state")]
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isPlayerInSight = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isAttacing = false;

    [Space]
    [Header("Attack setings")]
    [SerializeField] private Transform simpleAttackCircle;
    [SerializeField] private float radiusOfSimpleAttack = 0.2f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private LayerMask enemyLayers;
    [Space]
    // [SerializeField] private Transform mainAttackCircle;
    //[SerializeField] private float radiusOfMainAttack = 1f;

    [SerializeField] private float nextAttack = 1f;
    [SerializeField] private float currentTimeAtteck;
    

    private bool isHit = false;

    public event EventHandler onTakeHurt;
    public event EventHandler death;
    public event EventHandler attack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    
    protected override void Start()
    {
        base.Start();
    }
    

    
    void Update()
    {

        if (!isHit && !isDead)
        {
            if (Mathf.Abs(Player.Instance.transform.position.x - simpleAttackCircle.transform.position.x) <= 1f)
            {
                isRunning = false;
                rb.velocity = Vector2.zero;
                if (Time.time >= currentTimeAtteck)
                { 
                    isAttacing = true;
                    currentTimeAtteck = Time.time + nextAttack;
                    attack?.Invoke(this, EventArgs.Empty);
                    Attack();
                }
            }
            else if(!isAttacing)
            {

            
            IsPlayerInSight();
            if (!isPlayerInSight)
            {
                if (Time.time >= currentWalkingTime)
                {
                    SetTargetPositio();
                    currentWalkingTime = Time.time + walkingTime;
                }
                Walking();
            }
            else if (isPlayerInSight)
            {
                Running();
            }
            }

        }

    }

    private void SetTargetPositio()
    {
        float randomPoint = UnityEngine.Random.Range(point.position.x - positionOfPatrol, point.position.x + positionOfPatrol);
        targetPosition = new Vector2(randomPoint, transform.position.y);       
    }

    private void Walking()
    {
        Flip();
        transform.position = Vector2.MoveTowards(transform.position, targetPosition , walkSpeed * Time.deltaTime);
        if (transform.position.x != targetPosition.x)
        {
            isWalking = true;
        }else if(transform.position.x == targetPosition.x)
        {
            isWalking = false;
        }
        
    }

    private void Running()
    {
        targetPosition = Player.Instance.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);
        Flip();
    }

    private void Flip()
    {
        if (targetPosition.x > transform.position.x && !isFacingRight)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (targetPosition.x < transform.position.x && isFacingRight)
        {
            isFacingRight = false;
            transform.Rotate(0f, 180f, 0f);
        }
    }
    
    private void IsPlayerInSight()
    {
        if (Mathf.Abs(Player.Instance.transform.position.x - transform.position.x) <= visionRadius)
        {
            isPlayerInSight = true;
            isRunning = true;

        }else if (Mathf.Abs(Player.Instance.transform.position.x - transform.position.x) > visionRadius)
        {
            isPlayerInSight= false;
            isRunning = false;
        }
        
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;

        rb.velocity = Vector2.zero;
        isRunning = false;

        isHit = true;

        Vector2 knockbackDirection = (transform.position - Player.Instance.transform.position).normalized;
        Vector2 knockbackForceVector = new Vector2(knockbackDirection.x * knockbackForce, knockbackVerticalForce);
        rb.AddForce(knockbackForceVector, ForceMode2D.Impulse);
        onTakeHurt?.Invoke(this, EventArgs.Empty);

        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        death?.Invoke(this, EventArgs.Empty);
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (simpleAttackCircle == null) return;
        Gizmos.DrawWireSphere(simpleAttackCircle.position, radiusOfSimpleAttack);
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(simpleAttackCircle.position, radiusOfSimpleAttack, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }

    public void IsHurtEnd()
    {
        isHit = false;
    }

    public void SetIsntAttacking()
    {
        isAttacing = false;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void ChangeToIdleShape()
    {
        Vector2[] idle = new Vector2[]
        {
            new Vector2(0.3011649f, 0.9768334f),
            new Vector2(-0.05457568f, 0.8013536f),
            new Vector2(-0.1788386f, 0.4480163f),
            new Vector2(-0.3483977f, -0.1371874f),
            new Vector2(0.5643814f, -0.1232785f),
            new Vector2(0.5105637f, 0.4960316f),
            new Vector2(0.8169248f, 0.4770044f),
            new Vector2(0.9278901f, 0.5870156f)
        };

        polygonCollider2D.SetPath(0, idle);
    }

    public void ChangeToWalkingShape()
    {
        Vector2[] walk = new Vector2[]
        {
            new Vector2(0.2485421f, 1.305726f),
            new Vector2(-0.2058661f,  1.14998f),
            new Vector2(-0.4485305f, 0.4874833f),
            new Vector2(-0.5983562f, -0.1503431f),
            new Vector2(0.08048892f, -0.1504941f),
            new Vector2(0.3538902f, 0.2713925f),
            new Vector2(0.3066505f, 0.9235919f),
            new Vector2(0.6656343f, 0.8650975f),
            new Vector2(0.8094887f, 0.9619529f)
        };
        polygonCollider2D.SetPath(0, walk);
    }

    public void ChangeToRunningShape()
    {
        Vector2[] run = new Vector2[]
        {
            new Vector2(0.8795002f, 0.6792324f),
            new Vector2(0.3788815f, 0.7843674f),
            new Vector2(-0.3362057f, 0.8784388f),
            new Vector2(-0.4702538f, 0.7155776f),
            new Vector2(-0.9242048f, -0.1612047f),
            new Vector2(-0.473454f, -0.1613557f),
            new Vector2(-0.1939149f, 0.5378933f),
            new Vector2(0.3213055f, 0.3474239f),
            new Vector2(0.6216377f, -0.1517096f),
            new Vector2(0.8489027f, -0.149145f),
            new Vector2(0.785112f, 0.4632172f),
            new Vector2(1.059306f, 0.4514562f)
        };
        polygonCollider2D.SetPath(0, run);
    }

    public void ChangeToAttackShape()
    {
        Vector2[] attack = new Vector2[] {
            new Vector2(0.2877052f, -0.1828066f),
            new Vector2(0.3642948f, 0.493615f),
            new Vector2(0.3552685f, 1.037311f),
            new Vector2(0.7985688f, 1.226522f),
            new Vector2(0.5227307f, 1.519367f),
            new Vector2(-0.0234828f, 1.322091f),
            new Vector2(-0.5214005f, 0.4602648f),
            new Vector2(-0.3698759f, -0.1728645f)
        };
        polygonCollider2D.SetPath(0, attack);
    }
}
