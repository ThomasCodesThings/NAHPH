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
    private float triggerRadius = 10f;
    private int damage = 2;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float lastShotTime = 0f;
    private float shotDelay = 0.5f;
    private float bulletOffset = 0.4f;
    private float bulletSpeed = 7f;
    private float bulletLifeTime = 5f;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject droneBody;
    private GameObject droneWeapon;
    private float destroyedTime = 3f;
    private float explosionForce = 10f;
    private bool playerIsOnLeft = false;

    private int minDistance = 2;

    public void setHealth(int damage)
    {
        health -= damage;
    }

    public bool isKilled(){
        return health <= 0;
    }

    public int getXP(){
        return Random.Range(100, 200);
    }

    public int getDamage()
    {
        return damage;
    }

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

    }

    // Update is called once per frame
    void Update()
    {
        //IgnoreCollision();

        if (Time.timeScale == 0)
        {
            return;
        }

        playerIsOnLeft = player.transform.position.x < transform.position.x;
        if (playerIsOnLeft)
        {
            droneBody.GetComponent<SpriteRenderer>().flipX = true;
            droneWeapon.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            droneBody.GetComponent<SpriteRenderer>().flipX = false;
            droneWeapon.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (healthBar != null){
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * healthBarOffset);
        healthBar.value = health;
        healthBar.fillRect.GetComponent<Image>().color = Color.Lerp(minHealthColor, maxHealthColor, healthBar.normalizedValue);
        }

        int width = gameManager.GetComponent<GameManager>().getBaseWidth();
        int distance = (int)Vector3.Distance(transform.position, player.transform.position);
        if (distance < minDistance)
        {
            return;
        }
        (int newX, int newY) = gameManager.GetComponent<GameManager>().getNextBlock(transform.position.x, transform.position.y);
        Vector3 target = new Vector3(newX - width, newY, 0);
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smooth);

        if (Time.time - lastShotTime > shotDelay)
        {
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

    private void OnDestroy(){
  
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
