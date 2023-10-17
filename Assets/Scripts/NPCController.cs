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

    // Reset when NPC gets new dialogue
    public string[] npcText;
    public string[] playerResponseText;
    public int npcDialogueIndex = 0;
    public bool canSpeak = false;
    public bool playerDiscoveredIfNpcCanSpeak = false;

    private Vector3 directionToPlayer;
    private GameObject player;
    private Quaternion npcRotationToPlayer;
    private GameManager gameManagerScript;
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

        if (npcDialogueIndex == 0)
        {
            canSpeak = true;
        }

        npcState = NPCState.WORKING;
        defaultRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (npcState == NPCState.SPEAKING && canSpeak)
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
        gameManagerScript.npcDialogueText.text = npcText[npcDialogueIndex];
        gameManagerScript.playerResponseButtonText.text = playerResponseText[npcDialogueIndex];
    }
}
