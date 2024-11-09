using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructs;
using TMPro;
using UnityEngine.UI;

public class ProceduralGeneration : MonoBehaviour
{

    /************************************************************************
     * 
     *  Prefabs and Variables
     * 
     * *********************************************************************/

    [SerializeField] GameObject dirtPrefab;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject floorLeftPrefab;
    [SerializeField] GameObject floorRightPrefab;
    [SerializeField] GameObject platformCenterPrefab;
    [SerializeField] GameObject platformLeftPrefab;
    [SerializeField] GameObject platformRightPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] GameObject player;
    [SerializeField] GameObject soldierEnemyPrefab;

    /************************************************************************
     * 
     *  Texts
     * 
     * *********************************************************************/

    [SerializeField] TMP_Text healthText;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider xpBar;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] TMP_Text magazineText;
    [SerializeField] TMP_Text weaponNameText;
    [SerializeField] TMP_Text aidKitsText;

    /************************************************************************
     * 
     *  Private Variables
     * 
     * *********************************************************************/

    private GameObject difficultyManager;
    private PlayerStats playerStats;
    private float totalElapsedTime = 0.0f;
    private float gracePeriod = 10.0f;
    private float duration = 0.0f;
    private int currentWave = 1;
    private int enemiesKilled = 0;
    private int enemiesToKill = 10;
    private int maxWaves = 5;

    private List<int> heights = new List<int>();
    private Difficulty currentDifficulty = Difficulty.Easy;

    private int wallSize = 20;

    public List<int> getHeights(){
        return heights;
    }

    public int getBaseWidth(){
        return width;
    }

    public float getElapsedTime(){
        return totalElapsedTime;
    }


   public void generate()
    {
        // Initial random height for the first segment

        for (int x = -width; x < width; x++)
        {
            Instantiate(dirtPrefab, new Vector3(x, 2, 0), Quaternion.identity);
            Instantiate(floorPrefab, new Vector3(x, 3, 0), Quaternion.identity);
        }
    }

    public string floatToMinutesSeconds(float time)
{
    int minutes = Mathf.FloorToInt(time / 60);
    int seconds = Mathf.FloorToInt(time % 60);

    return string.Format("{0:00}:{1:00}", minutes, seconds);
}

    public void updateUI(){
        healthText.text = playerStats.health.ToString();
    }

    private IEnumerator handleWaves(){
        while (currentWave <= maxWaves)
        {
            waveText.text = "GRACE PERIOD";
            timerText.color = Color.red;
            yield return handleGraceTime();
            waveText.text = "WAVE " + currentWave;
            timerText.color = Color.white;
            yield return handleWaveTime();
            currentWave++;
        }
    }

    private IEnumerator handleGraceTime(){
        duration = gracePeriod;
        while (duration > 0)
        {
            timerText.text = floatToMinutesSeconds(duration);
            yield return null;
            duration -= Time.deltaTime;
        }
        Debug.Log("Grace period ended.");
    }

    private IEnumerator handleWaveTime(){
        duration = 0;
        while( enemiesKilled < enemiesToKill)
        {
            timerText.text = floatToMinutesSeconds(duration);
            yield return null;
            duration += Time.deltaTime;
        }
    }
    
   void Awake()
{
    difficultyManager = GameObject.Find("DifficultyManager");

    if (difficultyManager != null)
    {
        currentDifficulty = difficultyManager.GetComponent<DifficultyManager>().GetDifficulty();
        Debug.Log("Current Difficulty: " + currentDifficulty);
    }
    else
    {
        Debug.LogError("DifficultyManager not found in the scene.");
    }
}


    // Start is called before the first frame update
    void Start()
    {
      StartCoroutine(handleWaves());
      generate();   
    }

    // Update is called once per frame
    void Update()
    {
        playerStats = player.GetComponent<PlayerScript>().getPlayerStats();
        healthBar.value = playerStats.health;
        healthBar.maxValue = playerStats.maxHealth;
        updateUI();
        totalElapsedTime += Time.deltaTime;
    }
}
