using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private float combatDistance = 10f;
    private EnemyCombatState enemyCombatState;
    private GameObject player;
    private Vector3 directionToPlayer;
    private Quaternion enemyRotationToPlayer;
    private float enemyRotationSpeed = 1f;
    private GameManager gameManagerScript;

    public Slider healthBar;
    public float health = 100;
    public Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        enemyCombatState = EnemyCombatState.PASSIVE;
        player = GameObject.Find("FirstPersonController");
        enemyAnimator = GetComponent<Animator>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();

        healthBar = Instantiate(gameManagerScript.enemyHealthbarPrefab, new Vector3(transform.position.x, transform.position.y + 2,
            transform.position.z), Quaternion.identity);
        healthBar.gameObject.SetActive(false);
        healthBar.transform.parent = GameObject.Find("World Canvas").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            enemyCombatState = EnemyCombatState.DEAD;
        }
        else if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= combatDistance)
        {
            enemyCombatState = EnemyCombatState.COMBAT;
        }
        else
        {
            enemyCombatState = EnemyCombatState.PASSIVE;
        }

        if (enemyCombatState == EnemyCombatState.COMBAT)
        {
            healthBar.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            healthBar.gameObject.SetActive(true);

            enemyAnimator.SetBool("attack", true);

            directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0;
            enemyRotationToPlayer = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, enemyRotationToPlayer, enemyRotationSpeed * Time.deltaTime);
        }
        else if (enemyCombatState == EnemyCombatState.PASSIVE)
        {
            healthBar.gameObject.SetActive(false);

            enemyAnimator.SetBool("attack", false);
        }
        else if (enemyCombatState == EnemyCombatState.DEAD)
        {
            enemyAnimator.SetTrigger("die");
            healthBar.gameObject.SetActive(false);
        }
    }

    public enum EnemyCombatState
    {
        PASSIVE,
        COMBAT,
        DEAD
    }
}
