using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameStructs;

public class DifficultyManager : MonoBehaviour
{
    public int gameDifficulty = 0;
    public TextMeshProUGUI difficultyText;
    private Difficulty currentDifficulty = Difficulty.Easy;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;
        UpdateDifficultyUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
        {
            //Debug.Log("Scene changed to 1");
        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    
    public Difficulty GetDifficulty()
    {
        return currentDifficulty;
    }

    public void IncreaseDifficulty()
    {
        gameDifficulty = Mathf.Clamp(gameDifficulty + 1, 0, 2);
        UpdateDifficultyUI();
    }

    public void DecreaseDifficulty()
    {
        gameDifficulty = Mathf.Clamp(gameDifficulty - 1, 0, 2);
        UpdateDifficultyUI();
    }

    private void UpdateDifficultyUI()
    {
        currentDifficulty = (Difficulty)gameDifficulty;

        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                difficultyText.text = "EASY";
                difficultyText.color = new Color32(0, 255, 0, 255);
                break;

            case Difficulty.Medium:
                difficultyText.text = "MEDIUM";
                difficultyText.color = new Color32(255, 255, 0, 255);
                break;

            case Difficulty.Hard:
                difficultyText.text = "HARD";
                difficultyText.color = new Color32(255, 0, 0, 255);
                break;
        }
    }
}
