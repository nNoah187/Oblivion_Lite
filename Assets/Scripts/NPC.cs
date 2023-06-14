using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private int numDialogueBranches;
    public TreeList<string> dialogeBranches = new TreeList<string>();
    private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();

        for (int i = 0; i < numDialogueBranches; i++) {
            dialogeBranches.Add(new TreeList<string>());

            GameObject dialogueOption = Instantiate(gameManagerScript.dialogeChoiceButtonPrefab, GameObject.Find("Canvas").transform);
            dialogueOption.transform.position = new Vector3(gameManagerScript.dialogeChoiceButtonPrefab.transform.position.x, gameManagerScript.dialogeChoiceButtonPrefab.transform.position.y - (i * 2), gameManagerScript.dialogeChoiceButtonPrefab.transform.position.z);

            for (int j = 0; j < 3; j++) {
                dialogeBranches[i].Values.Add("fragment " + j);
            }
            
        }

        // What's at branch 0's values?
        foreach (string value in dialogeBranches[0].Values) {
            Debug.Log(value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
