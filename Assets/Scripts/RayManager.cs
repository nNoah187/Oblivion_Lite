using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayManager : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private GameManager gameManagerScript;
    private PlayerController playerControllerScript;

    public Camera firstPersonCamera;
    public float maxWeaponPickupDistance;
    public float minWeaponPickupDistance;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerControllerScript = GameObject.Find("FirstPersonController").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = firstPersonCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxWeaponPickupDistance))
        {
            if (hit.collider.gameObject.CompareTag("Weapon") && hit.distance > minWeaponPickupDistance)
            {
                gameManagerScript.pickupWeaponPrompt.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    playerControllerScript.playerAnimator.SetTrigger("pickup");

                    Vector3 weaponGroundPosition = hit.collider.transform.position;
                    Quaternion weaponGroundRotation = hit.collider.gameObject.transform.rotation;
                    GameObject.Find("Weapon").transform.DetachChildren();
                    gameManagerScript.currentWeapon.transform.position = weaponGroundPosition;
                    gameManagerScript.transform.rotation = weaponGroundRotation;
                    gameManagerScript.currentWeapon = hit.collider.gameObject;
                    gameManagerScript.currentWeapon.transform.parent = GameObject.Find("Weapon").transform;
                    gameManagerScript.currentWeapon.transform.localPosition = new Vector3(0, -0.403f, -0.552f);
                    gameManagerScript.currentWeapon.transform.localRotation = Quaternion.Euler(new Vector3(-235.347f, 171.696f, -5.524994f));
                }
            }
        }
        else
        {
            gameManagerScript.pickupWeaponPrompt.gameObject.SetActive(false);
        }
    }
}
