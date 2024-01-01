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
    public GameObject otherCellDoor;
    public GameObject otherPrisonExitDoor;

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
                    gameManagerScript.interactPrompt.gameObject.SetActive(true);
                    gameManagerScript.interactPrompt.text = "Press F to open chest";

                    // If the player presses F
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        // If the chest being opened has never been opened before
                        if (hit.collider.gameObject.GetComponent<ChestController>().chestState == ChestController.ChestState.UNOPENED)
                        {
                            gameManagerScript.openingChest = true;
                            // Set the chest to opened state
                            hit.collider.gameObject.GetComponent<ChestController>().chestState = ChestController.ChestState.OPENED;
                            gameManagerScript.interactPrompt.gameObject.SetActive(false);
                            // Play the chest opening animation
                            hit.collider.gameObject.GetComponent<Animator>().SetTrigger("open");
                            StartCoroutine(gameManagerScript.WaitForChestAnimation(hit.collider.gameObject));
                        }
                        // Or if the chest being opened has been opened before
                        else if (hit.collider.gameObject.GetComponent<ChestController>().chestState == ChestController.ChestState.OPENED)
                        {
                            if (gameManagerScript.questIndex == 0 && gameManagerScript.questObjectiveIndex >= 7)
                            {
                                gameManagerScript.ShowNotification("This chest is empty");
                            }
                            else
                            {
                                gameManagerScript.OpenChest(hit.collider.gameObject);
                            }
                        }

                        if (gameManagerScript.questIndex == 0 && gameManagerScript.questObjectiveIndex == 5)
                        {
                            gameManagerScript.OnQuestObjectiveCompletion("Equip the axe in the chest");
                            gameManagerScript.ShowTutorial("-Chests give you random loot scaled for your level\n-Finding and equipping better loot will make combat easier");
                        }
                    }
                }
                else if (hit.collider.gameObject.CompareTag("NPC") && hit.collider.gameObject.GetComponent<NPCController>().npcState == NPCController.NPCState.WORKING)
                {
                    NPCController npcController = hit.collider.GetComponent<NPCController>();
                    gameManagerScript.interactPrompt.gameObject.SetActive(true);

                    if (!npcController.playerDiscoveredIfNpcCanSpeak || npcController.canSpeak)
                    {
                        gameManagerScript.interactPrompt.text = "Press F to speak";
                    }
                    else
                    {
                        gameManagerScript.interactPrompt.text = npcController.name + " doesn't want to speak right now.";
                    }
                    
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        npcController.playerDiscoveredIfNpcCanSpeak = true;

                        if (npcController.canSpeak)
                        {
                            gameManagerScript.interactPrompt.gameObject.SetActive(false);
                            hit.collider.gameObject.GetComponent<Animator>().SetBool("speaking", true);
                            hit.collider.gameObject.GetComponent<NPCController>().npcState = NPCController.NPCState.SPEAKING;
                        }
                    }
                }
                else if (hit.collider.gameObject.CompareTag("Key") && gameManagerScript.questIndex == 0 && gameManagerScript.questObjectiveIndex == 1)
                {
                    gameManagerScript.interactPrompt.gameObject.SetActive(true);
                    gameManagerScript.interactPrompt.text = "Press F to steal key";

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        gameManagerScript.OnQuestObjectiveCompletion("Wait for Grognak to leave");
                        Destroy(hit.collider.gameObject);
                        Destroy(gameManagerScript.tutorialKey);
                        StartCoroutine(gameManagerScript.ShowNotificationAfterTime(3, "Grognak's key added"));
                        StartCoroutine(gameManagerScript.GrognakWalkAfterTime(3));
                    }
                }
                else if (hit.collider.gameObject.CompareTag("Cell Door") && gameManagerScript.questIndex == 0 && gameManagerScript.questObjectiveIndex == 3)
                {
                    gameManagerScript.interactPrompt.gameObject.SetActive(true);
                    gameManagerScript.interactPrompt.text = "Press F to unlock door";

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (hit.collider.gameObject.name != "Correct Prison Door")
                        {
                            gameManagerScript.ShowNotification("This key doesn't fit this door");
                        }
                        else
                        {
                            Destroy(hit.collider.gameObject);
                            otherCellDoor.gameObject.SetActive(true);
                            gameManagerScript.OnQuestObjectiveCompletion("Speak to the prisoner");
                            //GameObject.Find("Ravi").GetComponent<NPCController>().playerDiscoveredIfNpcCanSpeak = false;
                            GameObject.Find("Ravi").GetComponent<NPCController>().canSpeak = true;
                        }
                    }
                }
                else if (hit.collider.gameObject.CompareTag("Exit Door") && gameManagerScript.questIndex == 0 && gameManagerScript.questObjectiveIndex == 7)
                {
                    gameManagerScript.interactPrompt.gameObject.SetActive(true);
                    gameManagerScript.interactPrompt.text = "Press F to open door";

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        Camera.main.farClipPlane = 200;
                        Destroy(hit.collider.gameObject);
                        otherPrisonExitDoor.gameObject.SetActive(true);
                        gameManagerScript.OnQuestObjectiveCompletion("Wait for Ravi to meet you outside the prison");
                    }
                }
                else
                {
                    gameManagerScript.interactPrompt.gameObject.SetActive(false);
                }
            }
            else
            {
                gameManagerScript.interactPrompt.gameObject.SetActive(false);
            }
        }
        else
        {
            gameManagerScript.interactPrompt.gameObject.SetActive(false);
        }
    }
}
