using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;


    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(currentHealth);

        if (currentHealth < 0)
        {
            Die();
        }
    }
    
    protected virtual void Die()
    {
        //nothing
    }
}
