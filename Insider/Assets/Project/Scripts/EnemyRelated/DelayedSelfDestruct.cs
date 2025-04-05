using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSelfDestruct : MonoBehaviour

{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(WaitForAnimationEnd());
    }

    private IEnumerator WaitForAnimationEnd()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}

