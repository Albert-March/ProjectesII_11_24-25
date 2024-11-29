using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public GameObject[] gameObjects; // Array of GameObjects with Animators

    void Start()
    {
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogError("GameObjects array is empty!");
            return;
        }

        foreach (GameObject obj in gameObjects)
        {
            Animator animator = obj.GetComponent<Animator>();

            if (animator == null)
            {
                Debug.LogWarning($"GameObject {obj.name} does not have an Animator component.");
                continue;
            }

            List<string> animationClips = new List<string>();
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                animationClips.Add(clip.name);
            }

            if (animationClips.Count == 0)
            {
                Debug.LogWarning($"Animator on {obj.name} has no animations.");
                continue;
            }

            int randomIndex = Random.Range(0, animationClips.Count);
            string randomAnimation = animationClips[randomIndex];

            float randomOffset = Random.Range(0f, 1f);
            animator.speed = 0f;
            animator.Play(randomAnimation, 0, randomOffset);
            animator.speed = 1f;
        }
    }
}
