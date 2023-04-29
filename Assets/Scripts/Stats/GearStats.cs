using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearStats : MonoBehaviour
{
    public int level;
    public Vector3 localPos;
    public Vector3 localRot;

    private float totalGearValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public float CalculateTotalGearValue(Stat gearValue, int gearLevel)
    //{
    //    totalGearValue = gearValue.GetValue() * gearLevel;
    //    return totalGearValue;
    //}

    public Color GetGearImprovementColor(float oldGearInt, float newGearInt)
    {
        if (newGearInt < oldGearInt)
        {
            return Color.red;
        }
        else if (newGearInt > oldGearInt)
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
