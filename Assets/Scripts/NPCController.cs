using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public NPCState npcState;
    public float npcRotationSpeed;
    public bool startedNPCInteraction = false;
    public bool stoppedNPCInteraction = false;

    private Vector3 directionToPlayer;
    private GameObject player;
    private Quaternion npcRotationToPlayer;
    private GameManager gameManagerScript;
    private TreeList<string> dialogeBranches = new TreeList<string>();
    [SerializeField] private string[] dialogueBranchOptions;
    [SerializeField] public string[] branch0DialogueFragments;
    [SerializeField] private string[] branch1DialogueFragments;
    [SerializeField] private string[] branch2DialogueFragments;
    private Quaternion defaultRotation;

    public enum NPCState
    {
        WORKING,
        SPEAKING
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("FirstPersonController");
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();

        npcState = NPCState.WORKING;
        defaultRotation = transform.rotation;

        for (int i = 0; i < branch0DialogueFragments.Length; i++)
        {
            dialogeBranches[0].Values.Add(branch0DialogueFragments[i]);
            Debug.Log("fragment " + i + ": " + branch0DialogueFragments[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (npcState == NPCState.SPEAKING)
        {
            directionToPlayer = player.transform.position - (transform.position);
            directionToPlayer.y = 0;
            npcRotationToPlayer = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, npcRotationToPlayer, npcRotationSpeed * Time.deltaTime);

            if (!startedNPCInteraction)
            {
                startedNPCInteraction = true;
                stoppedNPCInteraction = false;
                gameManagerScript.dialogueParent.SetActive(true);
                OnNPCInteractEnter();
                gameManagerScript.OnDialogueOpen();
            }
            
        }
        else if (npcState == NPCState.WORKING)
        {
            if (!stoppedNPCInteraction) {
                stoppedNPCInteraction = true;
                startedNPCInteraction = false;
                StartCoroutine(RotateNPC(defaultRotation));
            }
        }
    }

    private IEnumerator RotateNPC(Quaternion endRotation)
    {
        while (transform.rotation != defaultRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, npcRotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnNPCInteractEnter()
    {
        gameManagerScript.currentInteractedNPC = gameObject;

        for (int i = 0; i < dialogueBranchOptions.Length; i++)
        {
            dialogeBranches.Add(new TreeList<string>());

            GameObject dialogueOption = Instantiate(gameManagerScript.dialogeChoiceButtonPrefab, GameObject.Find("Dialogue Choices").transform);
            dialogueOption.transform.localPosition = new Vector3(0, -135 - (i * 30), 0);
            dialogueOption.GetComponentInChildren<TextMeshProUGUI>().text = dialogueBranchOptions[i];
            dialogueOption.GetComponent<DialogueButtonController>().branchNum = i;

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
