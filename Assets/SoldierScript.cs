using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierScript : MonoBehaviour
{
    private GameObject gameManager;
    private GameObject player;
    private List<int> heights;
    private int playerX;
    private int playerY;
    private int enemyX;
    private int enemyY;

    private int health = 50;
    private float triggerRadius = 10f;
    private bool isGrounded = true;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;


    [SerializeField] float jumpForce = 10f;
    [SerializeField] float speed = 2.5f;

    [SerializeField] Slider healthBar;
    [SerializeField] Color minHealthColor = Color.red;
    [SerializeField] Color maxHealthColor = Color.green;
    private float healthBarOffset = 0.75f;

    private int damage = 1;
    private float lastShotTime = 0f;
    private float shotDelay = 0.5f;
    private float bulletOffsetX = 1f;
    private float bulletOffsetY = 0.5f;
    private float bulletSpeed = 7f;
    private float bulletLifeTime = 5f;
    [SerializeField] private GameObject bulletPrefab;
    private bool canMove = true;


    public void setHealth(int damage)
    {
        health -= damage;
        
    }

    public bool isKilled(){
        return health <= 0;
    }

    public int getXP(){
        return Random.Range(50, 100);
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
        animator = GetComponent<Animator>();
        heights = gameManager.GetComponent<GameManager>().getHeights();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar.value = health;
        healthBar.maxValue = health;
        player = GameObject.FindGameObjectWithTag("Player");

        IgnoreCollision();

    }

    // Update is called once per frame
    void Update()
    {

    if(gameObject.transform.position.y < 0){
        health = 0;
    }

    //IgnoreCollision();

    if (Time.timeScale == 0)
    {
        return;
    }


        float directionMultiplier = transform.localScale.x > 0 ? 1 : -1; // Flip offset based on character's facing direction
        Vector3 healthBarWorldPosition = new Vector3(
        transform.position.x + directionMultiplier * -0.1f, // Adjust for facing direction
        transform.position.y + 2f * healthBarOffset, 
        transform.position.z   
    );

        healthBar.transform.position = Camera.main.WorldToScreenPoint(healthBarWorldPosition);
        healthBar.value = health;
        healthBar.fillRect.GetComponent<Image>().color = Color.Lerp(minHealthColor, maxHealthColor, healthBar.normalizedValue);
        playerX = (int)player.transform.position.x;
        playerY = (int)player.transform.position.y;
        enemyX = (int)transform.position.x;
        enemyY = (int)transform.position.y;

        if(playerX < enemyX){
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }else{
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        int index = Mathf.Clamp(enemyX + gameManager.GetComponent<GameManager>().getBaseWidth(), 0, heights.Count - 1);

        float distance = Vector2.Distance(new Vector2(playerX, playerY), new Vector2(enemyX, enemyY));
        if (distance < triggerRadius)
        {
            bool isPlayerOnLeft = playerX < enemyX;
            if (playerY == enemyY + 1)
            {
                shoot(isPlayerOnLeft);
                animator.SetBool("IsWalking", false);
            }
            else
            {
                if(distance < 2){
                    return;
                }
                else
                if (isPlayerOnLeft)
                {
                    if (index - 1 >= 0 && heights[index - 1] > enemyY && isGrounded)
                    {
                        jump(isPlayerOnLeft);
                        animator.SetBool("IsWalking", false);
                    }
                    else
                    {
                        walk(isPlayerOnLeft);
                        animator.SetBool("IsWalking", true);
                    }
                }
                else
                {
                    if (index + 1 < heights.Count && heights[index + 1] > enemyY && isGrounded)
                    {
                        jump(isPlayerOnLeft);
                        animator.SetBool("IsWalking", false);
                    }else
                    {
                        walk(isPlayerOnLeft);
                        animator.SetBool("IsWalking", true);
                    }
                }
            }
        }
    }
    

     private void walk(bool isPlayerOnLeft)
    {
        float direction = isPlayerOnLeft ? -1 : 1;
        transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);


        transform.position += Vector3.right * direction * speed * Time.deltaTime;
    }

    private void jump(bool isPlayerOnLeft)
    {
        if (isGrounded)
        {
            float direction = isPlayerOnLeft ? -1 : 1;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            
        
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);

            isGrounded = false;
        }
    }

    private void shoot(bool isPlayerOnLeft)
{
    if (Time.time - lastShotTime > shotDelay)
    {
        lastShotTime = Time.time;
        float direction = isPlayerOnLeft ? -1 : 1;


        Vector3 bulletSpawnPosition = (gameObject.transform.position + new Vector3(direction * bulletOffsetX, bulletOffsetY, 0));
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * bulletSpeed, 0);

        Destroy(bullet, bulletLifeTime);
    }
}

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            
            int damage = player.GetComponent<PlayerScript>().getDamage();
            setHealth(damage);
            Destroy(other.gameObject);
        }
    }

    
}
