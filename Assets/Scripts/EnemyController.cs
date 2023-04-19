using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float combatDistance = 5;
    private EnemyCombatState enemyCombatState;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemyCombatState = EnemyCombatState.PASSIVE;
        player = GameObject.Find("FirstPersonController");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= combatDistance)
        {
            enemyCombatState = EnemyCombatState.COMBAT;
            transform.LookAt(player.transform.position);
        }
    }

    public enum EnemyCombatState
    {
        PASSIVE,
        COMBAT
    }
}
