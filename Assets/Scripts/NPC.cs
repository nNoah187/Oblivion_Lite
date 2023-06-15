using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //[SerializeField] private int numDialogueBranches;
    private TreeList<string> dialogeBranches = new TreeList<string>();
    private GameManager gameManagerScript;
    [SerializeField] private string[] dialogueBranchOptions;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNPCInteractEnter()
    {
        gameManagerScript.currentInteractedNPC = gameObject;

        for (int i = 0; i < dialogueBranchOptions.Length; i++)
        {
            dialogeBranches.Add(new TreeList<string>());

            GameObject dialogueOption = Instantiate(gameManagerScript.dialogeChoiceButtonPrefab, GameObject.Find("Dialogue Parent").transform);
            dialogueOption.transform.localPosition = new Vector3(0, -135 - (i * 30), 0);
            dialogueOption.GetComponentInChildren<TextMeshProUGUI>().text = dialogueBranchOptions[i];

            for (int j = 0; j < 3; j++)
            {
                dialogeBranches[i].Values.Add("fragment " + j);
            }

        }

        //What's at branch 0's values?
        foreach (string value in dialogeBranches[0].Values)
        {
            Debug.Log(value);
        }
    }
}
