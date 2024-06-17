using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWerewolfVisual : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private BlackWerewolf body;

    private const string IS_WALKING = "IsWalking";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_HURT = "IsHurt";
    private const string IS_DEAD = "IsDead";
    private const string IS_ATTACK = "Attack";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        body.onTakeHurt += Body_onTakeHurt;
        body.death += Body_death;
        body.attack += Body_attack;
    }

    private void Body_attack(object sender, System.EventArgs e)
    {
        animator.SetTrigger(IS_ATTACK);
    }

    private void Body_death(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DEAD, true);
    }

    private void Body_onTakeHurt(object sender, System.EventArgs e)
    {
        animator.SetTrigger(IS_HURT);
        //animator.SetBool(IS_WALKING, false);
        //animator.SetBool(IS_RUNNING, false);
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, body.IsWalking());
        animator.SetBool(IS_RUNNING, body.IsRunning());      
    }

    private void ChangeToIdleShape()
    {
        body.ChangeToIdleShape();
    }

    private void ChangeToWalkingShape()
    {
        body.ChangeToWalkingShape();
    }

    private void ChangeToRunningShape()
    {
        body.ChangeToRunningShape();
    }

    private void IsHurtEnd()
    {
        body.IsHurtEnd();
    }
    private void ChangeToAttackShape()
    {
        body.ChangeToAttackShape();
    }

    private void SetIsntAttacking()
    {
        body.SetIsntAttacking();
    }
}
