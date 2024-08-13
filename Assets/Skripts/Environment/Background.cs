using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] Transform followingTarget;
    [SerializeField, Range(0f, 1f)] float paralaxStrenght = 0.1f;
    [SerializeField] bool disableVerticalParallax;
    Vector3 targetPreviosPosition;

    private void Start()
    {
        if (!followingTarget)
        {
            followingTarget = Camera.main.transform;
        }

        targetPreviosPosition = followingTarget.position;
    }

  
    void FixedUpdate()
    {
        var delta = followingTarget.position - targetPreviosPosition; ;

        if (disableVerticalParallax)
            delta.y = 0;
        targetPreviosPosition = followingTarget.position;
        transform.position += delta * paralaxStrenght;
    }
}
