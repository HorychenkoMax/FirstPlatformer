using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy parametrs")]
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected float visionYRadius = 2f;
    [SerializeField] protected float visionXRadius = 1f;
    [SerializeField] protected int attackDamage = 10;

    [Space]
    [Header("Enemy state")]
    [SerializeField] protected bool isAttacking = false;
    [SerializeField] protected bool isPlayerInSight = false;
    [SerializeField] protected bool isDead = false;
    [SerializeField] protected bool isFacingRight = true;

    protected Vector2 targetPosition;



    protected virtual void Start()
    { 
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            Die();
        }
    }
    
    protected virtual void Die()
    {
        isDead = true;
    }

    protected virtual void Flip() 
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
    protected virtual void IsPlayerInSight() 
    {
        if (Mathf.Abs(Player.Instance.transform.position.x - transform.position.x) <= visionXRadius && Mathf.Abs(Player.Instance.transform.position.y - transform.position.y) <= visionYRadius)
        {
            isPlayerInSight = true;
        }
        else if (Mathf.Abs(Player.Instance.transform.position.x - transform.position.x) > visionXRadius)
        {
            isPlayerInSight = false;
        }
    }
}
