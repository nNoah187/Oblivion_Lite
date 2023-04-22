using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    private BoxCollider attackTrigger;
    private EnemyStats enemyStats;
    private EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        attackTrigger = GetComponent<BoxCollider>();
        enemyStats = GetComponentInParent<EnemyStats>();
        enemyController = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enemyController.dealtDamageForThisAttack && other.gameObject.CompareTag("Player"))
        {
            enemyController.dealtDamageForThisAttack = true;
            StartCoroutine(resetAttack());
            enemyStats.DealDamange(other.gameObject);
        }
    }

    // Cooldown for enemy attack
    public IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(enemyStats.GetAttackCooldown());
        enemyController.dealtDamageForThisAttack = false;
    }
}