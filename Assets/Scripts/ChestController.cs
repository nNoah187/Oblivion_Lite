using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private GameManager gameManagerScript;

    public ChestState chestState;
    public GameObject gearContained;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        chestState = ChestState.UNOPENED;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum ChestState
    {
        UNOPENED,
        OPENED
    }
}
