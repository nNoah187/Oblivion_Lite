using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private float swordAttackCooldown = 0.5f;

    public bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetFloat("forwardSpeed", Input.GetAxis("Vertical"));
        playerAnimator.SetFloat("horizontalSpeed", Input.GetAxis("Horizontal"));

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            playerAnimator.SetTrigger("attack");
            StartCoroutine(resetAttack());
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetBool("sprinting", true);
        }
        else
        {
            playerAnimator.SetBool("sprinting", false);
        }
    }

    private IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(swordAttackCooldown);
        isAttacking = false;
    }
}
