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
    private int xp = 0;
    private float elapsedTime = 0.0f;
    private float timeToHeal = 5.0f;
    private Medkit[] medkits = new Medkit[]{new Medkit("small"), new Medkit("medium"), new Medkit("large")};

    public int getDamage()
    {
        return baseDamage;
    }
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 1)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
           
        }

        if (Time.timeScale == 0)
        {
            return;
        }
        Move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(Move * speed, rb.velocity.y);
        if(Input.GetButtonDown("Jump") && isGrounded){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }
        if(Input.GetButton("Sprint")){
            speed = 10;
        }else{
            speed = 5;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Grass")){
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("Grass")){
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
}
