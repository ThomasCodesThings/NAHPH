using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeakEnemyScript : MonoBehaviour
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

    [SerializeField] float jumpForce = 10f;
    [SerializeField] float speed = 5f;

    [SerializeField] Slider healthBar;
    [SerializeField] Color minHealthColor = Color.red;
    [SerializeField] Color maxHealthColor = Color.green;
    private float healthBarOffset = 0.75f;

    private int damage = 10;
    private float lastShotTime = 0f;
    private float shotDelay = 0.5f;
    private float bulletOffset = 0.4f;
    private float bulletSpeed = 10f;
    private float bulletLifeTime = 5f;
    [SerializeField] private GameObject bulletPrefab;



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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        heights = gameManager.GetComponent<GameManager>().getHeights();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar.value = health;
        healthBar.maxValue = health;
        player = GameObject.FindGameObjectWithTag("Player");
       
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * healthBarOffset);
        healthBar.value = health;
        healthBar.fillRect.GetComponent<Image>().color = Color.Lerp(minHealthColor, maxHealthColor, healthBar.normalizedValue);
        playerX = (int)player.transform.position.x;
        playerY = (int)player.transform.position.y;
        enemyX = (int)transform.position.x;
        enemyY = (int)transform.position.y;

        int index = Mathf.Clamp(enemyX + gameManager.GetComponent<GameManager>().getBaseWidth(), 0, heights.Count - 1);

        float distance = Vector2.Distance(new Vector2(playerX, playerY), new Vector2(enemyX, enemyY));
        if (distance < triggerRadius)
        {
            bool isPlayerOnLeft = playerX < enemyX;
            if (playerY - 1 == enemyY)
            {
                shoot(isPlayerOnLeft);
            }
            else
            {
                if (isPlayerOnLeft)
                {
                    if (index - 1 >= 0 && heights[index - 1] > enemyY && isGrounded)
                    {
                        jump(isPlayerOnLeft);
                    }
                    else
                    {
                        walk(isPlayerOnLeft); 
                    }
                }
                else
                {
                    if (index + 1 < heights.Count && heights[index + 1] > enemyY && isGrounded)
                    {
                        jump(isPlayerOnLeft); 
                    }else
                    {
                        walk(isPlayerOnLeft); 
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

        // Adjust bullet spawn position based on direction
        Vector3 bulletSpawnPosition = transform.position + Vector3.right * bulletOffset * direction;
        
        // Instantiate bullet and set velocity
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * bulletSpeed, 0);

        // Destroy bullet after its lifetime expires
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
