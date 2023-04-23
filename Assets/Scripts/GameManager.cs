using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject bear;
    public int enemyHealth = 100;
    public GameObject bearEnemyPrefab;
    public Slider enemyHealthbarPrefab;
    public Slider sprintBar;
    public Slider attackCooldownBar;
    public Slider healthbar;
    public Image sprintBarFill;
    public Image sprintBarBackground;
    public TextMeshProUGUI pickupWeaponPrompt;
    public GameObject currentWeapon;
    public GameObject cutlassPrefab;
    public GameObject weaponPosition;
    public MeleeController meleeControllerScript;
    public WeaponStats currentWeaponStats;

    private FirstPersonController firstPersonController;

    // Debug components
    public GameObject debugMenu;
    public TextMeshProUGUI difficultyText;
    private bool debugEnabled = true;
    public Difficulty difficulty;
    public bool cursorEnabled = false;
    public bool enemyFollowPlayer;

    public enum Difficulty
    {
        easy,
        normal,
        hard
    }

    // Start is called before the first frame update
    void Start()
    {
        firstPersonController = GameObject.Find("FirstPersonController").GetComponent<FirstPersonController>();

        // Set sprint bar max value to the sprint duration from the first person controller
        sprintBar.maxValue = firstPersonController.sprintDuration;

        // Spawn in 2 bears
        Instantiate(bearEnemyPrefab, new Vector3(5, 0, 5), Quaternion.identity);
        Instantiate(bearEnemyPrefab, new Vector3(-5, 0, -5), Quaternion.identity);

        currentWeapon = Instantiate(cutlassPrefab);
        currentWeapon.transform.parent = GameObject.Find("Weapon").transform;
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        meleeControllerScript = currentWeapon.GetComponent<MeleeController>();
        currentWeaponStats = currentWeapon.GetComponent<WeaponStats>();

        // Show the debug menu
        debugMenu.SetActive(true);
        // Set the difficulty to normal
        difficulty = Difficulty.normal;
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle debug menu when T key is pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (debugEnabled)
            {
                debugMenu.SetActive(false);
                debugEnabled = false;
            }
            else
            {
                debugMenu.SetActive(true);
                debugEnabled = true;
            }
        }

        // Show cursor and allow cursor movement when left alt is held down
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            cursorEnabled = true;
            firstPersonController.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            cursorEnabled = false;
            firstPersonController.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
