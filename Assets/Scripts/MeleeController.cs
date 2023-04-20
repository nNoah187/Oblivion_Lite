using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameManager gameManagerScript;

    public EnemyController enemyControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("FirstPersonController").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerControllerScript.isAttacking && other.gameObject.CompareTag("Enemy"))
        {
            enemyControllerScript = other.gameObject.GetComponent<EnemyController>();

            enemyControllerScript.enemyAnimator.SetTrigger("takeDamage");
            enemyControllerScript.health -= 10;
            enemyControllerScript.healthBar.value = enemyControllerScript.health;
        }
    }
}
