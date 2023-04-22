using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth {  get; private set; }
    public Stat damage;
    //public Stat armor;

    private WeaponStats weaponStats;
    private float damageDealt;
    private GameManager gameManagerScript;

    private void Awake()
    {
        weaponStats = GameObject.Find("Weapon").GetComponentInChildren<WeaponStats>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    TakeDamage(10);
        //}
    }

    //public void TakeDamage()
    //{
    //    //damage -= armor.GetValue();

    //    float damage;
    //    damage = 
    //    damage = Mathf.Clamp(damage, 0, int.MaxValue);

    //    currentHealth -= damage;
    //    Debug.Log("take damage");
    //    if (currentHealth <= 0)
    //    {
    //        Debug.Log("bruh 1");
    //        Die();
    //    }
    //}

    public void DealDamange(GameObject other)
    {
        damageDealt = weaponStats.damage.GetValue() * damage.GetValue() * GetDifficultyModifier();

        EnemyController enemyController = other.GetComponent<EnemyController>();

        enemyController.enemyAnimator.SetTrigger("takeDamage");
        //enemyControllerScript.health -= damageDealt;
        other.GetComponent<CharacterStats>().currentHealth -= damageDealt;
        enemyController.healthBar.value = other.GetComponent<CharacterStats>().currentHealth;

        if (other.GetComponent<CharacterStats>().currentHealth <= 0)
        {
            other.GetComponent<CharacterStats>().Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log(transform.name + " died");
    }

    public float GetDifficultyModifier()
    {
        if (gameManagerScript.difficulty == GameManager.Difficulty.easy)
        {
            return 1.5f;
        }
        else if (gameManagerScript.difficulty == GameManager.Difficulty.normal)
        {
            return 1;
        }
        else if (gameManagerScript.difficulty == GameManager.Difficulty.hard)
        {
            return 0.75f;
        }

        return 0;
    }
}
