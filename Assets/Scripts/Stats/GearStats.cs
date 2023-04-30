using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearStats : MonoBehaviour
{
    public int level;
    public Vector3 localPos;
    public Vector3 localRot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Color GetGearImprovementColor(float baseValue, float comparisonValue)
    {
        if (comparisonValue < baseValue)
        {
            return Color.red;
        }
        else if (comparisonValue > baseValue)
        {
            return Color.green;
        }
        else
        {
            return Color.white;
        }
    }

    public string GetGearImprovementArrow(float oldGearFloat, float newGearFloat)
    {
        if (newGearFloat < oldGearFloat)
        {
            return "↓";
        }
        else if (newGearFloat > oldGearFloat)
        {
            return "↑";
        }
        else
        {
            return "";
        }
    }
}
