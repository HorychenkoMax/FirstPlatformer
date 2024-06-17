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

    private bool isEnd = true;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!body.IsHurt())
        {
            
        animator.SetBool(IS_WALKING, body.IsWalking());
        animator.SetBool(IS_RUNNING, body.IsRunning());
        }
        if (body.IsHurt() && isEnd)
        {
            isEnd = false;
            animator.SetTrigger(IS_HURT);
            //body.SetHurtOut();
        }
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
    private void SetHurtOut()
    {
        body.SetHurtOut();
        isEnd = true;
    }
}
