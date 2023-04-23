using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayManager : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private GameManager gameManagerScript;
    private PlayerController playerControllerScript;

    public Camera firstPersonCamera;
    public float maxWeaponPickupDistance;
    public float minWeaponPickupDistance;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerControllerScript = GameObject.Find("FirstPersonController").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = firstPersonCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxWeaponPickupDistance))
        {
            if (hit.collider.gameObject.CompareTag("Chest"))
            {
                gameManagerScript.openChestPrompt.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    //playerControllerScript.playerAnimator.SetTrigger("pickup");
                    gameManagerScript.chest.GetComponent<Animator>().SetTrigger("open");
                }
            }
        }
        else
        {
            gameManagerScript.openChestPrompt.gameObject.SetActive(false);
        }
    }
}
