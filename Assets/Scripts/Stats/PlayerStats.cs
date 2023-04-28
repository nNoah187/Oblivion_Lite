using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : CharacterStats
{
    public GameObject currentHelmet;
    public GameObject currentChest;

    // Start is called before the first frame update
    void Start()
    {
        currentHelmet = GameObject.Find("Helmet").GetComponentInChildren<ArmorStats>().gameObject;
        currentChest = GameObject.Find("Chest").GetComponentInChildren<ArmorStats>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
