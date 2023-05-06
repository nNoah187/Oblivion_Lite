using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is from youtbe.com/@CodingWithUnity
 */
public class SetEnemyAnimations : MonoBehaviour
{
    [SerializeField] private AnimatorOverrideController[] overrideControllers;
    [SerializeField] private AnimationOverrider overrider;

    public void Set(int value)
    {
        overrider.SetAnimations(overrideControllers[value]);
    }
}
