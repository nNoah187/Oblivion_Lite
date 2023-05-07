using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static WeaponStats;
using Unity.VisualScripting;

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
    public GameObject[] testingGearPrefabArray;
    public GameObject[] gearPrefabArray;
    public GameObject[] testingEnemyPrefabArray;
    public GameObject[] enemyPrefabArray;
    public GameObject currentChestBeingOpened;
    public GameObject defaultHelmetPrefab;
    public GameObject defaultChestplatePrefab;
    public GameObject defaultWeaponPrefab;
    public Transform chestplateTransform;
    public GameObject reticle;

    private FirstPersonController firstPersonController;
    private PlayerStats playerStats;
    private ButtonManager buttonManager;
    private PlayerController playerControllerScript;

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
        playerControllerScript = firstPersonController.gameObject.GetComponent<PlayerController>();

        // Set sprint bar max value to the sprint duration from the first person controller
        sprintBar.maxValue = firstPersonController.sprintDuration;

        // Spawn in enemies
        enemyPrefabArray = testingEnemyPrefabArray;
        for (int i = 0; i < enemyPrefabArray.Length; i++)
        {
            GameObject enemy = Instantiate(enemyPrefabArray[i], new Vector3(4, 0, i * -4), Quaternion.identity);
            enemy.GetComponent<EnemyStats>().level = playerStats.level;
        }

        // Spawn in chests
        Instantiate(chestPrefab, new Vector3(0, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-2.5f, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-5, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-7.5f, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-10, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-12.5f, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-15, 0, 5), chestPrefab.transform.rotation);
        Instantiate(chestPrefab, new Vector3(-17.5f, 0, 5), chestPrefab.transform.rotation);

        gearPrefabArray = testingGearPrefabArray;

        // Spawn in default weapon prefab (axe)
        currentWeapon = Instantiate(defaultWeaponPrefab);
        currentWeapon.transform.position = weaponPosition.transform.position;
        currentWeapon.transform.rotation = weaponPosition.transform.rotation;
        currentWeapon.transform.SetParent(GameObject.Find("Weapon").transform);
        currentWeapon.transform.localPosition = currentWeapon.GetComponent<GearStats>().localPos;
        currentWeapon.transform.localRotation = Quaternion.Euler(currentWeapon.GetComponent<GearStats>().localRot);
        playerStats.currentWeapon = currentWeapon;
        currentWeapon = defaultWeapon;

        OverrideWeaponAnimation(playerStats.currentWeapon);

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

        if (firstPersonController.isSprintCooldown || playerControllerScript.isAttacking)
        {
            sprintBarFill.color = Color.gray;
            sprintBarBackground.color = Color.gray;
        }
        else
        {
            sprintBarFill.color = Color.white;
            sprintBarBackground.color = Color.white;
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
        debugTotalDamageOutputWithCurrentWeaponText.text = "Total damage output with current weapon: " + GetTotalDamageOutput(playerStats.currentWeapon);
        debugTotalDamageOutputWithDefaultWeaponText.text = "Total damage output with default weapon: " + GetTotalDefaultDamageOutput();
        debugWeaponLevelText.text = "Weapon level: " + playerStats.currentWeapon.GetComponent<WeaponStats>().level;
        debugWeaponDamageText.text = "Weapon damage: " + playerStats.currentWeapon.GetComponent<WeaponStats>().damage.GetValue();
        debugWeaponAttackCooldownSecondsText.text = "Weapon attack cooldown: " + GetWeaponAttackCooldown(playerStats.currentWeapon) + "s";
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
        firstPersonController.crosshair = false;
        reticle.SetActive(false);
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
        reticle.SetActive(true);
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
        gearNameText.text = gearToDisplay.GetComponent<GearStats>().name;

        // Declare stats to show in the UI
        int levelNewStat = -1;
        int levelOldStat = -1;
        float valueNewStat = -1;
        float valueOldStat = -1;

        // Displaying weapon
        if (gearToDisplay.CompareTag("Weapon"))
        {
            // Show attack cooldown since we're showing a weapon
            attackCooldownOldStatText.gameObject.SetActive(true);
            attackCooldownNewStatText.gameObject.SetActive(true);
            //Adjust the alightnment of the other stat text to make room for the attack weapon cooldown text
            levelNewStatText.verticalAlignment = VerticalAlignmentOptions.Middle;
            levelOldStatText.verticalAlignment = VerticalAlignmentOptions.Middle;
            valueNewStatText.verticalAlignment = VerticalAlignmentOptions.Middle;
            valueOldStatText.verticalAlignment = VerticalAlignmentOptions.Middle;
            // The UI will display the weapon in the chest's level
            levelNewStat = newGearLevel;
            // The UI will display the weapon in the chest's total damage
            valueNewStat = GetTotalDamageOutput(gearToDisplay);
            // The UI will display the player's weapon level
            levelOldStat = playerStats.currentWeapon.GetComponent<WeaponStats>().level;
            // The UI will display the player's weapon total damage
            valueOldStat = GetTotalDamageOutput(playerStats.currentWeapon);

            // Display player's current weapon damage
            valueOldStatText.text = playerStats.currentWeapon.GetComponent<WeaponStats>().damage.GetName() + ": " +
                valueOldStat + "→";
            // Display player's current weapon level
            levelOldStatText.text = "Level: " + levelOldStat + "→";
            // Display player's current weapon attack cooldown
            attackCooldownOldStatText.text = "Attack cooldown: " + GetWeaponAttackCooldown(playerStats.currentWeapon) + "s→";

            // Display the chest's weapon attack cooldown
            attackCooldownNewStatText.text = GetWeaponAttackCooldown(gearToDisplay) + "s"
            + gearToDisplay.GetComponent<GearStats>().GetGearImprovementArrow(GetWeaponAttackCooldown(gearToDisplay),
                GetWeaponAttackCooldown(playerStats.currentWeapon));
            // Change the color of the attack cooldown stat UI to red, green, or white depenind on if the gear in the chest is an improvement
            attackCooldownNewStatText.color = gearToDisplay.GetComponent<GearStats>().GetGearImprovementColor(GetWeaponAttackCooldown(gearToDisplay),
                GetWeaponAttackCooldown(playerStats.currentWeapon));
        }
        // Displaying armor
        else if (gearToDisplay.CompareTag("Helmet") || gearToDisplay.CompareTag("Chestplate"))
        {
            // Don't attack cooldown since we're not showing a weapon
            attackCooldownOldStatText.gameObject.SetActive(false);
            attackCooldownNewStatText.gameObject.SetActive(false);
            //Adjust the alightnment of the other stat text to take more space since there's no attack cooldown
            levelNewStatText.verticalAlignment = VerticalAlignmentOptions.Bottom;
            levelOldStatText.verticalAlignment = VerticalAlignmentOptions.Bottom;
            valueNewStatText.verticalAlignment = VerticalAlignmentOptions.Top;
            valueOldStatText.verticalAlignment = VerticalAlignmentOptions.Top;
            // The UI will display the armor in the chest's level
            levelNewStat = newGearLevel;
            // The UI will display the armor in the chest's protection
            valueNewStat = gearToDisplay.GetComponent<ArmorStats>().protection.GetValue();

            // If the gear item in the chest is a helmet, get the player's current helmet for comparison
            if (gearToDisplay.CompareTag("Helmet"))
            {
                // The UI will display the player's helmet level
                levelOldStat = playerStats.currentHelmet.GetComponent<ArmorStats>().level;
                // The UI will display the player's helmet protection
                valueOldStat = playerStats.currentHelmet.GetComponent<ArmorStats>().protection.GetValue();

                // Display player's current helmet protection
                valueOldStatText.text = playerStats.currentHelmet.GetComponent<ArmorStats>().protection.GetName() + ": " +
                    valueOldStat +"→";
                // Display player's current helmet level
                levelOldStatText.text = "Level: " + levelOldStat + "→";
            }
            // Or if the gear item in the chest is a chest, get the player's current helmet for comparison
            else if (gearToDisplay.CompareTag("Chestplate"))
            {
                // The UI will display the player's chestplate level
                levelOldStat = playerStats.currentChest.GetComponent<ArmorStats>().level;
                // The UI will display the player's chestplate protection
                valueOldStat = playerStats.currentChest.GetComponent<ArmorStats>().protection.GetValue();

                // Display player's current chestplate protection
                valueOldStatText.text = playerStats.currentChest.GetComponent<ArmorStats>().protection.GetName() + ": " +
                    valueOldStat + "→";
                // Display player's current chestplate level
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
        GameObject playerToChestGear = null;
        GameObject chestToPlayerGear = chestController.gearContained;
        Transform spawnObjTransform = null;
        
        // Swap the gear in the chest with the player's current gear
        if (chestToPlayerGear.CompareTag("Helmet"))
        {
            playerToChestGear = playerStats.currentHelmet;

            chestController.gearContained = playerToChestGear;
            playerStats.currentHelmet = chestToPlayerGear;

            spawnObjTransform = GameObject.Find("Helmet").transform;

        }
        else if (chestToPlayerGear.CompareTag("Chestplate"))
        {
            playerToChestGear = playerStats.currentChest;

            chestController.gearContained = playerToChestGear;
            playerStats.currentChest = chestToPlayerGear;

            spawnObjTransform = GameObject.Find("Chestplate").transform;
        }
        else if (chestToPlayerGear.CompareTag("Weapon"))
        {
            playerToChestGear = playerStats.currentWeapon;

            chestController.gearContained = playerToChestGear;
            playerStats.currentWeapon = chestToPlayerGear;

            spawnObjTransform = GameObject.Find("Weapon").transform;

            // Update the attack cooldown bar UI since the new weapon could be a different weapon type
            attackCooldownBar.maxValue = GetWeaponAttackCooldown(playerStats.currentWeapon);
            attackCooldownBar.value = attackCooldownBar.maxValue;

            // Override the attack animation for the weapon type
            OverrideWeaponAnimation(playerStats.currentWeapon);
        }

        // Make the new gear in the chest disappear
        playerToChestGear.SetActive(false);
        // Make the new gear in the chest a child of the chest object
        playerToChestGear.transform.SetParent(currentChestBeingOpened.transform);

        // Move the new equipped gear to the world location of its spawn parent
        chestToPlayerGear.transform.position = spawnObjTransform.position;
        chestToPlayerGear.transform.rotation = spawnObjTransform.rotation;
        // Set the new equipped gear to be a child of the correct location on the character
        chestToPlayerGear.transform.SetParent(spawnObjTransform);
        // Set the new equipped gear's position and rotation to its custom Vector3
        chestToPlayerGear.transform.localPosition = chestToPlayerGear.GetComponent<GearStats>().localPos;
        chestToPlayerGear.transform.localRotation = Quaternion.Euler(chestToPlayerGear.GetComponent<GearStats>().localRot);

        // Only show the new gear if it's a weapon or chestplate (helmets don't need to be shown in game)
        if (chestToPlayerGear.CompareTag("Weapon") || chestToPlayerGear.CompareTag("Chestplate"))
        {
            chestToPlayerGear.SetActive(true);
        }

        // Exit the menu
        buttonManager.ExitMenu(gearAcquiredPrompt);
    }

    public int GenerateRandomGear()
    {
        System.Random random = new System.Random();
        // Get a random index of all possible gear pieces on the array of prefabs
        int gearArrayIndex = random.Next(0, gearPrefabArray.Length);

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
        return (playerStats.currentHelmet.GetComponent<ArmorStats>().protection.GetValue()
            + playerStats.currentChest.GetComponent<ArmorStats>().protection.GetValue())
            * GetDifficultyValueMultiplier()
            * GetHighestArmorLevel();
    }

    public float GetTotalDefaultArmorOutput()
    {
        return (defaultHelmetPrefab.GetComponent<ArmorStats>().protection.GetValue()
            + defaultChestplatePrefab.GetComponent<ArmorStats>().protection.GetValue())
            * GetDifficultyValueMultiplier()
            * defaultHelmetPrefab.GetComponent<ArmorStats>().level;
    }

    public float GetTotalDamageOutput(GameObject weapon)
    {
        return weapon.GetComponent<WeaponStats>().damage.GetValue()
            * GetWeaponTypeDamageMultiplier(weapon)
            * GetDifficultyValueMultiplier()
            * weapon.GetComponent<WeaponStats>().level;
    }

    public float GetTotalDefaultDamageOutput()
    {
        return defaultWeaponPrefab.GetComponent<WeaponStats>().damage.GetValue()
            * GetWeaponTypeDamageMultiplier(defaultWeaponPrefab)
            * GetDifficultyValueMultiplier()
            * playerStats.level.GetValue();
    }

    // Return the highest level of the player's current helmet or armor
    public int GetHighestArmorLevel()
    {
        int highestArmorLevel = playerStats.currentHelmet.GetComponent<GearStats>().level;

        if (playerStats.currentChest.GetComponent<GearStats>().level > highestArmorLevel)
        {
            highestArmorLevel = playerStats.currentChest.GetComponent<GearStats>().level;
        }

        return highestArmorLevel;
    }

    // Return weapon attack cooldown depending on weapon type
    public float GetWeaponAttackCooldown(GameObject weapon)
    {
        WeaponStats weaponStats = weapon.GetComponent<WeaponStats>();

        if (weaponStats.weaponType == WeaponType.SWORD)
        {
            return 0.75f;
        }
        else if (weaponStats.weaponType == WeaponType.AXE)
        {
            return 1.5f;
        }
        else if (weaponStats.weaponType == WeaponType.BLUNT)
        {
            return 2.25f;
        }

        return -1;
    }

    // Return weapon damage multiplier depending on weapon type
    public float GetWeaponTypeDamageMultiplier(GameObject weapon)
    {
        WeaponStats weaponStats = weapon.GetComponent<WeaponStats>();

        if (weaponStats.weaponType == WeaponType.SWORD)
        {
            return 0.5f;
        }
        else if (weaponStats.weaponType == WeaponType.AXE)
        {
            return 1;
        }
        else if (weaponStats.weaponType == WeaponType.BLUNT)
        {
            return 1.25f;
        }

        return -1;
    }

    public void OverrideWeaponAnimation(GameObject weapon)
    {
        WeaponStats weaponStats = weapon.GetComponent<WeaponStats>();
        SetAttackType setAttackTypeScript = GetComponent<SetAttackType>();
        Animator playerAnimator = firstPersonController.gameObject.GetComponentInChildren<Animator>();

        if (weaponStats.weaponType == WeaponType.SWORD)
        {
            setAttackTypeScript.Set(0);
            playerAnimator.SetFloat("attackSpeed", 1.25f);
        }
        else if (weaponStats.weaponType == WeaponType.AXE)
        {
            setAttackTypeScript.Set(1);
            playerAnimator.SetFloat("attackSpeed", 1);
        }
        else if (weaponStats.weaponType == WeaponType.BLUNT)
        {
            setAttackTypeScript.Set(2);
            playerAnimator.SetFloat("attackSpeed", 1);
        }
    }
}
