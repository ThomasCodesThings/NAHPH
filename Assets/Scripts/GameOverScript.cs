using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameStructs;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
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

        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        GameObject audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        if(audioManager != null){
            Destroy(audioManager);
        }
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
        audioManager.GetComponent<AudioScript>().musicSource.clip = audioManager.GetComponent<AudioScript>().gameOverSound;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
