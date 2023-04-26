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
    public float maxInteractDistance;
    public float minInteractDistance;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerControllerScript = GameObject.Find("FirstPersonController").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.gameState == GameManager.GameState.GAMEPLAY)
        {
            ray = firstPersonCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, maxInteractDistance))
            {
                if (!gameManagerScript.openingChest && hit.collider.gameObject.CompareTag("Chest"))
                {
                    gameManagerScript.openChestPrompt.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (hit.collider.gameObject.GetComponent<ChestController>().chestState == ChestController.ChestState.UNOPENED)
                        {
                            gameManagerScript.openingChest = true;
                            hit.collider.gameObject.GetComponent<ChestController>().chestState = ChestController.ChestState.OPENED;
                            gameManagerScript.openChestPrompt.gameObject.SetActive(false);
                            hit.collider.gameObject.GetComponent<Animator>().SetTrigger("open");
                            StartCoroutine(gameManagerScript.WaitForChestAnimation());
                        }
                        else if (hit.collider.gameObject.GetComponent<ChestController>().chestState == ChestController.ChestState.OPENED)
                        {
                            gameManagerScript.OpenChest(hit.collider.gameObject);
                        }
                    }
                }
                else
                {
                    gameManagerScript.openChestPrompt.gameObject.SetActive(false);
                }
            }
            else
            {
                gameManagerScript.openChestPrompt.gameObject.SetActive(false);
            }
        }
        else
        {
            gameManagerScript.openChestPrompt.gameObject.SetActive(false);
        }
    }
}
