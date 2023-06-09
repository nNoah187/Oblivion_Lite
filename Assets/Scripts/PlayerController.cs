using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManagerScript;
    private MeleeController meleeControllerScript;
    private PlayerStats playerStats;
    private float maxSprintCooldown = 5f;

    public FirstPersonController firstPersonController;
    public Animator playerAnimator;
    public bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        firstPersonController = GetComponent<FirstPersonController>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        meleeControllerScript = GameObject.Find("Weapon").GetComponentInChildren<MeleeController>();
        playerStats = GetComponent<PlayerStats>();

        gameManagerScript.healthbar.maxValue = playerStats.maxHealth;
        gameManagerScript.healthbar.value = playerStats.maxHealth;
        gameManagerScript.attackCooldownBar.maxValue = gameManagerScript.GetWeaponAttackCooldown(playerStats.currentWeapon);
        gameManagerScript.attackCooldownBar.value = gameManagerScript.attackCooldownBar.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        // If the cursor is not enabled (in gameplay)
        if (gameManagerScript.gameState == GameManager.GameState.GAMEPLAY && !gameManagerScript.cursorEnabled)
        {
            // Forward and horizontal movement
            playerAnimator.SetFloat("forwardSpeed", Input.GetAxis("Vertical"));
            playerAnimator.SetFloat("horizontalSpeed", Input.GetAxis("Horizontal") * 0.75f);

            // Swing weapon if LMB is clicked and the player is not already attacking
            if (Input.GetMouseButtonDown(0) && !isAttacking)
            {
                isAttacking = true;
                firstPersonController.enableSprint = false;
                // The sprint cooldown should last for however much sprint was used to be recharged
                firstPersonController.sprintCooldown = maxSprintCooldown - firstPersonController.sprintRemaining;
                // Stop playing player sprint animation
                playerAnimator.SetBool("sprinting", false);
                firstPersonController.isSprintCooldown = true;
                // Play player attack animation
                playerAnimator.SetTrigger("attack");
                // Start the weapon attack cooldown
                StartCoroutine(meleeControllerScript.resetAttack());

                // Update the attack cooldown bar on the HUD if it hasn't started already
                if (!meleeControllerScript.startedUpdatingAttackCooldownBar)
                {
                    StartCoroutine(meleeControllerScript.UpdateAttackCooldownBar());
                }
            }

            // Sprint if left shift is pressed and the player is not in a sprint cooldown
            if (Input.GetKey(KeyCode.LeftShift) && !firstPersonController.isSprintCooldown)
            {
                // Play player sprinting animation
                playerAnimator.SetBool("sprinting", true);
            }
            else
            {
                // The sprint cooldown should last for however much sprint was used to be recharged
                firstPersonController.sprintCooldown = maxSprintCooldown - firstPersonController.sprintRemaining;
                // Stop playing player sprint animation
                playerAnimator.SetBool("sprinting", false);
                if (firstPersonController.sprintCooldown == 0)
                {
                    firstPersonController.isSprintCooldown = false;
                }
                else
                {
                    firstPersonController.isSprintCooldown = true;
                }
            }

            // Continually update the sprint bar on the HUD with the amount of sprint remaining before cooldown
            gameManagerScript.sprintBar.value = firstPersonController.sprintRemaining;

            // Play player death animation if player health is 0 or below
            if (playerStats.currentHealth <= 0)
            {
                playerAnimator.SetTrigger("die");
            }
        }
    }
}
