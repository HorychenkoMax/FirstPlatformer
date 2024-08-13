using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private Rigidbody2D rb;
    private float horizontalMove = 0f;

    [Header("Characteristics of player")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private float hitJump = 0.4f;

    [Space]
    [Header("Player Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 7f; 
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpSpeed = 2f;
    private float minSpeed = 0.1f;

    [Space]
    [Header("Character state")]
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isSimpleAttacking = false;
    [SerializeField] private bool isSimpleAttackingAnimationEnd = true;
    [SerializeField] private bool isUltimateAttacking = false;
    [SerializeField] private bool isUltimateAttackAnimationEnd = true;
    [SerializeField] private bool isHiten = false;
    [SerializeField] private bool isDead = false;

    [Space]
    [Header("Ground Checker Sattings")]
    [SerializeField] private bool isGround = false;
    [SerializeField] private float checkGroundRadius = 0.1f;

    [Space]
    [Header("Attack Settings")]
    [SerializeField] private Transform attackCircle;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int atteckDamage = 10;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackRate = 2f;
    private float nextAttackTime = 0f;
    [Space]
    [SerializeField] private Transform firePoitForSimpleAttack;
    [SerializeField] private int simpleAttackDamage = 20;
    [SerializeField] private GameObject bulletForSimpleAttack;
    [SerializeField] private float timeForNextSimpleAttack;
    [SerializeField] private float simpleAttackCooldown = 2f;
    [Space]
    [SerializeField] private Transform firePointForUltimateAttack;
    [SerializeField] private int ultimateAttackDamage = 50;
    [SerializeField] private GameObject bulletForUltimateAttack;
    [SerializeField] private float timeForNextUltimateAttack;
    [SerializeField] private float ultimateAttackCooldown = 5f;

    [Space]
    [SerializeField] private Image hp;

    public event EventHandler IsAttacing;
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        
            checkGround();
            HandleMovement();  
        
    }

    private void FixedUpdate()
    {
        if (isSimpleAttackingAnimationEnd && isUltimateAttackAnimationEnd && !isDead)
        {
            Move();
        }
    }


    private void HandleMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        EnableAllAnimations();

        if (isSimpleAttackingAnimationEnd && isUltimateAttackAnimationEnd && !isDead)
        {
            if (horizontalMove != 0 && !isGround)
            {
                horizontalMove *= jumpSpeed;

            }
            else if (horizontalMove != 0 && isGround)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    horizontalMove *= runSpeed;
                }
                else
                {
                    horizontalMove *= walkSpeed;
                }

            }


            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }


            if (Time.time >= nextAttackTime)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    IsAttacing?.Invoke(this, EventArgs.Empty);
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }

            if (Time.time >= timeForNextUltimateAttack)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isUltimateAttacking = true;
                    isUltimateAttackAnimationEnd = false;
                    timeForNextUltimateAttack = Time.time + ultimateAttackCooldown;
                }
            }


            if (Time.time >= timeForNextSimpleAttack)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    isSimpleAttacking = true;
                    isSimpleAttackingAnimationEnd = false;
                    timeForNextSimpleAttack = Time.time + simpleAttackCooldown;
                }
            }
        }
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCircle.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(atteckDamage);
        }
    }

    public void SimpleAttack()
    {
        Instantiate(bulletForSimpleAttack, firePoitForSimpleAttack.position, firePoitForSimpleAttack.rotation);
    }

    public void UltimateAttack()
    {
        Instantiate(bulletForUltimateAttack, firePointForUltimateAttack.position, firePointForUltimateAttack.rotation);
    }

    private void Move()
    {
        Vector3 movement = new Vector3(horizontalMove, 0.0f, 0.0f);
        

        if (Mathf.Abs(movement.x) > minSpeed && Mathf.Abs(movement.x) <= walkSpeed)
        {
            isWalking = true;
            isRunning = false;
        }
        else if (Mathf.Abs(movement.x) > walkSpeed)
        {
            isWalking = false;
            isRunning = true;
        }
        else
        {
            isWalking = false;
            isRunning = false;
        }
        transform.Translate(movement * Time.deltaTime);

    }

    private void checkGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), checkGroundRadius);
        if(colliders.Length > 2)
        {
            isGround = true;
            isJumping = false;
        }
        else
        {
            isGround = false;
            isJumping = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackCircle == null) return;

        Gizmos.DrawWireSphere(attackCircle.position, attackRange);
    }

    private void EnableAllAnimations()
    {
        isHiten = false;
        isSimpleAttacking = false;
        isUltimateAttacking = false;
    }

    public void TakeDamage(int damage)
    {   
            isHiten = true;
            currentHealth -= damage;
            rb.AddForce(transform.up * hitJump, ForceMode2D.Impulse);

            hp.fillAmount = (float)currentHealth / (float)maxHealth;

            isUltimateAttackAnimationEnd = true;
            isSimpleAttackingAnimationEnd = true;

        Debug.Log(currentHealth);

            if (currentHealth <= 0)
            {
            Die();
            }  
    }

    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
    }

    public float GetHorizontalMove()
    {
        return horizontalMove;
    }

    public void Flip()
    {
        FlipPlayerVisual();
        FlipFirePointForSimpleAttack();
        FlipFirePointForUltimateAttack();
    }

    private void FlipPlayerVisual()
    {
        Vector3 newPosition = attackCircle.localPosition;
        newPosition.x *= -1f;
        attackCircle.localPosition = newPosition;
    }

    private void FlipFirePointForSimpleAttack()
    {
        Vector3 newPosition = firePoitForSimpleAttack.localPosition;
        newPosition.x *= -1f;
        firePoitForSimpleAttack.localPosition = newPosition;
        firePoitForSimpleAttack.Rotate(0f, 180, 0f);
    }

    private void FlipFirePointForUltimateAttack()
    {
        Vector3 newPosition = firePointForUltimateAttack.localPosition;
        newPosition.x *= -1f;
        firePointForUltimateAttack.localPosition = newPosition;
        firePointForUltimateAttack.Rotate(0f, 180, 0f);
    }

    public void SimpleAttackAnimationEnd()
    {
        isSimpleAttackingAnimationEnd = true;
    }

    public void UltimateAttackAnimationEnd()
    {
        isUltimateAttackAnimationEnd = true;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsSimpleAttacking()
    {
        return isSimpleAttacking;
    }

    public int GetSimpleAttackDamage()
    {
        return simpleAttackDamage;
    }

    public bool IsUltimateAttacking()
    {
        return isUltimateAttacking;
    }

    public int GetUltimateAttackDamage()
    {
        return ultimateAttackDamage;
    }

    public bool IsHiten()
    {
        return isHiten;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
