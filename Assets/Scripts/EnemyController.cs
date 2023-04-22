using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private float combatDistance = 10f;
    private GameObject player;
    private Vector3 directionToPlayer;
    private Quaternion enemyRotationToPlayer;
    private GameManager gameManagerScript;
    private EnemyStats enemyStats;
    private float currentWalkSpeed;

    public EnemyCombatState enemyCombatState;
    public Slider healthBar;
    public Animator enemyAnimator;
    public float maxWalkSpeed;
    public float minDistanceToPlayer;
    public float enemyRotationSpeed;
    public bool dealtDamageForThisAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyCombatState = EnemyCombatState.PASSIVE;
        player = GameObject.Find("FirstPersonController");
        enemyAnimator = GetComponent<Animator>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        enemyStats = GetComponent<EnemyStats>();

        // Spawn in a healthbar for each enemy that is spawned in
        healthBar = Instantiate(gameManagerScript.enemyHealthbarPrefab, new Vector3(transform.position.x, transform.position.y + 2,
            transform.position.z), Quaternion.identity);
        healthBar.gameObject.SetActive(false);
        healthBar.transform.parent = GameObject.Find("World Canvas").transform;

        // Start enemy attack coroutine
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy comabt state
        if (enemyCombatState == EnemyCombatState.COMBAT)
        {
            // Gradually increase walk speed while in combat state
            if (currentWalkSpeed < maxWalkSpeed)
            {
                currentWalkSpeed += 0.01f;
            }

            // Move the enemy forward if it is not too close to the player
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) > minDistanceToPlayer && gameManagerScript.enemyFollowPlayer)
            {
                transform.Translate(Vector3.forward * currentWalkSpeed * Time.deltaTime);
            }
            else
            {
                currentWalkSpeed = 0;
            }

            // Show the enemy healthbar if in combat state and continually ensure the healthbar is above the enemy
            healthBar.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            healthBar.gameObject.SetActive(true);

            // The enemy gradually turns toward the player if in combat state
            directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0;
            enemyRotationToPlayer = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, enemyRotationToPlayer, enemyRotationSpeed * Time.deltaTime);
        }
        // Enemy passive state
        else if (enemyCombatState == EnemyCombatState.PASSIVE)
        {
            // The enemy will not walk if in passive state
            currentWalkSpeed = 0;

            // The enemy healthbar will not show if in passive state
            healthBar.gameObject.SetActive(false);

            // The enemy attack animation will not play if in passive state
            enemyAnimator.SetBool("attack", false);
        }
        // Enemy dead state
        else if (enemyCombatState == EnemyCombatState.DEAD)
        {
            // Play enemy death animation and hide the enemy healthbar
            enemyAnimator.SetTrigger("die");
            healthBar.gameObject.SetActive(false);
        }

        // Trigger enemy dead state if enemy health is 0 or below
        if (enemyStats.currentHealth <= 0)
        {
            enemyCombatState = EnemyCombatState.DEAD;
        }
        // Trigger enemy combat state if the player is within the combat distance of the enemy and the enemy is not dead
        else if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= combatDistance)
        {
            enemyCombatState = EnemyCombatState.COMBAT;
        }
        // Triger enemy passive state if the enemy is not dead and the player is not within the combat distance of the enemy
        else
        {
            enemyCombatState = EnemyCombatState.PASSIVE;
        }

        // Set the enemy walking animation speed of the current moving speed of the enemy
        enemyAnimator.SetFloat("walkSpeed", currentWalkSpeed);
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            if (enemyCombatState == EnemyCombatState.COMBAT && !(Vector3.Distance(gameObject.transform.position, player.transform.position) > minDistanceToPlayer))
            {
                // Make the enemy attack animation play
                enemyAnimator.SetBool("attack", true);
                yield return new WaitForSeconds(0.5f);
                enemyAnimator.SetBool("attack", false);
                yield return new WaitForSeconds(enemyStats.GetAttackCooldown());
            }
            yield return null;
        }
    }

    public enum EnemyCombatState
    {
        PASSIVE,
        COMBAT,
        DEAD
    }
}
