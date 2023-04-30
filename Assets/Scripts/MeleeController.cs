using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameManager gameManagerScript;
    private PlayerStats playerStats;

    public bool thisAttackRegistered = false;
    public bool startedUpdatingAttackCooldownBar = false;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("FirstPersonController").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerStats = GameObject.Find("FirstPersonController").GetComponent <PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.attackCooldownBar.maxValue - gameManagerScript.attackCooldownBar.value <= 0.1)
        {
            thisAttackRegistered = false;
        }
    }

    // Cooldown for melee attack
    public IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(gameManagerScript.GetWeaponAttackCooldown(playerStats.currentWeapon));
        playerControllerScript.isAttacking = false;
        playerControllerScript.firstPersonController.enableSprint = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Call DealDamage method from PlayerStats if the player is attacking and the weapon enters the trigger of an enemy
        if (playerControllerScript.isAttacking && other.gameObject.CompareTag("Enemy") && !thisAttackRegistered)
        {
            playerStats.DealDamange(other.gameObject);
            thisAttackRegistered = true;
        }
    }

    // Show the attack cooldown length on the attack cooldown bar on the HUD
    public IEnumerator UpdateAttackCooldownBar()
    {
        startedUpdatingAttackCooldownBar = true;
        float startTime = Time.time;
        float finishTime = startTime + gameManagerScript.GetWeaponAttackCooldown(playerStats.currentWeapon);

        // Continually increase the attack cooldown bar while the attack is still in cooldown
        while (Time.time < finishTime)
        {
            gameManagerScript.attackCooldownBar.value = Time.time - startTime;
            yield return null;
        }

        startedUpdatingAttackCooldownBar = false;
    }
}
