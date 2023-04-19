using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private float swordAttackCooldown = 0.75f;
    private FirstPersonController firstPersonController;
    private GameManager gameManagerScript;

    public bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        firstPersonController = GetComponent<FirstPersonController>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Forward and horizontal movement
        playerAnimator.SetFloat("forwardSpeed", Input.GetAxis("Vertical"));
        playerAnimator.SetFloat("horizontalSpeed", Input.GetAxis("Horizontal"));

        // Swing sword
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            playerAnimator.SetTrigger("attack");
            StartCoroutine(resetAttack());
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetBool("sprinting", true);
        }
        else
        {
            playerAnimator.SetBool("sprinting", false);
        }
    }

    // Cooldown for melee attack
    private IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(swordAttackCooldown);
        isAttacking = false;
    }
}
