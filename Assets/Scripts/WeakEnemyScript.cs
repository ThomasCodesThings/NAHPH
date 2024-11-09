using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeakEnemyScript : MonoBehaviour
{
    [SerializeField] GameManager proceduralGeneration;
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

    public void setHealth(int damage)
    {
        health -= damage;
        /*if (health <= 0)
        {
            Destroy(gameObject);
        }*/
    }

    public bool isKilled(){
        return health <= 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (proceduralGeneration != null)
        {
            heights = proceduralGeneration.getHeights();
        }
        else
        {
            Debug.LogError("ProceduralGeneration object not assigned!");
        }

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
        /*playerX = (int)player.transform.position.x;
        playerY = (int)player.transform.position.y;
        enemyX = (int)transform.position.x;
        enemyY = (int)transform.position.y;

        int index = Mathf.Clamp(enemyX + proceduralGeneration.getBaseWidth(), 0, heights.Count - 1);

        float distance = Vector2.Distance(new Vector2(playerX, playerY), new Vector2(enemyX, enemyY));
        if (distance < triggerRadius)
        {
            bool isPlayerOnLeft = playerX < enemyX;
            if (playerY == enemyY)
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
        }*/
    }
    

     private void walk(bool isPlayerOnLeft)
    {
        if (isPlayerOnLeft)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            spriteRenderer.flipX = true;
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            spriteRenderer.flipX = false;
        }
    }

    private void jump(bool isPlayerOnLeft)
    {
        if (isGrounded)
        {
           rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            if (isPlayerOnLeft)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                spriteRenderer.flipX = true;
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                spriteRenderer.flipX = false;
            }

            isGrounded = false;
        }
    }

    private void shoot(bool isPlayerOnLeft)
    {
        Debug.Log("Shoot");
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
