using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{

    [SerializeField] private float timeForNextAttack = 1f;
    [SerializeField] private float currentAttackTime;
    [SerializeField] private int damage = 5;
    private Player currentPlayer;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            currentPlayer = player;        
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && player == currentPlayer)
        {
            currentPlayer = null;
        }
    }

    private void Update()
    {
        if (currentPlayer != null)
        { 
            if(Time.time >= currentAttackTime)
            {
                currentAttackTime = Time.time + timeForNextAttack;
                currentPlayer.TakeDamage(damage);
            }
        }
    }

}
