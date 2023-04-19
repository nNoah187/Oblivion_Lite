using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameManager gameManagerScript;
    private Animator bearAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("FirstPersonController").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        bearAnimator = gameManagerScript.bear.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerControllerScript.isAttacking && playerControllerScript.canAttack && other.gameObject.CompareTag("Enemy"))
        {
            bearAnimator.SetTrigger("takeDamage");
            gameManagerScript.enemyHealth -= 10;
            gameManagerScript.enemyHealthText.text = gameManagerScript.enemyHealth.ToString();
            gameManagerScript.enemyHealthSlider.value = gameManagerScript.enemyHealth;
        }
    }
}
