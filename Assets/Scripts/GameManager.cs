using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructs;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] int width = 25;
    [SerializeField] int height;
    [SerializeField] GameObject player;
    [SerializeField] GameObject soldierEnemyPrefab;
    [SerializeField] GameObject medkitPrefab;
    [SerializeField] GameObject ammoPackPrefab;

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
    [SerializeField] TMP_Text medkitsCountText;
    /************************************************************************
     * 
     *  Private Variables
     * 
     * *********************************************************************/

    private GameObject difficultyManager;
    private PlayerStats playerStats;
    private float totalElapsedTime = 0.0f;
    private float gracePeriod = 5.0f;
    private float duration = 1.0f;
    private int currentWave = 1;
    private int enemyCount = 1;
    private int medkitCount = 1;
    private int ammoCount = 1;
    private int maxWaves = 5;


    private List<int> heights = new List<int>();
    private List<Level> levels = new List<Level>();
    private Difficulty currentDifficulty = Difficulty.Easy;
    private string[] medkitTypes = { "small", "medium", "large" };
    private string[] ammoTypes = { "small", "medium", "large" };
    private Dictionary<int, GameObject> weaponThresholds;
    private HashSet<int> spawnedThresholds;

    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> medkits = new List<GameObject>();
    private List<GameObject> ammos = new List<GameObject>();

    private int wallSize = 10;

    /************************************************************************
     * 
     *  Weapons
     * 
     * *********************************************************************/

    [SerializeField] GameObject smgPrefab;
    [SerializeField] GameObject shotgunPrefab;

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
            heights.Add(3);
           

        }
        for (int i = 0; i < wallSize; i++)
        {
            Instantiate(dirtPrefab, new Vector3(-width, 3 + i, 0), Quaternion.identity);
            Instantiate(dirtPrefab, new Vector3(width - 1, 3 + i, 0), Quaternion.identity);
        }

       

    }
    //(number of enemies, number of medkits, number of ammos)
    public (int, int, int) getLevelSettings(){
        switch(currentDifficulty){
            case Difficulty.Easy:
                switch(currentWave){
                    case 1:
                        return (2, 1, 1);
                    case 2:
                        return (3, 3, 1);
                    case 3:
                        return (5, 5, 3);
                    case 4:
                        return (7, 2, 5);
                    case 5:
                        return (10, 3, 5);
                    default:
                        return (1, 1, 1);
                }
            case Difficulty.Medium:
                switch(currentWave){
                    case 1:
                        return (5, 3, 1);
                    case 2:
                        return (7, 3, 1);
                    case 3:
                        return (10, 5, 3);
                    case 4:
                        return (15, 2, 5);
                    case 5:
                        return (15, 3, 5);
                    default:
                        return (1, 1, 1);
                }
            case Difficulty.Hard:
                switch(currentWave){
                    case 1:
                        return (7, 3, 1);
                    case 2:
                        return (10, 3, 1);
                    case 3:
                        return (15, 5, 3);
                    case 4:
                        return (20, 2, 5);
                    case 5:
                        return (20, 3, 5);
                    default:
                        return (1, 1, 1);
                }
            default:
                return (1, 1, 1);
        }
    }

    private int calculateWaveTime(){
        switch(currentDifficulty){
            case Difficulty.Easy:
                return (currentWave - 1)  * 60 + 180;
            case Difficulty.Medium:
                return (currentWave - 1)  * 60 + 120;
            case Difficulty.Hard:
                return (currentWave - 1)  * 60 + 60;
            default:
                return 60;
        }
    }

    public string floatToMinutesSeconds(float time)
{
    int minutes = Mathf.FloorToInt(time / 60);
    int seconds = Mathf.FloorToInt(time % 60);

    return string.Format("{0:00}:{1:00}", minutes, seconds);
}

public string floatToDate(float time)
{
    int minutes = Mathf.FloorToInt(time / 60);
    int seconds = Mathf.FloorToInt(time % 60);
    int hours = Mathf.FloorToInt(minutes / 60);

    return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
}

public string formatNumber(int number)
{
    return string.Format("{0:n0}", number);
}

public GameStats getGameStats()
{
    if(player == null){
        return new GameStats("00:00:00", "0", "0", "0");
    }
    string formatedTime = floatToDate(totalElapsedTime);
    string formatedEnemiesKilled = formatNumber(player.GetComponent<PlayerScript>().getKills());
    string formatedMedkitsUsed = formatNumber(player.GetComponent<PlayerScript>().getMedkitsUsed());
    string formatedXp = formatNumber(player.GetComponent<PlayerScript>().getXp());
    return new GameStats(formatedTime, formatedEnemiesKilled, formatedMedkitsUsed, formatedXp);
}

private void removeCollision(List<GameObject> enemies)
{
    foreach (GameObject enemy in enemies)
    {
        if (enemy == null)
        {
            continue;
        }
        foreach (GameObject medkit in medkits)
        {
            if (medkit == null)
            {
                continue;
            }
            Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), medkit.GetComponent<Collider2D>());
        }

        foreach (GameObject ammo in ammos)
        {
            if (ammo == null)
            {
                continue;
            }
            Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), ammo.GetComponent<Collider2D>());
        }
    }

}

public void clearAfterWave(){
    foreach(GameObject enemy in enemies){
        if(enemy != null){
            Destroy(enemy);
        }
    }

    foreach(GameObject medkit in medkits){
        if(medkit != null){
            Destroy(medkit);
        }
    }

    foreach(GameObject ammo in ammos){
        if(ammo != null){
            Destroy(ammo);
        }
    }
}

    private Vector3 generateRandomSpawnPoint(){
    float randomX = Random.Range(-width, width);
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

private List<GameObject> spawnMedKits(int count){
    List<GameObject> medkits = new List<GameObject>();
    for (int i = 0; i < count; i++)
    {
        Vector3 spawnPoint = generateRandomSpawnPoint();
        GameObject medkit = Instantiate(medkitPrefab, spawnPoint, Quaternion.identity);
        string medkitType = medkitTypes[Random.Range(0, medkitTypes.Length)];
        medkit.GetComponent<MedkitScript>().updateMedkit(medkitType);
        medkits.Add(medkit);
    }
    return medkits;
}

private List<GameObject> spawnAmmo(int count){
    List<GameObject> ammos = new List<GameObject>();
    for (int i = 0; i < count; i++)
    {
        Vector3 spawnPoint = generateRandomSpawnPoint();
        GameObject ammo = Instantiate(ammoPackPrefab, spawnPoint, Quaternion.identity);
        string ammoType = ammoTypes[Random.Range(0, ammoTypes.Length)];
        ammo.GetComponent<AmmoPackScript>().updateAmmoPack(ammoType);
        ammos.Add(ammo);
    }
    return ammos;
}

    private IEnumerator handleWaves(){
        while (currentWave <= maxWaves)
        {
            (int enemiesToKill, int medkitsToSpawn, int ammosToSpawn) = getLevelSettings();
            enemyCount = enemiesToKill;
            medkitCount = medkitsToSpawn;
            ammoCount = ammosToSpawn;
            waveText.text = "GRACE PERIOD";
            timerText.color = Color.red;
            yield return handleGraceTime();
            waveText.text = "WAVE " + currentWave;
            timerText.color = Color.white;
            yield return handleWaveTime();
            currentWave++;
            clearAfterWave();
        }
        SceneManager.LoadScene("VictoryScene");
    }

    private IEnumerator handleGraceTime(){
        duration = gracePeriod;
        medkits = spawnMedKits(medkitCount);
        ammos = spawnAmmo(ammoCount);
        while (duration > 0)
        {
            timerText.text = floatToMinutesSeconds(duration);
            yield return null;
            duration -= Time.deltaTime;
        }
    }

    private IEnumerator handleWaveTime()
{
    duration = calculateWaveTime();
    enemies = spawnEnemies(enemyCount);

    removeCollision(enemies);
    while (enemies.Count > 0 && duration > 0)
    {
        timerText.text = floatToMinutesSeconds(duration);
        yield return null;
        duration -= Time.deltaTime;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null && enemies[i].GetComponent<WeakEnemyScript>().isKilled())
            {
                int xp = enemies[i].GetComponent<WeakEnemyScript>().getXP();
                player.GetComponent<PlayerScript>().addXP(xp);
                player.GetComponent<PlayerScript>().addKill();

                Destroy(enemies[i]);
                enemies.RemoveAt(i);
                i--;
            }
        }
    }
    //game over
    if (duration <= 0)
    {
        SceneManager.LoadScene("GameOverScene");
    }
}

public void updateUI(PlayerStats playerStats){
        healthText.text = playerStats.health.ToString();
        healthBar.value = playerStats.health;
        healthBar.maxValue = playerStats.maxHealth;
        medkitsCountText.text = playerStats.medkits.ToString();

        int currentLevel = 0;
        int xpCount = 0;

        while(currentLevel < levels.Count - 1 && xpCount + levels[currentLevel].xpToNextLevel <= playerStats.xp)
        {
            xpCount += levels[currentLevel].xpToNextLevel;
            currentLevel++;
        }

        int difference = playerStats.xp - xpCount;
        int xpForNextLevel = levels[currentLevel].xpToNextLevel;
        float differencePercentage = (float)difference / xpForNextLevel;
        xpBar.value = differencePercentage;
        levelText.text = "LEVEL " + ++currentLevel;
        ammoText.text = playerStats.ammo.ToString();
        magazineText.text = playerStats.magazine.ToString();
        weaponNameText.text = playerStats.weaponName;
    }
    
   void Awake()
{
    DontDestroyOnLoad(gameObject);
    difficultyManager = GameObject.FindGameObjectWithTag("DifficultyManager");

    if (difficultyManager != null)
    {
        currentDifficulty = difficultyManager.GetComponent<DifficultyManager>().getDifficulty();
        Debug.Log("Current Difficulty: " + currentDifficulty);
    }
    else
    {
        Debug.LogError("DifficultyManager not found in the scene.");
    }

    levels.Add(new Level(100));
    levels.Add(new Level(200));
    levels.Add(new Level(300));
    levels.Add(new Level(400));
    levels.Add(new Level(500));

    weaponThresholds = new Dictionary<int, GameObject>
        {
            { 100, smgPrefab },
            { 200, shotgunPrefab },
        };

    spawnedThresholds = new HashSet<int>();

    
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
    if (player != null)
    {
        playerStats = player.GetComponent<PlayerScript>().getPlayerStats();
        updateUI(playerStats);
        totalElapsedTime += Time.deltaTime;

        if (player.GetComponent<PlayerScript>().isKilled())
        {
       
            //player = null;

            SceneManager.LoadScene("GameOverScene");
        }

        foreach (int threshold in weaponThresholds.Keys)
        {
            if (playerStats.xp >= threshold && !spawnedThresholds.Contains(threshold))
            {
                Vector3 spawnPoint = generateRandomSpawnPoint();
                Instantiate(weaponThresholds[threshold], spawnPoint, Quaternion.identity);
                spawnedThresholds.Add(threshold);
            }
        }
    }
}
}
