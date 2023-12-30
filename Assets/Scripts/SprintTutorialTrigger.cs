using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintTutorialTrigger : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider other)
    {
        gameManagerScript.ShowTutorial("-While walking forward, hold left shift to sprint\n-Your sprint stamina is shown by the white bar below\n-If your sprint stamina bar is grayed out, you have to wait for it to recharge before you can sprint again");
        Destroy(gameObject);
    }
}
