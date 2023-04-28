using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth {  get; private set; }   // Allows other class to access but not change
    public Stat damage;
    public Stat level;
    //public Stat armor;

    private float damageDealt;

    protected GameManager gameManagerScript;

    private void Awake()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }

    public void DealDamange(GameObject other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameManagerScript.meleeControllerScript.thisAttackRegistered = true;
            // Damage is determined by the stats of the weapon, the character's damage value, and the difficulty
            damageDealt = gameManagerScript.currentWeaponStats.damage.GetValue() * damage.GetValue() * gameManagerScript.GetDifficultyValueMultiplier();
            EnemyController enemyController = other.GetComponent<EnemyController>();
            // Play take damage animation on the character hit
            enemyController.enemyAnimator.SetTrigger("takeDamage");
            // Take damage from hit character's health
            other.GetComponent<CharacterStats>().currentHealth -= damageDealt;
            // Update hit character's healthbar
            enemyController.healthBar.value = other.GetComponent<CharacterStats>().currentHealth;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            damageDealt = damage.GetValue() / gameManagerScript.GetTotalArmorOutput();
            UnityEngine.Debug.Log("Damage dealt: " +  damageDealt);
            PlayerController playerController = other.GetComponent<PlayerController>();
            // Play take damage animation on the character hit
            playerController.playerAnimator.SetTrigger("takeDamage");
            // Take damage from hit character's health
            other.GetComponent<CharacterStats>().currentHealth -= damageDealt;
            // Update hit character's healthbar
            gameManagerScript.healthbar.value = other.GetComponent<CharacterStats>().currentHealth;
        }
        

        // Call Die method if hit character's health is 0 or below
        if (other.GetComponent<CharacterStats>().currentHealth <= 0)
        {
            other.GetComponent<CharacterStats>().Die();
        }
    }

    // Can be overwritten in subclasses
    public virtual void Die()
    {
        UnityEngine.Debug.Log(transform.name + " died");
    }

    // Adjusts damage dealt to enemies depending on difficulty
    //public float GetDifficultyDamageModifier()
    //{
    //    if (gameManagerScript.difficulty == GameManager.Difficulty.easy)
    //    {
    //        return 1.5f;
    //    }
    //    else if (gameManagerScript.difficulty == GameManager.Difficulty.normal)
    //    {
    //        return 1;
    //    }
    //    else if (gameManagerScript.difficulty == GameManager.Difficulty.hard)
    //    {
    //        return 0.75f;
    //    }

    //    return 0;
    //}
}
