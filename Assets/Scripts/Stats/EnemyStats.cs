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
            return baseAttackCooldown * 2.5f;
        }
        else if (base.gameManagerScript.difficulty == GameManager.Difficulty.normal)
        {
            return baseAttackCooldown * 1;
        }
        else if (base.gameManagerScript.difficulty == GameManager.Difficulty.hard)
        {
            return baseAttackCooldown * 0.5f;
        }

        return 0;
    }
}
