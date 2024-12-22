using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneScript : MonoBehaviour
{
    private GameObject gameManager;
    private GameObject player;
    private float smooth = 5.0f;
    [SerializeField] Slider healthBar;
    [SerializeField] Color minHealthColor = Color.red;
    [SerializeField] Color maxHealthColor = Color.green;
    private float healthBarOffset = 0.5f;

    private int health = 100;
    private int damage = 10;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float lastShotTime = 0f;
    private float shotDelay = 0.35f;
    private float bulletOffset = 0.4f;
    private float bulletSpeed = 10f;
    private float bulletLifeTime = 5f;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject droneBody;
    private GameObject droneWeapon;
    private float destroyedTime = 3f;
    private float explosionForce = 10f;
    private bool playerIsOnLeft = false;
    private GameObject audioManager;

    private int minDistance = 2; // Minimum distance to player

    // Get the health of the drone
    public void setHealth(int damage)
    {
        health -= damage;
    }

    // Get the health of the drone
    public bool isKilled(){
        return health <= 0;
    }

    // Get the XP of the drone
    public int getXP(){
        return Random.Range(100, 200);
    }

    // Get the damage of the drone
    public int getDamage()
    {
        return damage;
    }

    // Ignore collision with other objects
    private void IgnoreCollision()
    {
        GameObject[] medkits = GameObject.FindGameObjectsWithTag("Medkit");
        GameObject[] ammopacks = GameObject.FindGameObjectsWithTag("AmmoPack");
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Soldier");
        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");
        GameObject[] energyProjectiles = GameObject.FindGameObjectsWithTag("EnergyProjectile");
        GameObject[] ballisticProjectiles = GameObject.FindGameObjectsWithTag("BallisticProjectile");

        foreach(GameObject medkit in medkits){
            if(medkit == null){
                continue;
            }
            Physics2D.IgnoreCollision(medkit.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        foreach(GameObject ammopack in ammopacks){
            if(ammopack == null){
                continue;
            }
            Physics2D.IgnoreCollision(ammopack.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }


       foreach(GameObject soldier in soldiers){
            if(soldier == null){
                continue;
            }
            Physics2D.IgnoreCollision(soldier.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        foreach(GameObject drone in drones){
            if(drone == null){
                continue;
            }
            Physics2D.IgnoreCollision(drone.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        foreach(GameObject energyProjectile in energyProjectiles){
            if(energyProjectile == null){
                continue;
            }
            Physics2D.IgnoreCollision(energyProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        foreach(GameObject ballisticProjectile in ballisticProjectiles){
            if(ballisticProjectile == null){
                continue;
            }
            Physics2D.IgnoreCollision(ballisticProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }   
    }
 
    // Start is called before the first frame update
    void Start()
    {
    gameManager = GameObject.FindGameObjectWithTag("GameManager");
    if (gameManager == null)
    {
        Debug.LogError("GameManager not found in the scene.");
    }

    player = GameObject.FindGameObjectWithTag("Player");
    if (player == null)
    {
        Debug.LogError("Player not found in the scene.");
    }

    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();

    // Set the health bar value
    if (healthBar != null)
    {
        healthBar.value = health;
        healthBar.maxValue = health;
    }
    else
    {
        Debug.LogWarning("HealthBar is not assigned in the Inspector.");
    }

    droneBody = GameObject.FindGameObjectWithTag("DroneBody");
    if (droneBody == null)
    {
        Debug.LogError("DroneBody not found in the scene.");
    }

    droneWeapon = GameObject.FindGameObjectWithTag("DroneWeapon");
    if (droneWeapon == null)
    {
        Debug.LogError("DroneWeapon not found in the scene.");
    }

    audioManager = GameObject.FindGameObjectWithTag("AudioManager");

    IgnoreCollision();
    }

    // Update is called once per frame
    void Update()
    {
        //IgnoreCollision();

        if (Time.timeScale == 0) // If the game is paused do nothing
        {
            return;
        }

        // Check if the player is on the left or right side of the drone and base on that flip the sprite
        playerIsOnLeft = player.transform.position.x < transform.position.x;
        if (playerIsOnLeft)
        {
            if (droneBody == null || droneWeapon == null)
            {
                return;
            }
            droneBody.GetComponent<SpriteRenderer>().flipX = true;
            droneWeapon.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            if (droneBody == null || droneWeapon == null)
            {
                return;
            }
            droneBody.GetComponent<SpriteRenderer>().flipX = false;
            droneWeapon.GetComponent<SpriteRenderer>().flipX = false;
        }

        // Update the health bar position, value and color
        if (healthBar != null){
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * healthBarOffset);
        healthBar.value = health;
        healthBar.fillRect.GetComponent<Image>().color = Color.Lerp(minHealthColor, maxHealthColor, healthBar.normalizedValue);
        }

        int width = gameManager.GetComponent<GameManager>().getBaseWidth();
        int distance = (int)Vector3.Distance(transform.position, player.transform.position);
        if (distance < minDistance) // If the player is too close to the drone, the drone does not move
        {
            return;
        }

        // Otherwise it moves to new position
        (int newX, int newY) = gameManager.GetComponent<GameManager>().getNextBlock(transform.position.x, transform.position.y);
        Vector3 target = new Vector3(newX - width, newY, 0);
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smooth);

        // Drone attack
        if (Time.time - lastShotTime > shotDelay)
        {
            audioManager.GetComponent<AudioScript>().playDroneAttack();
            lastShotTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, droneWeapon.transform.position + droneWeapon.transform.right * bulletOffset, droneWeapon.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = droneWeapon.transform.right * bulletSpeed;
            Destroy(bullet, bulletLifeTime);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       if (other.gameObject.CompareTag("Bullet"))
       {
           int damage = player.GetComponent<PlayerScript>().getDamage();
           setHealth(damage);
           Destroy(other.gameObject);
       }
    }

    // Destroy drone parts separately, it worked before but now it does not work
    private void OnDestroy(){

            if(droneBody == null || droneWeapon == null){
                return;
            }

            Rigidbody2D bodyRb = droneBody.GetComponent<Rigidbody2D>();
            Rigidbody2D weaponRb = droneWeapon.GetComponent<Rigidbody2D>();


            Vector2 bodyDirection = (transform.position - player.transform.position).normalized;
            Vector2 weaponDirection = (droneWeapon.transform.position - transform.position).normalized;

       
            bodyRb.AddForce(bodyDirection * explosionForce);
            weaponRb.AddForce(weaponDirection * explosionForce);

            if (healthBar != null){
                Destroy(healthBar.gameObject);
            }

            if(droneBody != null){
                Destroy(droneBody);
            }

            if(droneWeapon != null){
                Destroy(droneWeapon);
            }

            if(gameObject != null){
                Destroy(gameObject, destroyedTime);
            }
            
    }
}
