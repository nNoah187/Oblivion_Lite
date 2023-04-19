using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject bear;
    public int enemyHealth = 100;
    public TextMeshProUGUI enemyHealthText;
    public Slider enemyHealthSlider;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealthSlider.gameObject.transform.position = new Vector3(bear.transform.position.x, 
            bear.transform.position.y + 2, bear.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
