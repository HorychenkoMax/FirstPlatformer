using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAtteckBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private const string IS_MOVE = "IsMove";
    private const string IS_DESTROY = "IsDestroy";

    [SerializeField] private float speed = 20f;
    [SerializeField] private float liveTime = 1f;
    [SerializeField] private float currentLiveTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        animator.SetTrigger(IS_MOVE);

        rb.velocity = transform.right * speed;
        currentLiveTime = Time.time + liveTime;
    }

    private void Update()
    {
        
        if(Time.time >= currentLiveTime)
        {
            animator.SetTrigger(IS_DESTROY);
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy =  collision.GetComponent<Enemy>();
        
        if(enemy != null)
        {
            enemy.TakeDamage(Player.Instance.GetSimpleAttackDamage());
        }

        Player player = collision.GetComponent<Player>();
        if(player != null)  return;


        rb.velocity = Vector2.zero;
        animator.SetTrigger(IS_DESTROY);
    }

    private void BulletDestroy()
    {
        Destroy(gameObject);
    }
}
