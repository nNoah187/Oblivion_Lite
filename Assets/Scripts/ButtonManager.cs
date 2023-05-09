using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private GameManager gameManagerScript;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerStats = GameObject.Find("FirstPersonController").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitMenu(GameObject uiPrompt)
    {
        uiPrompt.SetActive(false);
        gameManagerScript.OnMenuExit();
    }

    public void EnableEasyDifficulty()
    {
        PlayerPrefs.SetInt("difficulty", 0);
        gameManagerScript.difficulty = GameManager.Difficulty.easy;
        gameManagerScript.difficultyText.text = "Difficulty: easy";
    }

    public void EnableNormalDifficulty()
    {
        PlayerPrefs.SetInt("difficulty", 1);
        gameManagerScript.difficulty = GameManager.Difficulty.normal;
        gameManagerScript.difficultyText.text = "Difficulty: normal";
    }

    public void EnableHardDifficulty()
    {
        PlayerPrefs.SetInt("difficulty", 2);
        gameManagerScript.difficulty = GameManager.Difficulty.hard;
        gameManagerScript.difficultyText.text = "Difficulty: hard";
    }

    public void IncreasePlayerLevel()
    {
        playerStats.level.SetValue(playerStats.level.GetValue() + 1);
    }

    public void DecreasePlayerLevel()
    {
        playerStats.level.SetValue(playerStats.level.GetValue() - 1);
    }

    public void AddXP(float amount)
    {
        playerStats.AddXP(amount);
    }

    public void SetPlayerLevel(int level)
    {
        playerStats.level.SetValue(level);
    }
}
