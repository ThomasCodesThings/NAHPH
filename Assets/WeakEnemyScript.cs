using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEnemyScript : MonoBehaviour
{
    [SerializeField] ProceduralGeneration proceduralGeneration;
    [SerializeField] GameObject player;
    private List<int> heights;
    private int playerX;
    private int playerY;
    private int enemyX;
    private int enemyY;

    private int health = 50;
    private float triggerRadius = 10f;
    private bool isGrounded = true;
    private Rigidbody2D rb;

    [SerializeField] float jumpForce = 10f;
    [SerializeField] float speed = 5f;

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
    }

    // Update is called once per frame
    void Update()
    {
        playerX = (int)player.transform.position.x;
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
                    else
                    {
                        walk(isPlayerOnLeft); 
                    }
                }
            }
        }
    }

     private void walk(bool isPlayerOnLeft)
    {
        if (isPlayerOnLeft)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
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
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
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
 
        if (other.gameObject.CompareTag("Grass"))
        {
            isGrounded = true;
        }
    }
}
