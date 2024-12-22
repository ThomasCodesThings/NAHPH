using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameStructs;
using UnityEngine.SceneManagement;


public class VictoryScript : MonoBehaviour
{
    private GameObject gameManager;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text xpText;
    [SerializeField] TMP_Text enemiesKilledText;
    [SerializeField] TMP_Text medkitsUsedText;

    private GameObject audioManager;


    public void exitGame(){
        Application.Quit();
    }

    public void switchToMainMenu(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            Destroy(player);
        }
        
        if(gameManager != null){
            Destroy(gameManager);
        }

        if(audioManager != null){
            Destroy(audioManager);
        }

        GameObject difficultyManager = GameObject.FindGameObjectWithTag("DifficultyManager");
        if(difficultyManager != null){
            Destroy(difficultyManager);
        }
        
        SceneManager.LoadScene(0);
       
    }

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");
    }
    // Start is called before the first frame update
    void Start()
    {   
        audioManager.GetComponent<AudioScript>().musicSource.Stop();
        audioManager.GetComponent<AudioScript>().musicSource.clip = audioManager.GetComponent<AudioScript>().victorySound;
        audioManager.GetComponent<AudioScript>().musicSource.loop = false;
        audioManager.GetComponent<AudioScript>().musicSource.Play();

        if(gameManager == null){
            return;
        }
        GameStats stats = gameManager.GetComponent<GameManager>().getGameStats();
        timeText.text = stats.time;
        xpText.text = stats.totalXp;
        enemiesKilledText.text = stats.totalEnemiesKilled;
        medkitsUsedText.text = stats.totalMedkitsUsed;
        Destroy(gameManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
