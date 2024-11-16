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
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private int damage = 10;
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
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar.value = health;
        healthBar.maxValue = health;

        droneBody = GameObject.FindGameObjectWithTag("DroneBody");
        droneWeapon = GameObject.FindGameObjectWithTag("DroneWeapon");
    }

    // Update is called once per frame
    void Update()
    {
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

        if (isKilled())
        {
            Rigidbody2D bodyRb = droneBody.GetComponent<Rigidbody2D>();
            Rigidbody2D weaponRb = droneWeapon.GetComponent<Rigidbody2D>();


            Vector2 bodyDirection = (transform.position - player.transform.position).normalized;
            Vector2 weaponDirection = (droneWeapon.transform.position - transform.position).normalized;

       
            bodyRb.AddForce(bodyDirection * explosionForce);
            weaponRb.AddForce(weaponDirection * explosionForce);

            if (healthBar != null){
                Destroy(healthBar.gameObject);
            }

            Destroy(gameObject, destroyedTime);       
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
}
