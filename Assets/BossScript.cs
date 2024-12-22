using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    private GameObject gameManager;
    private GameObject player;
    private float smooth = 5.0f;
    [SerializeField] Slider healthBar;
    [SerializeField] Color minHealthColor = Color.red;
    [SerializeField] Color maxHealthColor = Color.green;
    [SerializeField] Animator animator;
    private float healthBarOffset = 0.5f;

    private int health = 500;
    private float triggerRadius = 10f;
    private int damage = 0;
    private SpriteRenderer spriteRenderer;

    private float lastShotTime = 0f;
    private float shotDelay = 0.2f;
    private float bulletOffset = 0.4f;
    private float bulletSpeed = 15f;
    private float bulletLifeTime = 5f;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject bossBody;
    private GameObject bossWeapon;
    private float destroyedTime = 3f;
    private float explosionForce = 10f;
    private bool playerIsOnLeft = false;
    private GameObject audioManager;

    public void setHealth(int damage)
    {
        health -= damage;
    }

    public bool isKilled(){
        return health <= 0;
    }

    public int getXP(){
        return Random.Range(500, 1000);
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
        GameObject[] bosss = GameObject.FindGameObjectsWithTag("boss");
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

        foreach(GameObject boss in bosss){
            if(boss == null){
                continue;
            }
            Physics2D.IgnoreCollision(boss.GetComponent<Collider2D>(), GetComponent<Collider2D>());
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

    bossBody = GameObject.FindGameObjectWithTag("BossBody");
    if (bossBody == null)
    {
        Debug.LogError("bossBody not found in the scene.");
    }

    bossWeapon = GameObject.FindGameObjectWithTag("BossWeapon");
    if (bossWeapon == null)
    {
        Debug.LogError("bossWeapon not found in the scene.");
    }

    audioManager = GameObject.FindGameObjectWithTag("AudioManager");
    IgnoreCollision();
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
            bossBody.GetComponent<SpriteRenderer>().flipX = false;
            bossWeapon.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            bossBody.GetComponent<SpriteRenderer>().flipX = true;
            bossWeapon.GetComponent<SpriteRenderer>().flipX = true;
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

        animator.SetBool("IsFiring", Time.time - lastShotTime < shotDelay);

        if (Time.time - lastShotTime > shotDelay)
        {
            audioManager.GetComponent<AudioScript>().playBossAttack();
            lastShotTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, bossWeapon.transform.position + bossWeapon.transform.right * bulletOffset, bossWeapon.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bossWeapon.transform.right * bulletSpeed;
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
  
            Rigidbody2D bodyRb = bossBody.GetComponent<Rigidbody2D>();
            Rigidbody2D weaponRb = bossWeapon.GetComponent<Rigidbody2D>();


            Vector2 bodyDirection = (transform.position - player.transform.position).normalized;
            Vector2 weaponDirection = (bossWeapon.transform.position - transform.position).normalized;

       
            bodyRb.AddForce(bodyDirection * explosionForce);
            weaponRb.AddForce(weaponDirection * explosionForce);

            if (healthBar != null){
                Destroy(healthBar.gameObject);
            }

            if(bossBody != null){
                Destroy(bossBody);
            }

            if(bossWeapon != null){
                Destroy(bossWeapon);
            }

            if(gameObject != null){
                Destroy(gameObject, destroyedTime);
            }
            
    }
}
