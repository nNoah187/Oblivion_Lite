using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private FirstPersonController firstPersonController;
    private GameManager gameManagerScript;
    private MeleeController meleeControllerScript;
    private bool startedBlinkingSprintBar = false;
    private PlayerStats playerStats;
    private WeaponStats weaponStats;

    public bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        firstPersonController = GetComponent<FirstPersonController>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        meleeControllerScript = GameObject.Find("Weapon").GetComponentInChildren<MeleeController>();
        playerStats = GetComponent<PlayerStats>();
        weaponStats = GetComponentInChildren<WeaponStats>();

        gameManagerScript.healthbar.maxValue = playerStats.maxHealth;
        gameManagerScript.healthbar.value = playerStats.maxHealth;
        gameManagerScript.attackCooldownBar.maxValue = weaponStats.attackCooldown;
        gameManagerScript.attackCooldownBar.value = weaponStats.attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManagerScript.cursorEnabled)
        {
            // Forward and horizontal movement
            playerAnimator.SetFloat("forwardSpeed", Input.GetAxis("Vertical"));
            playerAnimator.SetFloat("horizontalSpeed", Input.GetAxis("Horizontal"));

            // Swing sword
            if (Input.GetMouseButtonDown(0) && !isAttacking)
            {
                isAttacking = true;
                playerAnimator.SetTrigger("attack");
                StartCoroutine(meleeControllerScript.resetAttack());

                if (!meleeControllerScript.startedUpdatingAttackCooldownBar)
                {
                    StartCoroutine(meleeControllerScript.UpdateAttackCooldownBar());
                }
            }

            // Sprint
            if (Input.GetKey(KeyCode.LeftShift) && !firstPersonController.isSprintCooldown)
            {
                playerAnimator.SetBool("sprinting", true);
            }
            else
            {
                playerAnimator.SetBool("sprinting", false);
            }
        }

        gameManagerScript.sprintBar.value = firstPersonController.sprintRemaining;

        if (!startedBlinkingSprintBar && firstPersonController.isSprintCooldown)
        {
            firstPersonController.sprintRemaining = 0;
            StartCoroutine(BlinkBar(gameManagerScript.sprintBar.gameObject, firstPersonController.sprintCooldown));
        }
    }

    public IEnumerator BlinkBar(GameObject obj, float duration)
    {
        startedBlinkingSprintBar = true;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            obj.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            obj.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        startedBlinkingSprintBar = false;
    }
}
