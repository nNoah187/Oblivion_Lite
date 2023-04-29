using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : GearStats
{
    public float attackCooldown;
    public Stat damage;
    public WeaponType weaponType;
    public float weaponTypeDamageMultiplier;

    private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (weaponType == WeaponType.SWORD)
        {
            weaponTypeDamageMultiplier = gameManagerScript.weaponTypeDamageMultiplierArray[0];
            attackCooldown = gameManagerScript.weaponAttackCooldownArray[0];
        }
        else if (weaponType == WeaponType.AXE)
        {
            weaponTypeDamageMultiplier = gameManagerScript.weaponTypeDamageMultiplierArray[1];
            attackCooldown = gameManagerScript.weaponAttackCooldownArray[1];
        }
        else if (weaponType == WeaponType.BLUNT)
        {
            weaponTypeDamageMultiplier = gameManagerScript.weaponTypeDamageMultiplierArray[2];
            attackCooldown = gameManagerScript.weaponAttackCooldownArray[2];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum WeaponType
    {
        SWORD,
        AXE,
        BLUNT
    }
}
