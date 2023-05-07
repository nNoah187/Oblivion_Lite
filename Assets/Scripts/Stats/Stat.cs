using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is from youtube.com/@Brackeys
 */
[System.Serializable]
public class Stat
{
    [SerializeField]
    private float baseValue;
    [SerializeField]
    private String name;

    public float GetValue()
    {
        return baseValue;
    }

    public void SetValue(float newValue)
    {
        baseValue = newValue;
    }

    public String GetName()
    {
        return name;
    }
}
