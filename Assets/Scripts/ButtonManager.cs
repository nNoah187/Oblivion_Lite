using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private GameManager gameManagerScript;
    private GameObject player;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("FirstPersonController");
        playerStats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerResponseDialogue()
    {
        NPCController npcController = gameManagerScript.currentInteractedNPC.GetComponent<NPCController>();
        npcController.npcDialogueIndex++;
        npcController.npcDialogueIndexForThisSequence++;

        // NPC is out of dialogue
        if (!npcController.canSpeak || npcController.npcDialogeSequenceLineAmountsArray[npcController.npcDialogueSequece] <= npcController.npcDialogueIndexForThisSequence)
        {
            gameManagerScript.OnNPCInteractExit();
        }
        else
        {
            gameManagerScript.npcDialogueText.text = npcController.npcText[npcController.npcDialogueIndex];
            gameManagerScript.playerResponseButtonText.text = npcController.playerResponseText[npcController.npcDialogueIndex];

            if (npcController.npcDialogueIndex >= npcController.npcText.Length - 1)
            {
                npcController.canSpeak = false;
            }
        }
    }

    public void ExitMenu(GameObject uiPrompt)
    {
        uiPrompt.SetActive(false);
        gameManagerScript.OnMenuExit();
    }

    /*
     * Debug methods
     */
    public void TeleportPlayer(int locationIndex)
    {
        switch(locationIndex)
        {
            // Starting spawn
            case 0:
                player.transform.position = new Vector3(-303.1f, 0.2f, 319.11f);
                break;
            // Demo area
            case 1:
                player.transform.position = new Vector3(0, 0, 0);
                break;
            // Starting village
            case 2:
                player.transform.position = new Vector3(-441.44f, 1.66f, 169.4f);
                break;
        }
    }

    public void EnableEasyDifficulty()
    {
        PlayerPrefs.SetInt("difficulty", 0);
        gameManagerScript.difficulty = GameManager.Difficulty.easy;
        gameManagerScript.difficultyText.text = "Difficulty: easy";
    }

    public void EnableNormalDifficulty()
    {
        PlayerPrefs.SetInt("difficulty", 1);
        gameManagerScript.difficulty = GameManager.Difficulty.normal;
        gameManagerScript.difficultyText.text = "Difficulty: normal";
    }

    public void EnableHardDifficulty()
    {
        PlayerPrefs.SetInt("difficulty", 2);
        gameManagerScript.difficulty = GameManager.Difficulty.hard;
        gameManagerScript.difficultyText.text = "Difficulty: hard";
    }

    public void IncreasePlayerLevel()
    {
        playerStats.level.SetValue(playerStats.level.GetValue() + 1);
    }

    public void DecreasePlayerLevel()
    {
        playerStats.level.SetValue(playerStats.level.GetValue() - 1);
    }

    public void AddXP(float amount)
    {
        playerStats.AddXP(amount);
    }

    public void SetPlayerLevel(int level)
    {
        playerStats.level.SetValue(level);
    }
}
