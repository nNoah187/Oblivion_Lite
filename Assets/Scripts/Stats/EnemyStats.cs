using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private EnemyController enemyControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyControllerScript = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
