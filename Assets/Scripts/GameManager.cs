using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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
    public TextMeshProUGUI openChestPrompt;
    public GameObject currentWeapon;
    public GameObject cutlassPrefab;
    public GameObject weaponPosition;
    public MeleeController meleeControllerScript;
    public WeaponStats currentWeaponStats;
    public GameObject gearAcquiredPrompt;
    public GameObject chestPrefab;
    public GameState gameState;
    public bool openingChest = false;
    public GameObject knightHelmetPrefab;
    public Image gearImage;
    public TextMeshProUGUI gearNameText;
    public TextMeshProUGUI valueNewStatText;
    public TextMeshProUGUI valueOldStatText;
    public TextMeshProUGUI levelNewStatText;
    public TextMeshProUGUI levelOldStatText;
    public bool wearingDefaultArmor = true;
    public GameObject[] gearPrefabArray;

    private FirstPersonController firstPersonController;
    private PlayerStats playerStats;

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

    public enum GameState
    {
        GAMEPLAY,
        MENU
    }

    // Start is called before the first frame update
    void Start()
    {
        firstPersonController = GameObject.Find("FirstPersonController").GetComponent<FirstPersonController>();
        playerStats = firstPersonController.GetComponent<PlayerStats>();

        // Set sprint bar max value to the sprint duration from the first person controller
        sprintBar.maxValue = firstPersonController.sprintDuration;

        // Spawn in 2 bears
        Instantiate(bearEnemyPrefab, new Vector3(5, 0, 5), Quaternion.identity);
        Instantiate(bearEnemyPrefab, new Vector3(-5, 0, -5), Quaternion.identity);

        Instantiate(chestPrefab, new Vector3(0, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-5, 0, 5), chestPrefab.transform.rotation);

        currentWeapon = Instantiate(cutlassPrefab);
        currentWeapon.transform.parent = GameObject.Find("Weapon").transform;
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        meleeControllerScript = currentWeapon.GetComponent<MeleeController>();
        currentWeaponStats = currentWeapon.GetComponent<WeaponStats>();

        gearAcquiredPrompt.SetActive(false);
        gameState = GameState.GAMEPLAY;

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

        // In gameplay
        if (gameState == GameState.GAMEPLAY)
        {
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

    public void OnMenuOpen()
    {
        Time.timeScale = 0;
        gameState = GameState.MENU;
        firstPersonController.enabled = false;
        cursorEnabled = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnMenuExit()
    {
        Time.timeScale = 1;
        gameState = GameState.GAMEPLAY;
        firstPersonController.enabled = true;
        cursorEnabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /*
     * Bug: chest won't keep stats of weapon when reopening
     * 
     */
    public void OpenChest(GameObject chest)
    {
        GameObject gearToDisplay;
        ChestController chestController = chest.GetComponent<ChestController>();
        int newGearLevel;

        if (chestController.gearContained == null)
        {
            gearToDisplay = gearPrefabArray[GenerateRandomGear()];
            newGearLevel = (int)(playerStats.level.GetValue() + 1);
            chestController.gearContained = gearToDisplay;
            
        }
        else
        {
            gearToDisplay = chestController.gearContained;
            newGearLevel = gearToDisplay.GetComponent<ArmorStats>().level;
        }

        gearImage.sprite = gearToDisplay.GetComponent<Image>().sprite;
        gearNameText.text = gearToDisplay.name;

        if (gearToDisplay.CompareTag("Weapon"))
        {

        }
        else if (gearToDisplay.CompareTag("Helmet") || gearToDisplay.CompareTag("Chest"))
        {
            int levelNewStat = newGearLevel;
            int levelOldStat = playerStats.currentChest.GetComponent<ArmorStats>().level;
            float valueNewStat = gearToDisplay.GetComponent<ArmorStats>().CalculateTotalGearValue(gearToDisplay.GetComponent<ArmorStats>().protection, levelNewStat);
            float valueOldStat = playerStats.currentHelmet.GetComponent<ArmorStats>().CalculateTotalGearValue(gearToDisplay.GetComponent<ArmorStats>().protection, levelOldStat);

            if (gearToDisplay.CompareTag("Helmet"))
            {
                valueOldStatText.text = playerStats.currentHelmet.GetComponent<ArmorStats>().protection.GetName() + ": " +
                    valueOldStat +"→";
                levelOldStatText.text = "Level: " + levelOldStat + "→";
            }
            else if (gearToDisplay.CompareTag("Chest"))
            {
                valueOldStatText.text = playerStats.currentChest.GetComponent<ArmorStats>().protection.GetName() + ": " +
                    valueOldStat + "→";
                levelOldStatText.text = "Level: " + levelOldStat + "→";
            }

            valueNewStatText.text = valueNewStat + gearToDisplay.GetComponent<GearStats>().GetGearImprovementArrow(valueOldStat, valueNewStat);
            valueNewStatText.color = gearToDisplay.GetComponent<GearStats>().GetGearImprovementColor((int)valueOldStat, (int)valueNewStat);

            levelNewStatText.text = levelNewStat +
                gearToDisplay.GetComponent<GearStats>().GetGearImprovementArrow(levelOldStat, levelNewStat);
            levelNewStatText.color = gearToDisplay.GetComponent<GearStats>().GetGearImprovementColor(levelOldStat, levelNewStat);
        }
        gearAcquiredPrompt.SetActive(true);
        OnMenuOpen();
    }

    public int GenerateRandomGear()
    {
        System.Random random = new System.Random();
        int gearArrayIndex = random.Next(0, gearPrefabArray.Length);

        return gearArrayIndex;
    }

    public IEnumerator WaitForChestAnimation(GameObject chest)
    {
        yield return new WaitForSeconds(1.5f);
        openingChest = false;
        OpenChest(chest);
    }
}
