using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructs;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 100;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Animator animator;

    public float Move;
    public Rigidbody2D rb;
    private bool isGrounded;
    private int health = 100;
    private int maxHealth = 100;
    private int xp = 0;
    private int medkitsUsed = 0;
    private int enemiesKilled = 0;
    private float timeToHeal = 5.0f;
    private float healingTimer = 0.0f; 
    private bool isHealing = false; 
    private List<int> medkitsHealingAmount = new List<int>();
    private GameObject rangeWeapon;
    private GameObject meleeWeapon;
    private GameObject currentWeapon;
    private string currentWeaponName = "Basic Pistol";
    private GameObject playerBody;
    private float pixelsPerUnit = 1000.0f;
    private Vector2 pivotPoint = new Vector2(0.5f, 0.5f);
    private string[] weaponUpgrades = new string[] { "Smg", "Shotgun", "Laser Gun", "Plasma Cannon" };
    private GameObject gameManager;
    private RangeWeapon currentRangeWeapon;

    private float lastMeleeAttack = 0.0f;
    private float meleeAttackDelay = 0.4f;
    private int meleeDamage = 20;
    private float minHitDistance = 3f;

     /************************************************************************
     * 
     *  Player body sprites
     * 
     * *********************************************************************/

    [SerializeField] Texture2D pistolBodyTexture;
    [SerializeField] Texture2D smgBodyTexture;
    [SerializeField] Texture2D shotgunBodyTexture;
    [SerializeField] Texture2D laserBodyTexture;
    [SerializeField] Texture2D plasmaCannonBodyTexture;

    [SerializeField] Animator meleeWeaponAnimator;

    GameObject audioManager;

    // Helper function to find a child object with a specific tag
    public GameObject FindChildWithTag(GameObject parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    // Get the current range weapon
    public RangeWeapon getCurrentRangeWeapon()
    {
        return currentRangeWeapon;
    }


    public GameObject getMeleeWeapon()
    {
        return meleeWeapon;
    }

    // Get the current range weapon damage
    public int getDamage()
    {
        return currentRangeWeapon.getDamage();
    }

    // Set the health of the player
    public void setHealth(int damage)
    {
       health -= damage;
    }


    // Check if the player is killed
    public bool isKilled()
    {
        return health <= 0;
    }

    // Add medkits used by the player
    public void addMedkitsUsed()
    {
        gameManager.GetComponent<GameManager>().addMedkitsUsed();
    }

    // Heal the player
    public int heal()
    {
        if (medkitsHealingAmount.Count > 0)
        {
            int healAmount = medkitsHealingAmount[0];
            health += healAmount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            addMedkitsUsed();
            medkitsHealingAmount.RemoveAt(0);
        }
        
        return health;
    }

    // Get the player stats
    public PlayerStats getPlayerStats()
    {
        return new PlayerStats(health, maxHealth, gameManager.GetComponent<GameManager>().getPlayerXP(), currentRangeWeapon.getAmmo(), currentRangeWeapon.getMagazine(), currentRangeWeapon.getName(), medkitsHealingAmount.Count, gameManager.GetComponent<GameManager>().getPlayerMedkitsUsed(), gameManager.GetComponent<GameManager>().getPlayerEnemiesKilled());
    }


    // Add XP to the player
    public void addXP(int xp)
    {
        gameManager.GetComponent<GameManager>().addXP(xp);
    }

    // Add kill to the player
    public void addKill()
    {
        gameManager.GetComponent<GameManager>().addKill();
    }

    // Get the current weapon name
    public string getCurrentWeaponName()
    {
        return currentWeaponName;
    }

    // Set the current animations based on the current weapon name
    private void setAnimationState(){
        switch(currentWeaponName){
            case "Basic Pistol":
                meleeWeaponAnimator.SetBool("HasBasicPistol", true);
                meleeWeaponAnimator.SetBool("HasSmg", false);
                meleeWeaponAnimator.SetBool("HasShotgun", false);
                meleeWeaponAnimator.SetBool("HasLaserGun", false);
                meleeWeaponAnimator.SetBool("HasPlasmaCannon", false);
                meleeWeaponAnimator.SetBool("OnMeleeAttack", false);
                break;
                
            case "Smg":
                meleeWeaponAnimator.SetBool("HasBasicPistol", false);
                meleeWeaponAnimator.SetBool("HasSmg", true);
                meleeWeaponAnimator.SetBool("HasShotgun", false);
                meleeWeaponAnimator.SetBool("HasLaserGun", false);
                meleeWeaponAnimator.SetBool("HasPlasmaCannon", false);
                meleeWeaponAnimator.SetBool("OnMeleeAttack", false);
                break;

            case "Shotgun":
                meleeWeaponAnimator.SetBool("HasBasicPistol", false);
                meleeWeaponAnimator.SetBool("HasSmg", false);
                meleeWeaponAnimator.SetBool("HasShotgun", true);
                meleeWeaponAnimator.SetBool("HasLaserGun", false);
                meleeWeaponAnimator.SetBool("HasPlasmaCannon", false);
                meleeWeaponAnimator.SetBool("OnMeleeAttack", false);
                break;

            case "Laser Gun":
                meleeWeaponAnimator.SetBool("HasBasicPistol", false);
                meleeWeaponAnimator.SetBool("HasSmg", false);
                meleeWeaponAnimator.SetBool("HasShotgun", false);
                meleeWeaponAnimator.SetBool("HasLaserGun", true);
                meleeWeaponAnimator.SetBool("HasPlasmaCannon", false);
                meleeWeaponAnimator.SetBool("OnMeleeAttack", false);
                break;

            case "Plasma Cannon":
                meleeWeaponAnimator.SetBool("HasBasicPistol", false);
                meleeWeaponAnimator.SetBool("HasSmg", false);
                meleeWeaponAnimator.SetBool("HasShotgun", false);
                meleeWeaponAnimator.SetBool("HasLaserGun", false);
                meleeWeaponAnimator.SetBool("HasPlasmaCannon", true);
                meleeWeaponAnimator.SetBool("OnMeleeAttack", false);
                break;           
        }
    }

    // Player melee attack
    private void attackEnemyWithMelee()
    {
        bool isFacingRight = transform.rotation.eulerAngles.y == 0;
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Soldier");
        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");

        foreach(GameObject soldier in soldiers){
            if(soldier == null){
                continue;
            }
            float distance = Vector2.Distance(transform.position, soldier.transform.position);
            if(distance < minHitDistance){ // If the player is close enough to the enemy, hit the enemy
                soldier.GetComponent<SoldierScript>().setHealth(meleeDamage);
                soldier.GetComponent<SoldierScript>().slowDown();
                if(soldier.GetComponent<SoldierScript>().isKilled()){
                    addXP(soldier.GetComponent<SoldierScript>().getXP());
                    addKill();
                }
            }
        }

        // Same thing goes for drones and bosses
        foreach(GameObject drone in drones){
            if(drone == null){
                continue;
            }
            float distance = Vector2.Distance(transform.position, drone.transform.position);
            if(distance < minHitDistance){
                drone.GetComponent<DroneScript>().setHealth(meleeDamage);
                if(drone.GetComponent<DroneScript>().isKilled()){
                    addXP(drone.GetComponent<DroneScript>().getXP());
                    addKill();
                }
            }
        }

        foreach(GameObject boss in bosses){
            if(boss == null){
                continue;
            }
            float distance = Vector2.Distance(transform.position, boss.transform.position);
            if(distance < minHitDistance){
                boss.GetComponent<BossScript>().setHealth(meleeDamage);
                if(boss.GetComponent<BossScript>().isKilled()){
                    addXP(boss.GetComponent<BossScript>().getXP());
                    addKill();
                }
            }
        }
        //meleeWeaponAnimator.SetBool("OnMeleeAttack", true);
    }


    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        rangeWeapon = GameObject.FindGameObjectWithTag("PlayerRangeWeapon");
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        meleeWeaponAnimator = playerBody.GetComponent<Animator>();

        currentRangeWeapon = gameManager.GetComponent<GameManager>().getCurrentWeapon();

        audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        
    }

    // Update is called once per frame
    void Update()
    {

        if(gameObject.transform.position.y < 0) // If the player falls off the map, set its health to 0
        {
            health = 0;
        }

        if (Input.GetKeyDown(KeyCode.E) && !isHealing && medkitsHealingAmount.Count > 0) // If the player presses E and has medkits, start healing, it takes around 2 seconds to heal
        {
            StartHealing();
        }

       
        if (isHealing)
        {
            healingTimer += Time.deltaTime;
            if (healingTimer >= timeToHeal)
            {
                EndHealing();
            }
            return; 
        }

     
        if (Time.timeScale == 0) // If the game is paused do nothing
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;

        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y - 0.5f);


        float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;

        
        //Rotate the player and his body based on the mouse position
        if (mousePosition.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            playerBody.transform.rotation = Quaternion.Euler(0, 180, angle);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            playerBody.transform.rotation = Quaternion.Euler(0, 0, angle);
        }


        Move = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(Move));
        animator.SetBool("IsJumping", !isGrounded);
        rb.velocity = new Vector2(Move * speed, rb.velocity.y); // Move the player
        if (Input.GetButtonDown("Jump") && isGrounded) // Player jump
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
            audioManager.GetComponent<AudioScript>().playPlayerJump();
        }


        // Player melee attack
        if(Input.GetMouseButtonDown(1) && Time.time - lastMeleeAttack > meleeAttackDelay)
        {
            lastMeleeAttack = Time.time;
            meleeWeaponAnimator.SetBool("OnMeleeAttack", true);
            meleeWeaponAnimator.SetBool("HasBasicPistol", false);
            meleeWeaponAnimator.SetBool("HasSmg", false);
            meleeWeaponAnimator.SetBool("HasShotgun", false);
            meleeWeaponAnimator.SetBool("HasLaserGun", false);
            meleeWeaponAnimator.SetBool("HasPlasmaCannon", false);
            audioManager.GetComponent<AudioScript>().playMeleeAttack();
            attackEnemyWithMelee();
        }

        if(Input.GetMouseButtonUp(1))
        {
            //meleeWeaponAnimator.SetBool("OnMeleeAttack", false);
            setAnimationState();
        }

       

        if (Input.GetKeyDown(KeyCode.Escape)) // Pause the game
        {
            if (Time.timeScale == 1)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

        if (other.gameObject.CompareTag("Medkit"))
        {

            medkitsHealingAmount.Add(other.gameObject.GetComponent<MedkitScript>().getHealAmount());
            audioManager.GetComponent<AudioScript>().playMedkitPickup();
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("AmmoPack")){
            int ammo = other.gameObject.GetComponent<AmmoPackScript>().getAmmoAmount();
            audioManager.GetComponent<AudioScript>().playAmmoPickup();
            currentRangeWeapon.addAmmo(ammo);
            Destroy(other.gameObject);
        }


    for (int i = 0; i < weaponUpgrades.Length; i++)
    {
        string weapon = weaponUpgrades[i];
        string weaponTag = weapon.Replace(" ", "");
        if(other.gameObject.CompareTag(weaponTag))
        {
            gameManager.GetComponent<GameManager>().setCurrentWeapon(weapon);
            currentRangeWeapon = gameManager.GetComponent<GameManager>().getCurrentWeapon();
            currentWeaponName = weapon;

            setAnimationState();
            audioManager.GetComponent<AudioScript>().playPlayerWeaponPickup();

            switch(weapon){
                case "Smg":
                    playerBody.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
                        smgBodyTexture,
                        new Rect(0.0f, 0.0f, smgBodyTexture.width, smgBodyTexture.height),
                        pivotPoint,
                        pixelsPerUnit
                    );
                    break;
                case "Shotgun":
                    playerBody.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
                        shotgunBodyTexture,
                        new Rect(0.0f, 0.0f, shotgunBodyTexture.width, shotgunBodyTexture.height),
                        pivotPoint,
                        pixelsPerUnit
                    );
                    break;
                case "Laser Gun":
                    playerBody.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
                        laserBodyTexture,
                        new Rect(0.0f, 0.0f, laserBodyTexture.width, laserBodyTexture.height),
                        pivotPoint,
                        pixelsPerUnit
                    );
                    break;
                case "Plasma Cannon":
                    playerBody.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
                        plasmaCannonBodyTexture,
                        new Rect(0.0f, 0.0f, plasmaCannonBodyTexture.width, plasmaCannonBodyTexture.height),
                        pivotPoint,
                        pixelsPerUnit
                    );
                    break;
            }
            
            //other.gameObject.SetActive(false);
            Destroy(other.gameObject); 
        }
    }

    if(other.gameObject.CompareTag("EnergyProjectile")){
        GameObject drone = GameObject.FindGameObjectWithTag("Drone");
        if(drone == null){
            return;
        }
        int damage = drone.GetComponent<DroneScript>().getDamage();
        //Debug.Log("Player hit by enemy bullet. Damage: " + damage);
        setHealth(damage);
        Destroy(other.gameObject);
    }

    if(other.gameObject.CompareTag("EnergyProjectileBoss")){
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if(boss == null){
            return;
        }
        int damage = boss.GetComponent<BossScript>().getDamage();
        //Debug.Log("Player hit by enemy bullet. Damage: " + damage);
        setHealth(damage);
        Destroy(other.gameObject);
    }

    if(other.gameObject.CompareTag("BallisticProjectile")){
        GameObject soldier = GameObject.FindGameObjectWithTag("Soldier");
        int damage = soldier.GetComponent<SoldierScript>().getDamage();
        //Debug.Log("Player hit by enemy bullet. Damage: " + damage);
        setHealth(damage);
        Destroy(other.gameObject);
    }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    public void PauseGame() // Pause the sounds
    {
        Time.timeScale = 0;
        audioManager.GetComponent<AudioScript>().musicSource.Pause();
        audioManager.GetComponent<AudioScript>().sfxSource.Pause();
        pauseMenu.SetActive(true);
    }

    public void ResumeGame() // Resume the sounds
    {
        Time.timeScale = 1;
        audioManager.GetComponent<AudioScript>().musicSource.Play();
        audioManager.GetComponent<AudioScript>().sfxSource.Play();
        pauseMenu.SetActive(false);
    }

    private void StartHealing() // Start healing
    {
        isHealing = true;
        audioManager.GetComponent<AudioScript>().playPlayerHeal();
        healingTimer = 0.0f;
    }

    private void EndHealing() // End healing
    {
        heal(); 
        isHealing = false; 
        healingTimer = 0.0f; 
    }
}
