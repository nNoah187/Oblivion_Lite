using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueButtonController : MonoBehaviour
{
    public int branchNum;

    private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDialogueOptionSelection()
    {
        GameObject.Find("Dialogue Choices").SetActive(false);
        TextMeshProUGUI dialogueText = GameObject.Find("NPC Dialogue").GetComponent<TextMeshProUGUI>();
        dialogueText.gameObject.SetActive(true);
        gameManagerScript.nextDialogueFragmentButton.SetActive(true);
        dialogueText.text = gameManagerScript.currentInteractedNPC.GetComponent<NPCController>().branch0DialogueFragments[0];
    }
}
