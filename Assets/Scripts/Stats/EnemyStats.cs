using NHance.Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public Stat damage;
    public float baseAttackCooldown;
    public float baseXPValue;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("FirstPersonController");
    }

    // Update is called once per frame
    void Update()
    {
        level = player.GetComponent<PlayerStats>().level;
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

    public override void Die()
    {
        player.GetComponent<PlayerStats>().AddXP(GetXPAmount());
        GetComponent<EnemyController>().physicalCollider.enabled = false;
        GetComponent<EnemyController>().deathCollider.enabled = true;
    }

    public float GetTotalDamageOutput()
    {
        // Increase this value to punish the player more for wearing under-leveled armor
        float lowerArmorLevelPunishmentMultiplier = 50;
        return damage.GetValue() * level.GetValue() * Mathf.Pow(gameManagerScript.GetArmorToPlayerLevelMultiplier(), lowerArmorLevelPunishmentMultiplier)
            / gameManagerScript.GetTotalArmorOutput() * 10;
    }

    public float GetTotalArmorOutput()
    {
        return level.GetValue();
    }

    public float GetXPAmount()
    {
        return baseXPValue * level.GetValue() * 10;
    }
}
