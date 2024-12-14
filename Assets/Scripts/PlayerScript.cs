using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructs;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 100;
    [SerializeField] GameObject pauseMenu;

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
    private string[] weaponUpgrades = new string[] { "Smg", "Shotgun", "Laser", "Plasma Cannon" };

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

    public GameObject getRangeWeapon()
    {
        return rangeWeapon;
    }

    public GameObject getMeleeWeapon()
    {
        return meleeWeapon;
    }

    public int getDamage()
    {
        switch(currentWeaponName){
            case "Basic Pistol":
                return rangeWeapon.GetComponent<GunScript>().getDamage();
            case "Smg":
                return rangeWeapon.GetComponent<SmgScript>().getDamage();
            case "Shotgun":
                return rangeWeapon.GetComponent<ShotgunScript>().getDamage();
            case "Laser":
                return rangeWeapon.GetComponent<LaserScript>().getDamage();
            case "Plasma Cannon":
                return rangeWeapon.GetComponent<PlasmaCannonScript>().getDamage();
            default:
                return rangeWeapon.GetComponent<GunScript>().getDamage();
        }
    }

    public void setHealth(int damage)
    {
       health -= damage;
    }

    public bool isKilled()
    {
        return health <= 0;
    }

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
            medkitsUsed++;
            medkitsHealingAmount.RemoveAt(0);
        }
        
        return health;
    }

    public PlayerStats getPlayerStats()
    {
        switch(currentWeaponName){
            case "Basic Pistol":
               return new PlayerStats(health, maxHealth, xp, rangeWeapon.GetComponent<GunScript>().getAmmo(), rangeWeapon.GetComponent<GunScript>().getMagazine(), rangeWeapon.GetComponent<GunScript>().getName(), medkitsHealingAmount.Count, medkitsUsed, enemiesKilled);
            case "Smg":
                return new PlayerStats(health, maxHealth, xp, rangeWeapon.GetComponent<SmgScript>().getAmmo(), rangeWeapon.GetComponent<SmgScript>().getMagazine(), rangeWeapon.GetComponent<SmgScript>().getName(), medkitsHealingAmount.Count, medkitsUsed, enemiesKilled);
            case "Shotgun":
                return new PlayerStats(health, maxHealth, xp, rangeWeapon.GetComponent<ShotgunScript>().getAmmo(), rangeWeapon.GetComponent<ShotgunScript>().getMagazine(), rangeWeapon.GetComponent<ShotgunScript>().getName(), medkitsHealingAmount.Count, medkitsUsed, enemiesKilled);
            case "Laser":
                return new PlayerStats(health, maxHealth, xp, rangeWeapon.GetComponent<LaserScript>().getAmmo(), rangeWeapon.GetComponent<LaserScript>().getMagazine(), rangeWeapon.GetComponent<LaserScript>().getName(), medkitsHealingAmount.Count, medkitsUsed, enemiesKilled);
            case "Plasma Cannon":
                return new PlayerStats(health, maxHealth, xp, rangeWeapon.GetComponent<PlasmaCannonScript>().getAmmo(), rangeWeapon.GetComponent<PlasmaCannonScript>().getMagazine(), rangeWeapon.GetComponent<PlasmaCannonScript>().getName(), medkitsHealingAmount.Count, medkitsUsed, enemiesKilled);
            default:
                return new PlayerStats(health, maxHealth, xp, rangeWeapon.GetComponent<GunScript>().getAmmo(), rangeWeapon.GetComponent<GunScript>().getMagazine(), rangeWeapon.GetComponent<GunScript>().getName(), medkitsHealingAmount.Count, medkitsUsed, enemiesKilled);
        }
    }

    public void addXP(int xp)
    {
        this.xp += xp;
    }

    public void addKill()
    {
        enemiesKilled++;
    }

    public int getXp()
    {
        return xp;
    }

    public int getKills()
    {
        return enemiesKilled;
    }

    public int getMedkitsUsed()
    {
        return medkitsUsed;
    }

    public string getCurrentWeaponName()
    {
        return currentWeaponName;
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        rangeWeapon = GameObject.FindGameObjectWithTag("PlayerRangeWeapon");
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody");
        //meleeWeapon = GameObject.FindGameObjectWithTag("PlayerMeleeWeapon");

        currentWeapon = rangeWeapon;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(gameObject.transform.position.y < -10)
        {
            health = 0;
        }

        if (Input.GetKeyDown(KeyCode.E) && !isHealing && medkitsHealingAmount.Count > 0)
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

     
        if (Time.timeScale == 0)
        {
            return;
        }

        Move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(Move * speed, rb.velocity.y);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
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
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("AmmoPack")){
            int ammo = other.gameObject.GetComponent<AmmoPackScript>().getAmmoAmount();
            if (currentWeaponName == "Basic Pistol")
            {
                rangeWeapon.GetComponent<GunScript>().addAmmo(ammo);
            }
            else if (currentWeaponName == "Smg")
            {
                rangeWeapon.GetComponent<SmgScript>().addAmmo(ammo);
            }else if (currentWeaponName == "Shotgun")
            {
                rangeWeapon.GetComponent<ShotgunScript>().addAmmo(ammo);
            }

            Destroy(other.gameObject);
        }


    for (int i = 0; i < weaponUpgrades.Length; i++)
    {
        string weapon = weaponUpgrades[i];
        string weaponTag = weapon.Replace(" ", "");
        if(other.gameObject.CompareTag(weaponTag))
        {
            /*Transform weaponHolder = transform.GetChild(1);
            Vector3 previousPosition = rangeWeapon.transform.localPosition;
            Quaternion previousRotation = rangeWeapon.transform.localRotation;

            if (rangeWeapon != null)
            {
                Destroy(rangeWeapon);
            }
            rangeWeapon = Instantiate(other.gameObject);

            rangeWeapon.transform.SetParent(weaponHolder);

            rangeWeapon.transform.localPosition = previousPosition;
            rangeWeapon.transform.localRotation = previousRotation;*/
            //GameObject copy = other.gameObject;
            rangeWeapon = other.gameObject;
            currentWeapon = rangeWeapon;
            currentWeaponName = weapon;

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
                case "Laser":
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
            
            other.gameObject.SetActive(false);
            //Destroy(other.gameObject); 
        }
    }
        /*if(other.gameObject.CompareTag("Smg"))
    {

        Transform weaponHolder = transform.GetChild(1);
        Vector3 previousPosition = rangeWeapon.transform.localPosition;
        Quaternion previousRotation = rangeWeapon.transform.localRotation;

        if (rangeWeapon != null)
        {
            Destroy(rangeWeapon);
        }
        rangeWeapon = Instantiate(other.gameObject);

        rangeWeapon.transform.SetParent(weaponHolder);

        rangeWeapon.transform.localPosition = previousPosition;
        rangeWeapon.transform.localRotation = previousRotation;

        currentWeapon = rangeWeapon;
        currentWeaponName = "Smg";

        playerBody.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
            smgBodyTexture,
            new Rect(0.0f, 0.0f, smgBodyTexture.width, smgBodyTexture.height),
            pivotPoint,
            pixelsPerUnit
        );
        
        Destroy(other.gameObject); 
    }


    if(other.gameObject.CompareTag("Shotgun"))
    {
        Transform weaponHolder = transform.GetChild(1);
        Vector3 previousPosition = rangeWeapon.transform.localPosition;
        Quaternion previousRotation = rangeWeapon.transform.localRotation;

        if (rangeWeapon != null)
        {
            Destroy(rangeWeapon);
        }
        rangeWeapon = Instantiate(other.gameObject);

        rangeWeapon.transform.SetParent(weaponHolder);

        rangeWeapon.transform.localPosition = previousPosition;
        rangeWeapon.transform.localRotation = previousRotation;

        currentWeapon = rangeWeapon;
        currentWeaponName = "Shotgun";

        playerBody.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
            shotgunBodyTexture,
            new Rect(0.0f, 0.0f, shotgunBodyTexture.width, shotgunBodyTexture.height),
            pivotPoint,
            pixelsPerUnit
        );
        
        Destroy(other.gameObject); 
    }

    if(other.gameObject.CompareTag("Laser"))
    {
        Transform weaponHolder = transform.GetChild(1);
        Vector3 previousPosition = rangeWeapon.transform.localPosition;
        Quaternion previousRotation = rangeWeapon.transform.localRotation;

        if (rangeWeapon != null)
        {
            Destroy(rangeWeapon);
        }
        rangeWeapon = Instantiate(other.gameObject);

        rangeWeapon.transform.SetParent(weaponHolder);

        rangeWeapon.transform.localPosition = previousPosition;
        rangeWeapon.transform.localRotation = previousRotation;

        currentWeapon = rangeWeapon;
        currentWeaponName = "Laser";

        playerBody.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
            laserBodyTexture,
            new Rect(0.0f, 0.0f, laserBodyTexture.width, laserBodyTexture.height),
            pivotPoint,
            pixelsPerUnit
        );
        
        Destroy(other.gameObject); 
    }

    if(other.gameObject.CompareTag("PlasmaCannon"))
    {
        Transform weaponHolder = transform.GetChild(1);
        Vector3 previousPosition = rangeWeapon.transform.localPosition;
        Quaternion previousRotation = rangeWeapon.transform.localRotation;

        if (rangeWeapon != null)
        {
            Destroy(rangeWeapon);
        }
        rangeWeapon = Instantiate(other.gameObject);

        rangeWeapon.transform.SetParent(weaponHolder);

        rangeWeapon.transform.localPosition = previousPosition;
        rangeWeapon.transform.localRotation = previousRotation;

        currentWeapon = rangeWeapon;
        currentWeaponName = "Plasma Cannon";

        playerBody.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
            plasmaCannonBodyTexture,
            new Rect(0.0f, 0.0f, plasmaCannonBodyTexture.width, plasmaCannonBodyTexture.height),
            pivotPoint,
            pixelsPerUnit
        );
        
        Destroy(other.gameObject); 
    }*/

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

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    private void StartHealing()
    {
        isHealing = true;
        healingTimer = 0.0f;
    }

    private void EndHealing()
    {
        heal(); 
        isHealing = false; 
        healingTimer = 0.0f; 
      
    }
}
