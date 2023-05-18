using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public NPCState npcState;
    public float npcRotationSpeed;

    private Vector3 directionToPlayer;
    private GameObject player;
    private Quaternion npcRotationToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("FirstPersonController");

        npcState = NPCState.WORKING;
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
        }
    }

    public enum NPCState
    {
        WORKING,
        SPEAKING
    }
}
