using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructs;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 5;
    [SerializeField] GameObject pauseMenu;

    public float Move;
    public Rigidbody2D rb;
    private bool isGrounded;
    private int baseDamage = 10;
    private int health = 100;
    private int maxHealth = 100;
    private int xp = 0;
    private float timeToHeal = 5.0f;  // Healing duration
    private float healingTimer = 0.0f; // Timer for healing process
    private bool isHealing = false; // Flag to check if the player is healing
    private List<GameObject> medkits = new List<GameObject>();

    public int getDamage()
    {
        return baseDamage;
    }

    public void setHealth(int damage)
    {
        health -= damage;
    }

    public int heal()
    {
        if (health < maxHealth && medkits.Count > 0) // Check if there's a medkit and if healing is needed
        {
            GameObject medkit = medkits[0];
            //health += medkit.getHealAmount();

            if (health > maxHealth)
            {
                health = maxHealth;
            }
            medkits.RemoveAt(0);
        }
        return health;
    }

    public PlayerStats getPlayerStats()
    {
        return new PlayerStats(health, maxHealth, xp, 0, 0, "None", medkits.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If E key is pressed and player is not healing, start healing
        if (Input.GetKeyDown(KeyCode.E) && !isHealing && health < maxHealth && medkits.Count > 0)
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

            medkits.Add(other.gameObject);
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
