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
    public GameObject defaultWeapon;
    public GameObject weaponPosition;
    public MeleeController meleeControllerScript;
    public WeaponStats currentWeaponStats;
    public GameObject gearAcquiredPrompt;
    public GameObject chestPrefab;
    public GameState gameState;
    public bool openingChest = false;
    public Image gearImage;
    public TextMeshProUGUI gearNameText;
    public TextMeshProUGUI valueNewStatText;
    public TextMeshProUGUI valueOldStatText;
    public TextMeshProUGUI levelNewStatText;
    public TextMeshProUGUI levelOldStatText;
    public TextMeshProUGUI attackCooldownOldStatText;
    public TextMeshProUGUI attackCooldownNewStatText;
    public bool wearingDefaultArmor = true;
    public GameObject[] gearPrefabArray;
    public GameObject currentChestBeingOpened;
    public GameObject defaultHelmetPrefab;
    public GameObject defaultChestplatePrefab;
    public int currentGearPrefabArrayIndex = -1;
    public float[] weaponTypeDamageMultiplierArray;
    public float[] weaponAttackCooldownArray;
    public GameObject defaultWeaponPrefab;

    private FirstPersonController firstPersonController;
    private PlayerStats playerStats;
    private ButtonManager buttonManager;

    // Debug components
    public GameObject debugMenu;
    public TextMeshProUGUI difficultyText;
    private bool debugEnabled = true;
    public Difficulty difficulty;
    public bool cursorEnabled = false;
    public bool enemyFollowPlayer;

    public TextMeshProUGUI debugPlayerLevelText;
    public TextMeshProUGUI debugTotalArmorOutputWithCurrentArmorText;
    public TextMeshProUGUI debubTotalArmorOutputWithDefaultArmorText;
    public TextMeshProUGUI debugChestplateLevelText;
    public TextMeshProUGUI debugChestplateProtectionText;
    public TextMeshProUGUI debugHelmetLevelText;
    public TextMeshProUGUI debugHelmetProtectionText;
    public TextMeshProUGUI debugTotalProtectionText;
    public TextMeshProUGUI debugTotalDamageOutputWithCurrentWeaponText;
    public TextMeshProUGUI debugTotalDamageOutputWithDefaultWeaponText;
    public TextMeshProUGUI debugWeaponLevelText;
    public TextMeshProUGUI debugWeaponDamageText;
    public TextMeshProUGUI debugWeaponAttackCooldownSecondsText;
    public TextMeshProUGUI debugDifficultyGearValueMultiplier;

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
        buttonManager = GameObject.Find("Button Manager").GetComponent<ButtonManager>();

        // Set sprint bar max value to the sprint duration from the first person controller
        sprintBar.maxValue = firstPersonController.sprintDuration;

        // Spawn in 2 bears
        Instantiate(bearEnemyPrefab, new Vector3(5, 0, 5), Quaternion.identity);
        Instantiate(bearEnemyPrefab, new Vector3(-5, 0, -5), Quaternion.identity);

        Instantiate(chestPrefab, new Vector3(0, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-2.5f, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-5, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-7.5f, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-10, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-12.5f, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-15, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-17.5f, 0, 5), chestPrefab.transform.rotation);

        //currentWeapon = Instantiate(defaultWeaponPrefab);
        //currentWeapon.transform.SetParent(GameObject.Find("Weapon").transform);
        //currentWeapon.transform.localPosition = defaultWeaponPrefab.transform.position;
        //currentWeapon.transform.localRotation = defaultWeaponPrefab.transform.rotation;
        playerStats.currentWeapon = defaultWeapon;
        currentWeapon = defaultWeapon;

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

        // Debug stats
        debugPlayerLevelText.text = "Player level: " + playerStats.level.GetValue();
        debugTotalArmorOutputWithCurrentArmorText.text = "Total armor output with current armor: " + GetTotalArmorOutput();
        debubTotalArmorOutputWithDefaultArmorText.text = "Total armor output with default armor: " + GetTotalDefaultArmorOutput();
        debugChestplateLevelText.text = "Chestplate level: " + playerStats.currentChest.GetComponent<ArmorStats>().level;
        debugChestplateProtectionText.text = "Chestplate protection: " + playerStats.currentChest.GetComponent<ArmorStats>().protection.GetValue();
        debugHelmetLevelText.text = "Helmet level: " + playerStats.currentHelmet.GetComponent<ArmorStats>().level;
        debugHelmetProtectionText.text = "Helmet protection: " + playerStats.currentHelmet.GetComponent<ArmorStats>().protection.GetValue();
        debugTotalProtectionText.text = "Total protection: " + GetTotalArmorProtection(playerStats.currentHelmet, playerStats.currentChest);
        debugTotalDamageOutputWithCurrentWeaponText.text = "Total damage output with current weapon: " + GetTotalDamageOutput();
        debugTotalDamageOutputWithDefaultWeaponText.text = "Total damage output with default weapon: " + GetTotalDefaultDamageOutput();
        debugWeaponLevelText.text = "Weapon level: " + playerStats.currentWeapon.GetComponent<WeaponStats>().level;
        debugWeaponDamageText.text = "Weapon damage: " + playerStats.currentWeapon.GetComponent<WeaponStats>().damage.GetValue();
        debugWeaponAttackCooldownSecondsText.text = "Weapon attack cooldown: " + playerStats.currentWeapon.GetComponent<WeaponStats>().attackCooldown + "s";
        debugDifficultyGearValueMultiplier.text = "Difficulty gear value multiplier: " + GetDifficultyValueMultiplier() + "x";
    }

    // When opening a menu
    public void OnMenuOpen()
    {
        Time.timeScale = 0;
        gameState = GameState.MENU;
        firstPersonController.enabled = false;
        cursorEnabled = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // When exiting a menu
    public void OnMenuExit()
    {
        Time.timeScale = 1;
        gameState = GameState.GAMEPLAY;
        firstPersonController.enabled = true;
        cursorEnabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // When opening a chest
    public void OpenChest(GameObject chest)
    {
        currentChestBeingOpened = chest;
        GameObject gearToDisplay;
        ChestController chestController = chest.GetComponent<ChestController>();
        int newGearLevel;

        // If there's no gear already in the chest, generate a new gear item
        if (chestController.gearContained == null)
        {
            // Get random piece of gear from array of prefabs
            gearToDisplay = gearPrefabArray[GenerateRandomGear()];
            // Spawn in the piece of gear
            gearToDisplay = Instantiate(gearToDisplay, gearToDisplay.transform.position, gearToDisplay.transform.rotation);
            // Make the gear invisible
            gearToDisplay.SetActive(false);
            // Set the parent of the gear to the chest it is contained in
            //gearToDisplay.transform.parent = chest.transform;
            gearToDisplay.transform.SetParent(chest.transform);

            // The gear level of the gear is the player's level + 1
            newGearLevel = (int)(playerStats.level.GetValue() + 1);
            gearToDisplay.GetComponent<GearStats>().level = newGearLevel;
            // Make the chest contain the new gear
            chestController.gearContained = gearToDisplay;
        }
        // Otherwise, show the gear that's already in the chest
        else
        {
            gearToDisplay = chestController.gearContained;
            newGearLevel = gearToDisplay.GetComponent<GearStats>().level;
        }

        // Set the UI image and text to the gear's image and name
        gearImage.sprite = gearToDisplay.GetComponent<Image>().sprite;
        gearNameText.text = gearToDisplay.name;

        int levelNewStat = -1;
        int levelOldStat = -1;
        float valueNewStat = -1;
        float valueOldStat = -1;

        // Displaying weapon
        if (gearToDisplay.CompareTag("Weapon"))
        {
            // Set levels and values of the new gear and old gear
            attackCooldownOldStatText.gameObject.SetActive(true);
            attackCooldownNewStatText.gameObject.SetActive(true);
            levelNewStat = newGearLevel;
            levelOldStat = -1;
            valueNewStat = gearToDisplay.GetComponent<WeaponStats>().damage.GetValue();
            valueOldStat = -1;

            levelOldStat = playerStats.currentWeapon.GetComponent<WeaponStats>().level;
            valueOldStat = playerStats.currentWeapon.GetComponent<WeaponStats>().damage.GetValue();

            valueOldStatText.text = playerStats.currentWeapon.GetComponent<WeaponStats>().damage.GetName() + ": " +
                valueOldStat + "→";
            levelOldStatText.text = "Level: " + levelOldStat + "→";
            attackCooldownOldStatText.text = "Attack cooldown: " + playerStats.currentWeapon.GetComponent<WeaponStats>().attackCooldown + "s→";

            attackCooldownNewStatText.text = gearToDisplay.GetComponent<WeaponStats>().attackCooldown + "s"
            + gearToDisplay.GetComponent<GearStats>().GetGearImprovementArrow(gearToDisplay.GetComponent<WeaponStats>().attackCooldown,
            playerStats.currentWeapon.GetComponent<WeaponStats>().attackCooldown);
            attackCooldownNewStatText.color = gearToDisplay.GetComponent<GearStats>().GetGearImprovementColor(gearToDisplay.GetComponent<WeaponStats>().attackCooldown,
                playerStats.currentWeapon.GetComponent<WeaponStats>().attackCooldown);
        }
        // Displaying armor
        else if (gearToDisplay.CompareTag("Helmet") || gearToDisplay.CompareTag("Chestplate"))
        {
            // Set levels and values of the new gear and old gear
            attackCooldownOldStatText.gameObject.SetActive(false);
            attackCooldownNewStatText.gameObject.SetActive(false);
            levelNewStat = newGearLevel;
            levelOldStat = -1;
            valueNewStat = gearToDisplay.GetComponent<ArmorStats>().protection.GetValue();
            valueOldStat = -1;

            // If the gear item in the chest is a helmet, get the player's current helmet for comparison
            if (gearToDisplay.CompareTag("Helmet"))
            {
                levelOldStat = playerStats.currentHelmet.GetComponent<ArmorStats>().level;
                valueOldStat = playerStats.currentHelmet.GetComponent<ArmorStats>().protection.GetValue();

                valueOldStatText.text = playerStats.currentHelmet.GetComponent<ArmorStats>().protection.GetName() + ": " +
                    valueOldStat +"→";
                levelOldStatText.text = "Level: " + levelOldStat + "→";
            }
            // Or if the gear item in the chest is a chest, get the player's current helmet for comparison
            else if (gearToDisplay.CompareTag("Chestplate"))
            {
                levelOldStat = playerStats.currentChest.GetComponent<ArmorStats>().level;
                valueOldStat = playerStats.currentChest.GetComponent<ArmorStats>().protection.GetValue();

                valueOldStatText.text = playerStats.currentChest.GetComponent<ArmorStats>().protection.GetName() + ": " +
                    valueOldStat + "→";
                levelOldStatText.text = "Level: " + levelOldStat + "→";
            }
        }

        // Show the values of the new gear with the corresponding improvement color and up/down arrow for the stats
        valueNewStatText.text = valueNewStat + gearToDisplay.GetComponent<GearStats>().GetGearImprovementArrow(valueOldStat, valueNewStat);
        valueNewStatText.color = gearToDisplay.GetComponent<GearStats>().GetGearImprovementColor((int)valueOldStat, (int)valueNewStat);
        levelNewStatText.text = levelNewStat +
            gearToDisplay.GetComponent<GearStats>().GetGearImprovementArrow(levelOldStat, levelNewStat);
        levelNewStatText.color = gearToDisplay.GetComponent<GearStats>().GetGearImprovementColor(levelOldStat, levelNewStat);

        // Show the gear acquired UI
        gearAcquiredPrompt.SetActive(true);
        OnMenuOpen();
    }

    public void EquipGearFromChest()
    {
        ChestController chestController = currentChestBeingOpened.GetComponent<ChestController>();
        GameObject playerToChestGear;
        GameObject chestToPlayerGear = chestController.gearContained;
        

        if (chestToPlayerGear.CompareTag("Helmet"))
        {
            playerToChestGear = playerStats.currentHelmet;

            chestController.gearContained = playerToChestGear;
            playerStats.currentHelmet = chestToPlayerGear;

            playerToChestGear.transform.SetParent(currentChestBeingOpened.transform);
            chestToPlayerGear.transform.SetParent(GameObject.Find("Helmet").gameObject.transform);

        }
        else if (chestToPlayerGear.CompareTag("Chestplate"))
        {
            playerToChestGear = playerStats.currentChest;

            chestController.gearContained = playerToChestGear;
            playerStats.currentChest = chestToPlayerGear;

            playerToChestGear.SetActive(false);
            playerToChestGear.transform.SetParent(currentChestBeingOpened.transform);
            chestToPlayerGear.transform.SetParent(GameObject.Find("Chestplate").gameObject.transform);

            if (currentGearPrefabArrayIndex > -1)
            {
                chestToPlayerGear.transform.localPosition = gearPrefabArray[currentGearPrefabArrayIndex].transform.position;
                chestToPlayerGear.transform.localRotation = gearPrefabArray[currentGearPrefabArrayIndex].transform.rotation;
                chestToPlayerGear.SetActive(true);
            }
        }
        else if (chestToPlayerGear.CompareTag("Weapon"))
        {
            playerToChestGear = playerStats.currentWeapon;

            chestController.gearContained = playerToChestGear;
            playerStats.currentWeapon = chestToPlayerGear;

            playerToChestGear.SetActive(false);
            playerToChestGear.transform.SetParent(currentChestBeingOpened.transform);
            chestToPlayerGear.transform.SetParent(GameObject.Find("Weapon").gameObject.transform);

            chestToPlayerGear.transform.localPosition = gearPrefabArray[currentGearPrefabArrayIndex].transform.localPosition;
            chestToPlayerGear.transform.localRotation = gearPrefabArray[currentGearPrefabArrayIndex].transform.rotation;
            chestToPlayerGear.SetActive(true);

            attackCooldownBar.maxValue = chestToPlayerGear.GetComponent<WeaponStats>().attackCooldown;
        }

        buttonManager.ExitMenu(gearAcquiredPrompt);
    }

    public int GenerateRandomGear()
    {
        System.Random random = new System.Random();
        int gearArrayIndex = random.Next(0, gearPrefabArray.Length);

        currentGearPrefabArrayIndex = gearArrayIndex;
        return gearArrayIndex;
    }

    public IEnumerator WaitForChestAnimation(GameObject chest)
    {
        yield return new WaitForSeconds(1.5f);
        openingChest = false;
        OpenChest(chest);
    }

    public int GetTotalArmorProtection(GameObject helmet, GameObject chestplate)
    {
        return (int)(helmet.GetComponent<ArmorStats>().protection.GetValue() + chestplate.GetComponent<ArmorStats>().protection.GetValue());
    }

    public float GetDifficultyValueMultiplier()
    {
        if (difficulty == Difficulty.easy)
        {
            return 1.75f;
        }
        else if (difficulty == Difficulty.normal)
        {
            return 1;
        }
        else if (difficulty == Difficulty.hard)
        {
            return 0.75f;
        }

        return -1;
    }

    public float GetTotalArmorOutput()
    {
        return playerStats.currentHelmet.GetComponent<ArmorStats>().protection.GetValue()
            * playerStats.currentChest.GetComponent<ArmorStats>().protection.GetValue()
            * GetDifficultyValueMultiplier()
            * GetHighestArmorLevel();
    }

    public float GetTotalDefaultArmorOutput()
    {
        return defaultHelmetPrefab.GetComponent<ArmorStats>().protection.GetValue()
            * defaultChestplatePrefab.GetComponent<ArmorStats>().protection.GetValue()
            * GetDifficultyValueMultiplier()
            * defaultHelmetPrefab.GetComponent<ArmorStats>().level;
    }

    public float GetTotalDamageOutput()
    {
        return playerStats.currentWeapon.GetComponent<WeaponStats>().damage.GetValue()
            * playerStats.currentWeapon.GetComponent<WeaponStats>().weaponTypeDamageMultiplier
            * GetDifficultyValueMultiplier()
            * playerStats.currentWeapon.GetComponent<WeaponStats>().level;
    }

    public float GetTotalDefaultDamageOutput()
    {
        return defaultWeaponPrefab.GetComponent<WeaponStats>().damage.GetValue()
            * defaultWeaponPrefab.GetComponent<WeaponStats>().weaponTypeDamageMultiplier
            * GetDifficultyValueMultiplier()
            * playerStats.level.GetValue();
    }

    public int GetHighestArmorLevel()
    {
        int highestArmorLevel = playerStats.currentHelmet.GetComponent<GearStats>().level;

        if (playerStats.currentChest.GetComponent<GearStats>().level > highestArmorLevel)
        {
            highestArmorLevel = playerStats.currentChest.GetComponent<GearStats>().level;
        }

        return highestArmorLevel;
    }
}
