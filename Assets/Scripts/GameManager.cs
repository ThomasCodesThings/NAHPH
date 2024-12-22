using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructs;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AI;
using System;

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
    [SerializeField] int height = 20;
    [SerializeField] GameObject player;
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] GameObject dronePrefab;
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
    private int soldierCount = 1;
    private int droneCount = 1;
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
    private string currentWeapon = "Basic Pistol";
    private int[,] blockGrid;
    private AStar AStarSearch;

    private static int seed = Math.Abs(Guid.NewGuid().GetHashCode());

    System.Random random = new System.Random(seed);

    /************************************************************************
     * 
     *  Weapons
     * 
     * *********************************************************************/

    [SerializeField] GameObject smgPrefab;
    [SerializeField] GameObject shotgunPrefab;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject plasmaCannonPrefab;

    [SerializeField] GameObject baseBulletPrefab;
    [SerializeField] GameObject invisBulletPrefab;
    [SerializeField] GameObject plasmaBulletPrefab;

    [SerializeField] Texture2D basePistolIcon;
    [SerializeField] Texture2D smgIcon;
    [SerializeField] Texture2D shotgunIcon;
    [SerializeField] Texture2D laserIcon;
    [SerializeField] Texture2D plasmaCannonIcon;

    private Dictionary<string, RangeWeapon> weapons = new Dictionary<string, RangeWeapon>();

    private List<(int, int)> blockedCells = new List<(int, int)>();

    private GameObject audioManager;

    /************************************************************************
     * 
     *  Stats
     * 
     * *********************************************************************/

    private int playerXp = 0;
    private int playerMedkitsUsed = 0;
    private int playerEnemiesKilled = 0;




    public List<int> getHeights(){
        return heights;
    }

    public int getBaseWidth(){
        return width;
    }

    public float getElapsedTime(){
        return totalElapsedTime;
    }

    public void setCurrentWeapon(string weapon){
        currentWeapon = weapon;
    }

    public RangeWeapon getCurrentWeapon(){
        return weapons[currentWeapon];
    }

    public void addAmmo(int amount){
        weapons[currentWeapon].addAmmo(amount);
    }

    public void addKill(){
        playerEnemiesKilled++;
    }

    public void addMedkitsUsed(){
        playerMedkitsUsed++;
    }

    public void addXP(int xp){
        playerXp += xp;
    }

    public int getPlayerXP(){
        return playerXp;
    }

    public int getPlayerMedkitsUsed(){
        return playerMedkitsUsed;
    }

    public int getPlayerEnemiesKilled(){
        return playerEnemiesKilled;
    }

    private void generatePlatform(int startX, int startY, int platformWidth){
        int prefabLength = 2;

        if(startX + platformWidth > width){
            return;
        }

        Instantiate(platformLeftPrefab, new Vector3(startX, startY, 0), Quaternion.identity);
        blockGrid[startX + width, startY] = 0;
        blockGrid[startX + width + 1, startY] = 0;
        blockedCells.Add((startX, startY));
        blockedCells.Add((startX + 1, startY));

        for (int x = startX + prefabLength; x < startX + platformWidth - prefabLength; x+= prefabLength)
        {
            Instantiate(platformCenterPrefab, new Vector3(x, startY, 0), Quaternion.identity);
            blockGrid[x + width, startY] = 0;
            blockGrid[x + width + 1, startY] = 0;
            blockedCells.Add((x, startY));
            blockedCells.Add((x + 1, startY));
        }

        Instantiate(platformRightPrefab, new Vector3(startX + platformWidth - prefabLength, startY, 0), Quaternion.identity);
        blockGrid[startX + platformWidth - prefabLength + width, startY] = 0;
        blockGrid[startX + platformWidth - prefabLength + 1 + width, startY] = 0;
        blockedCells.Add((startX + platformWidth - prefabLength, startY));
        blockedCells.Add((startX + platformWidth - prefabLength + 1, startY));
    }


    /*
        private void generatePlatform(int startX, int startY, int platformWidth)
        {
            int prefabLength = 2;

            int endX = Mathf.Min(startX + platformWidth, width - prefabLength); 
            int clippedWidth = Mathf.Max(endX - startX, 0);

            if (clippedWidth < prefabLength * 2)
            {
                return;
            }

            Instantiate(platformLeftPrefab, new Vector3(startX, startY, 0), Quaternion.identity);

            int centerWidth = clippedWidth - 2 * prefabLength;
            if (centerWidth > 0)
            {
                GameObject centerInstance = Instantiate(platformCenterPrefab, new Vector3(startX + prefabLength, startY, 0), Quaternion.identity);

                Vector3 scale = centerInstance.transform.localScale;
                scale.x = centerWidth / (float)prefabLength;
                centerInstance.transform.localScale = scale;
            }

            Instantiate(platformRightPrefab, new Vector3(startX + clippedWidth - prefabLength, startY, 0), Quaternion.identity);

            for (int x = startX; x < startX + clippedWidth; x++)
            {
                int gridX = x + width;
                if (gridX >= 0 && gridX < blockGrid.GetLength(0))
                {
                    blockGrid[gridX, startY] = 0;
                }
            }
        }*/




   public void generate()
    {
        int wallSize = height;
        //0 - cell is blocked
        //1 -cell is non blocked

        for (int x = -width; x < width; x++)
        {
            int gridX = x + width;

            Instantiate(dirtPrefab, new Vector3(x, 0, 0), Quaternion.identity);
            Instantiate(floorPrefab, new Vector3(x, 1, 0), Quaternion.identity);
            
            blockGrid[gridX, 0] = 0;  
            blockGrid[gridX, 1] = 0;
            
            heights.Add(1);
        }

        for (int i = 0; i < height; i++)
        {
            Instantiate(dirtPrefab, new Vector3(-width, i, 0), Quaternion.identity);
            Instantiate(dirtPrefab, new Vector3(width - 1, i, 0), Quaternion.identity);

            blockGrid[0, i] = 0;              
            blockGrid[width * 2 - 1, i] = 0;
        }


      /* for (int y = 5; y < height; y+=4)
        {
            for (int x = -width + 3; x < width; x++)
            {   
                int randomInt = random.Next(0, 5);
                bool spawnPlatform = randomInt == 1 || randomInt == 2;

                if (spawnPlatform)
                {
                    int platformWidth = random.Next(4, 8);
                    if(!(platformWidth % 2 == 0)){
                        platformWidth++;
                    }
                    generatePlatform(x, y, platformWidth);
                    x += platformWidth;
                }
                else
                {
                    int gapSize = random.Next(4, 7);
                    x += gapSize; 
                }
            }
        }*/

    }

    //(number of soldiers, number of drones, number of medkits, number of ammos)
    public (int, int, int, int) getLevelSettings(){
        switch(currentDifficulty){
            case Difficulty.Easy:
                switch(currentWave){
                    case 1:
                        return (1, 0, 1, 1);
                    case 2:
                        return (3, 1, 3, 1);
                    case 3:
                        return (5, 1, 5, 3);
                    case 4:
                        return (7, 2, 2, 5);
                    case 5:
                        return (10, 2, 3, 5);
                    default:
                        return (1, 1, 1, 1);
                }
            case Difficulty.Medium:
                switch(currentWave){
                    case 1:
                        return (5, 0, 3, 1);
                    case 2:
                        return (7, 1, 3, 1);
                    case 3:
                        return (10, 2, 5, 3);
                    case 4:
                        return (15, 2, 2, 5);
                    case 5:
                        return (15, 3, 3, 5);
                    default:
                        return (1, 1, 1, 1);
                }
            case Difficulty.Hard:
                switch(currentWave){
                    case 1:
                        return (7, 1, 3, 1);
                    case 2:
                        return (10, 2, 3, 1);
                    case 3:
                        return (15, 2, 5, 3);
                    case 4:
                        return (20, 3, 2, 5);
                    case 5:
                        return (20, 4, 3, 5);
                    default:
                        return (1, 1, 1, 1);
                }
            default:
                return (1, 1, 1, 1);
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
    /*if(player == null){
        return new GameStats("00:00:00", "0", "0", "0");
    }*/
    string formatedTime = floatToDate(totalElapsedTime);
    string formatedEnemiesKilled = formatNumber(playerEnemiesKilled);
    string formatedMedkitsUsed = formatNumber(playerMedkitsUsed);
    string formatedXp = formatNumber(playerXp);
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

private Vector3 generateRandomSpawnPoint(int offsetY = 2)
{

    int chance = random.Next(1, 10);

    if(chance > 0){
        int randomX = random.Next(-width, width);
        int randomY = random.Next(2, 4);
        return new Vector3(randomX, randomY, 0);
    }else{
        //pick random item from blockedCells
        int randomIndex = random.Next(0, blockedCells.Count);
        (int x, int y) = blockedCells[randomIndex];
        return new Vector3(x, y + offsetY, 0);
    }
}


    private List<GameObject> spawnSoldiers(int count)
{
    List<GameObject> enemies = new List<GameObject>(); 
    for (int i = 0; i < count; i++)
    {
        Vector3 spawnPoint = generateRandomSpawnPoint();
        GameObject enemy = Instantiate(soldierPrefab, spawnPoint, Quaternion.identity);
        enemies.Add(enemy); 
    }
    return enemies;
}

 private List<GameObject> spawnDrones(int count)
{
    List<GameObject> enemies = new List<GameObject>(); 
    for (int i = 0; i < count; i++)
    {
        Vector3 spawnPoint = generateRandomSpawnPoint();
        spawnPoint.y = 10f;
        GameObject enemy = Instantiate(dronePrefab, spawnPoint, Quaternion.identity);
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
        string medkitType = medkitTypes[UnityEngine.Random.Range(0, medkitTypes.Length)];
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
        string ammoType = ammoTypes[UnityEngine.Random.Range(0, ammoTypes.Length)];
        ammo.GetComponent<AmmoPackScript>().updateAmmoPack(ammoType);
        ammos.Add(ammo);
    }
    return ammos;
}

public void clearDecals(){
    GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
    foreach(GameObject bullet in bullets){
        Destroy(bullet);
    }

    GameObject[] energyProjectiles = GameObject.FindGameObjectsWithTag("EnergyProjectile");
    foreach(GameObject energyProjectile in energyProjectiles){
        Destroy(energyProjectile);
    }

    GameObject[] ballisticProjectiles = GameObject.FindGameObjectsWithTag("BallisticProjectile");
    foreach(GameObject ballisticProjectile in ballisticProjectiles){
        Destroy(ballisticProjectile);
    }
}

    private IEnumerator handleWaves(){
        while (currentWave <= maxWaves)
        {
            if (waveText == null || timerText == null)
            {
                yield break;
            }
            (int soldiersToSpawn, int dronesToSpawn, int medkitsToSpawn, int ammosToSpawn) = getLevelSettings();
            soldierCount = soldiersToSpawn;
            droneCount = dronesToSpawn;
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
            clearDecals();
        }
        SceneManager.LoadScene("VictoryScene");
    }

    private IEnumerator handleGraceTime(){
        duration = gracePeriod;
        medkits = spawnMedKits(medkitCount);
        ammos = spawnAmmo(ammoCount);
        audioManager.GetComponent<AudioScript>().musicSource.Stop();
        audioManager.GetComponent<AudioScript>().musicSource.clip = audioManager.GetComponent<AudioScript>().graceTimeCountdown;
        audioManager.GetComponent<AudioScript>().musicSource.loop = false;
        audioManager.GetComponent<AudioScript>().musicSource.Play();
        while (duration > 0)
        {
            if(timerText == null){
                yield break;
            }
            timerText.text = floatToMinutesSeconds(duration);
            yield return null;
            duration -= Time.deltaTime;
        }
    }

    private IEnumerator handleWaveTime()
{
    duration = calculateWaveTime();
    List<GameObject> soldiers = spawnSoldiers(soldierCount);
    List<GameObject> drones = spawnDrones(droneCount);

    List<GameObject> enemies = new List<GameObject>();
    enemies.AddRange(soldiers);
    enemies.AddRange(drones);

    removeCollision(enemies);
    audioManager.GetComponent<AudioScript>().musicSource.Stop();
    audioManager.GetComponent<AudioScript>().musicSource.clip = audioManager.GetComponent<AudioScript>().backgroundMusic;
    audioManager.GetComponent<AudioScript>().musicSource.loop = true;
    audioManager.GetComponent<AudioScript>().musicSource.Play();
    while (enemies.Count > 0 && duration > 0)
    {
        if(timerText == null){
            yield break;
        }
        timerText.text = floatToMinutesSeconds(duration);
        yield return null;
        duration -= Time.deltaTime;

        for (int i = 0; i < enemies.Count; i++)
        {

            if(enemies[i] != null){
                switch(enemies[i].tag){
                    case "Soldier":
                        if(enemies[i].GetComponent<SoldierScript>().isKilled()){
                            int xp = enemies[i].GetComponent<SoldierScript>().getXP();
                            player.GetComponent<PlayerScript>().addXP(xp);
                            player.GetComponent<PlayerScript>().addKill();
                            Destroy(enemies[i]);
                            enemies.RemoveAt(i);
                        }

                        break;
                    case "Drone":
                        if(enemies[i].GetComponent<DroneScript>().isKilled()){
                            int xp = enemies[i].GetComponent<DroneScript>().getXP();
                            player.GetComponent<PlayerScript>().addXP(xp);
                            player.GetComponent<PlayerScript>().addKill();
                            Destroy(enemies[i]);
                            enemies.RemoveAt(i);
                        }
                        break;
                }
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
        if (healthBar == null || xpBar == null || levelText == null || waveText == null || timerText == null || ammoText == null || magazineText == null || weaponNameText == null || medkitsCountText == null)
        {
            return;
        }
        healthText.text = playerStats.health.ToString();
        healthBar.value = playerStats.health;
        healthBar.maxValue = playerStats.maxHealth;
        medkitsCountText.text = playerStats.medkits.ToString();

        int currentLevel = 0;
        int xpCount = 0;
        int previousLevel = levelText.text.StartsWith("LEVEL ") 
        ? int.Parse(levelText.text.Substring(6)) - 1 
        : 0;

        while(currentLevel < levels.Count - 1 && xpCount + levels[currentLevel].xpToNextLevel <= playerStats.xp)
        {
            xpCount += levels[currentLevel].xpToNextLevel;
            currentLevel++;
        }

        if(previousLevel != currentLevel){
            audioManager.GetComponent<AudioScript>().playLevelUp();
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
    levels.Add(new Level(600));
    levels.Add(new Level(700));
    levels.Add(new Level(800));
    levels.Add(new Level(900));
    levels.Add(new Level(1000));

    weaponThresholds = new Dictionary<int, GameObject>
        {
            { 10, smgPrefab },
            { 20, shotgunPrefab },
            { 30, laserPrefab },
            { 40, plasmaCannonPrefab }
        };

    spawnedThresholds = new HashSet<int>();

    blockGrid = new int[width * 2, height];
        for (int x = 0; x < blockGrid.GetLength(0); x++)
        {
            for (int y = 0; y < blockGrid.GetLength(1); y++)
            {
                blockGrid[x, y] = 1; 
            }
        }

        weapons.Add("Basic Pistol", new RangeWeapon(damage: 5, maxAmmo: 8, ammo: 8, magazine: 64, name: "Basic Pistol", offsetX: 0.55f, offsetY: 0.1f, bulletSpeed: 10f, bulletLifeTime: 1.5f, shotDelay: 0.2f, bulletPrefab: baseBulletPrefab, weaponIcon: basePistolIcon));
        weapons.Add("Smg", new RangeWeapon(damage: 10, maxAmmo: 10, ammo: 10, magazine: 40, name: "Smg", offsetX: 1.1f, offsetY: 0.15f, bulletSpeed: 10f, bulletLifeTime: 1.5f, shotDelay: 0.2f, bulletPrefab: baseBulletPrefab, weaponIcon: smgIcon));
        weapons.Add("Shotgun", new RangeWeapon(damage: 20, maxAmmo: 7, ammo: 7, magazine: 21, name: "Shotgun", offsetX: 0.9f, offsetY: 0f, bulletSpeed: 10f, bulletLifeTime: 1.5f, shotDelay: 0.2f, bulletPrefab: baseBulletPrefab, weaponIcon: shotgunIcon));
        weapons.Add("Laser Gun", new RangeWeapon(damage: 30, maxAmmo: 5, ammo: 5, magazine: 30, name: "Laser Gun", offsetX: 0.9f, offsetY: 0f, bulletSpeed: 10f, bulletLifeTime: 1.5f, shotDelay: 0.2f, bulletPrefab: baseBulletPrefab, weaponIcon: laserIcon));
        weapons.Add("Plasma Cannon", new RangeWeapon(damage: 40, maxAmmo: 5, ammo: 5, magazine: 20, name: "Plasma Cannon", offsetX: 0.9f, offsetY: 0f, bulletSpeed: 10f, bulletLifeTime: 1.5f, shotDelay: 0.2f, bulletPrefab: plasmaBulletPrefab, weaponIcon: plasmaCannonIcon));

     audioManager = GameObject.FindGameObjectWithTag("AudioManager");
}

public (int, int) getNextBlock(float srcX, float srcY)
{
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    int enemyX = (int)Math.Round(srcX) + width;
    int enemyY = (int)Math.Round(srcY);
    int playerX = (int)Math.Round(player.transform.position.x) + width;
    int playerY = (int)Math.Round(player.transform.position.y) + 1;
    
    return AStarSearch.getNextMove(enemyX, enemyY, playerX, playerY);
}

    // Start is called before the first frame update
    void Start()
    {
      generate();
      StartCoroutine(handleWaves());
      AStarSearch = new AStar(blockGrid);
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
            Destroy(player);
            SceneManager.LoadScene("GameOverScene");
        
        }

        if(weaponThresholds == null || spawnedThresholds == null){
            return;
        }
        foreach (int threshold in weaponThresholds.Keys)
        {
            if (playerStats.xp >= threshold && !spawnedThresholds.Contains(threshold))
            {
                Vector3 spawnPoint = generateRandomSpawnPoint(offsetY: 1);
                Instantiate(weaponThresholds[threshold], spawnPoint, Quaternion.identity);
                spawnedThresholds.Add(threshold);
            }
        }
    }
}
}
