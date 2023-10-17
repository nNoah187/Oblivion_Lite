using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : CharacterStats
{
    public GameObject currentHelmet;
    public GameObject currentChest;
    public GameObject currentWeapon;
    public float currentXP;

    // Start is called before the first frame update
    void Start()
    {
        currentHelmet = GameObject.Find("Helmet").GetComponentInChildren<ArmorStats>().gameObject;
        currentChest = GameObject.Find("Chestplate").GetComponentInChildren<ArmorStats>().gameObject;
        currentWeapon = GameObject.Find("Weapon").GetComponentInChildren<WeaponStats>().gameObject;

        currentXP = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public float GetFullXPToNextLevel()
    {
        return (level.GetValue() + 1) * 100;
    }

    public float GetRemainingXPToNextLevel()
    {
        return GetFullXPToNextLevel() - currentXP;
    }

    public void AddXP(float xpToAdd)
    {
        gameManagerScript.ShowXPNotification(xpToAdd);

        if (xpToAdd + currentXP < GetFullXPToNextLevel())
        {
            currentXP += xpToAdd;
            gameManagerScript.xpBar.value = currentXP;
        }
        else
        {
            float xpToCarryOver;
            xpToCarryOver = (xpToAdd + currentXP) - GetFullXPToNextLevel();

            level.SetValue(level.GetValue() + 1);
            currentXP = xpToCarryOver;
            gameManagerScript.xpBar.maxValue = GetFullXPToNextLevel();
            gameManagerScript.xpBar.value = xpToCarryOver;
            gameManagerScript.levelText.text = "Lvl " + level.GetValue();
        }
    }
}
