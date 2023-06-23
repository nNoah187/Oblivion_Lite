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
        if (other.gameObject.CompareTag("Player") && !enemyController.dealtDamageForThisAttack && enemyController.enemyCombatState == EnemyController.EnemyCombatState.COMBAT
            && GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
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
