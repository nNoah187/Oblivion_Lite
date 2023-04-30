using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayManager : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private GameManager gameManagerScript;

    public Camera firstPersonCamera;
    public float maxInteractDistance;
    public float minInteractDistance;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the game state is during gameplay
        if (gameManagerScript.gameState == GameManager.GameState.GAMEPLAY)
        {
            // Cast a ray from the mouse position (which is the center of the screen)
            ray = firstPersonCamera.ScreenPointToRay(Input.mousePosition);

            // If the ray hits an object within the max distance
            if (Physics.Raycast(ray, out hit, maxInteractDistance))
            {
                // If not currently opening a chest and the ray hits a chest
                if (!gameManagerScript.openingChest && hit.collider.gameObject.CompareTag("Chest"))
                {
                    // Display the prompt to open the chest
                    gameManagerScript.openChestPrompt.gameObject.SetActive(true);

                    // If the player presses F
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        // If the chest being opened has never been opened before
                        if (hit.collider.gameObject.GetComponent<ChestController>().chestState == ChestController.ChestState.UNOPENED)
                        {
                            gameManagerScript.openingChest = true;
                            // Set the chest to opened state
                            hit.collider.gameObject.GetComponent<ChestController>().chestState = ChestController.ChestState.OPENED;
                            gameManagerScript.openChestPrompt.gameObject.SetActive(false);
                            // Play the chest opening animation
                            hit.collider.gameObject.GetComponent<Animator>().SetTrigger("open");
                            StartCoroutine(gameManagerScript.WaitForChestAnimation(hit.collider.gameObject));
                        }
                        // Or if the chest being opened has been opened before
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
