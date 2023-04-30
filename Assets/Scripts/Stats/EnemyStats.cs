using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public Stat damage;
    public float baseAttackCooldown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetAttackCooldown()
    {
        if (base.gameManagerScript.difficulty == GameManager.Difficulty.easy)
        {
            return baseAttackCooldown * RandomizeCooldownTime(2, 3);
        }
        else if (base.gameManagerScript.difficulty == GameManager.Difficulty.normal)
        {
            return baseAttackCooldown * RandomizeCooldownTime(0.5f, 1.5f);
        }
        else if (base.gameManagerScript.difficulty == GameManager.Difficulty.hard)
        {
            return baseAttackCooldown * RandomizeCooldownTime(0, 1);
        }

        return 0;
    }

    private float RandomizeCooldownTime(float minWindow, float maxWindow)
    {
        System.Random random = new System.Random();

        return (float)(random.NextDouble() * (maxWindow - minWindow) + minWindow);
    }
}
