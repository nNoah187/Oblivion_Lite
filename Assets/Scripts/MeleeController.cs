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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerControllerScript.isAttacking && other.gameObject.CompareTag("Enemy"))
        {
            playerStats.DealDamange(other.gameObject);
        }
    }

    public IEnumerator UpdateAttackCooldownBar()
    {
        startedUpdatingAttackCooldownBar = true;
        float startTime = Time.time;
        float finishTime = startTime + weaponStats.attackCooldown;

        while (Time.time < finishTime)
        {
            gameManagerScript.attackCooldownBar.value = Time.time - startTime;
            yield return null;
        }

        startedUpdatingAttackCooldownBar = false;
    }
}
