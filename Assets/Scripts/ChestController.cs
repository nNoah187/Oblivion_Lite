using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public ChestState chestState;
    public GameObject gearContained;

    // Start is called before the first frame update
    void Start()
    {
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
