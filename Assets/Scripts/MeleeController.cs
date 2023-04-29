using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameManager gameManagerScript;
    private WeaponStats weaponStats;
    private EnemyController enemyControllerScript;
    private PlayerStats playerStats;

    public bool thisAttackRegistered = false;
    public bool startedUpdatingAttackCooldownBar = false;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("FirstPersonController").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        weaponStats = GetComponent<WeaponStats>();
        playerStats = GameObject.Find("FirstPersonController").GetComponent <PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Cooldown for melee attack
    public IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(weaponStats.attackCooldown);
        playerControllerScript.isAttacking = false;
        playerControllerScript.firstPersonController.enableSprint = true;
        thisAttackRegistered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Call DealDamage method from PlayerStats if the player is attacking and the weapon enters the trigger of an enemy
        if (playerControllerScript.isAttacking && other.gameObject.CompareTag("Enemy") && !thisAttackRegistered)
        {
            playerStats.DealDamange(other.gameObject);
        }
    }

    // Show the attack cooldown length on the attack cooldown bar on the HUD
    public IEnumerator UpdateAttackCooldownBar()
    {
        startedUpdatingAttackCooldownBar = true;
        float startTime = Time.time;
        float finishTime = startTime + playerStats.currentWeapon.GetComponent<WeaponStats>().attackCooldown;

        // Continually increase the attack cooldown bar while the attack is still in cooldown
        while (Time.time < finishTime)
        {
            gameManagerScript.attackCooldownBar.value = Time.time - startTime;
            yield return null;
        }

        startedUpdatingAttackCooldownBar = false;
    }
}
