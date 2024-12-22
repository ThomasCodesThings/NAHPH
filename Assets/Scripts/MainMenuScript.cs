using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] GameObject optionsPanel;

    // Play the game
    public void Play()
    {
        SceneManager.LoadSceneAsync(1);
    }


    // Open the settings menu
    public void Settings()
    {
        optionsPanel.SetActive(true);
    }

    
    // Exit the game
    public void Exit()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
