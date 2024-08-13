using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const string IS_WALKING = "IsWalking";
    private const string IS_JUMPING = "IsJumping";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_ATTACK = "Attack";
    private const string IS_SIMPLE_ATTACKING = "SimpleAttack";
    private const string IS_ULTIMATE_ATTACKING = "UltimateAttack";
    private const string IS_HITEN = "IsHiten";
    private const string IS_DEAD = "IsDead";

    private bool FacingRight = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Player.Instance.IsAttacing += Instance_IsAttacing;
    }

    private void Instance_IsAttacing(object sender, System.EventArgs e)
    {
        animator.SetTrigger(IS_ATTACK);
    }

    private void Update()
    {
        if (Player.Instance.IsWalking() || Player.Instance.IsRunning() || Player.Instance.IsJumping())
        {
            FlipToMove();
        }
        else 
        {
            FlipToMouse();
        }

        if (Player.Instance.IsDead())
        {
            animator.SetBool(IS_DEAD, Player.Instance.IsDead());
            this.enabled = false;
        }
        animator.SetBool(IS_JUMPING, Player.Instance.IsJumping());
        animator.SetBool(IS_WALKING, Player.Instance.IsWalking());
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        if (Player.Instance.IsSimpleAttacking())
        {
            animator.SetTrigger(IS_SIMPLE_ATTACKING);
        }
        if (Player.Instance.IsUltimateAttacking())
        {
            animator.SetTrigger(IS_ULTIMATE_ATTACKING);
        }
        if (Player.Instance.IsHiten())
        {
            animator.SetTrigger(IS_HITEN);
        }
        
        
        
    
       
    }

    private void FlipToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(FacingRight && mousePosition.x < Player.Instance.transform.position.x)
        {
            spriteRenderer.flipX = true;
            FacingRight = false;
            Player.Instance.Flip();
        }
        else if(!FacingRight && mousePosition.x > Player.Instance.transform.position.x) 
        {
            spriteRenderer.flipX = false;
            FacingRight = true;
            Player.Instance.Flip();
        }
    }

    private void FlipToMove()
    {
        if (FacingRight && Player.Instance.GetHorizontalMove() < 0f)
        {
            spriteRenderer.flipX = true;
            FacingRight = false;
            Player.Instance.Flip();
        }
        else if (!FacingRight && Player.Instance.GetHorizontalMove() > 0f)
        {
            spriteRenderer.flipX = false;
            FacingRight = true;
            Player.Instance.Flip();
        }
    }

    private void SimpleAtteck()
    {
        Player.Instance.SimpleAttack();
    }

    private void SimpleAttackAnimationEnd()
    {
        Player.Instance.SimpleAttackAnimationEnd();
    }

    private void UltimateAttackAnimationEnd()
    {
        Player.Instance.UltimateAttackAnimationEnd();
    }

    private void UltimateAttack()
    {
        Player.Instance.UltimateAttack();
    }

    private void Attack()
    {
        Player.Instance.Attack();
    }
}
