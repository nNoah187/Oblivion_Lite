using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is from youtube.com/Brackeys
 */
[System.Serializable]
public class Stat
{
    [SerializeField]
    private float baseValue;

    public float GetValue()
    {
        return baseValue;
    }
}
