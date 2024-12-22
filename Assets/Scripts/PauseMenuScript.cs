using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Get from paused menu to main menu
    public void GetBackToMainMenu()
    {
         GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            Destroy(player);
        }
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if(gameManager != null){
            Destroy(gameManager);
        }
        SceneManager.LoadScene(0);
        Time.timeScale = 1;

        GameObject audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        if(audioManager != null){
            Destroy(audioManager);
        }

        GameObject difficultyManager = GameObject.FindGameObjectWithTag("DifficultyManager");
        if(difficultyManager != null){
            Destroy(difficultyManager);
        }

    }

    // Exit the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
