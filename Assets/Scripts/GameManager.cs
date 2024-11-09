using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructs;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
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
    private float gracePeriod = 5.0f;
    private float duration = 0.0f;
    private int currentWave = 1;
    private int enemiesKilled = 0;
    private int enemiesToKill = 2;
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

    private Vector3 generateRandomSpawnPoint(){
    float randomX = Random.Range(-5, 5);//Random.Range(-width, width);
    float y = 5f;
    float z = 0f; 

    return new Vector3(randomX, y, z);
}

    private List<GameObject> spawnEnemies(int count)
{
    List<GameObject> enemies = new List<GameObject>(); 
    for (int i = 0; i < count; i++)
    {
        Vector3 spawnPoint = generateRandomSpawnPoint();
        GameObject enemy = Instantiate(soldierEnemyPrefab, spawnPoint, Quaternion.identity);
        enemies.Add(enemy); 
    }
    return enemies;
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

    private IEnumerator handleWaveTime()
{
    duration = 0;
    List<GameObject> enemies = spawnEnemies(enemiesToKill); // Use List instead of array
    while (enemies.Count > 0)
    {
        timerText.text = floatToMinutesSeconds(duration);
        yield return null;
        duration += Time.deltaTime;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetComponent<WeakEnemyScript>().isKilled())
            {
                Destroy(enemies[i]);
                enemies.RemoveAt(i);
                i--;
            }
        }
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
