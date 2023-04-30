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

        SetWeaponAttackCooldown();
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

    public void SetWeaponAttackCooldown()
    {
        if (weaponType == WeaponType.SWORD)
        {
            weaponTypeDamageMultiplier = 0.5f;
            attackCooldown = 0.75f;
        }
        else if (weaponType == WeaponType.AXE)
        {
            weaponTypeDamageMultiplier = 1;
            attackCooldown = 1.5f;
        }
        else if (weaponType == WeaponType.BLUNT)
        {
            weaponTypeDamageMultiplier = 2;
            attackCooldown = 3;
        }
    }
}
