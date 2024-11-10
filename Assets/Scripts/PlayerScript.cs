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
    private int medkitsUsed = 0;
    private int enemiesKilled = 0;
    private float timeToHeal = 5.0f;
    private float healingTimer = 0.0f; 
    private bool isHealing = false; 
    private List<int> medkitsHealingAmount = new List<int>();
    private GameObject rangeWeapon;
    private GameObject meleeWeapon;

      public GameObject FindChildWithTag(GameObject parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public GameObject getRangeWeapon()
    {
        return rangeWeapon;
    }

    public GameObject getMeleeWeapon()
    {
        return meleeWeapon;
    }

    public int getDamage()
    {
        return this.baseDamage;
    }

    public void setHealth(int damage)
    {
        this.health -= damage;
    }

    public bool isKilled()
    {
        return health <= 0;
    }

    public int heal()
    {
        if (medkitsHealingAmount.Count > 0)
        {
            int healAmount = medkitsHealingAmount[0];
            health += healAmount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            medkitsUsed++;
            medkitsHealingAmount.RemoveAt(0);
        }
        
        return health;
    }

    public PlayerStats getPlayerStats()
    {
        return new PlayerStats(health, maxHealth, xp, rangeWeapon.GetComponent<GunScript>().getAmmo(), rangeWeapon.GetComponent<GunScript>().getMagazine(), rangeWeapon.GetComponent<GunScript>().getName(), medkitsHealingAmount.Count, medkitsUsed, enemiesKilled);
    }

    public void addXP(int xp)
    {
        this.xp += xp;
    }

    public void addKill()
    {
        enemiesKilled++;
    }

    public int getXp()
    {
        return xp;
    }

    public int getKills()
    {
        return enemiesKilled;
    }

    public int getMedkitsUsed()
    {
        return medkitsUsed;
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        rangeWeapon = GameObject.FindGameObjectWithTag("PlayerRangeWeapon");
        //meleeWeapon = GameObject.FindGameObjectWithTag("PlayerMeleeWeapon");

        int weaponDamage = rangeWeapon.GetComponent<GunScript>().getDamage();
        Debug.Log("Weapon Damage: " + weaponDamage);
        
    }

    // Update is called once per frame
    void Update()
    {

        if(gameObject.transform.position.y < -10)
        {
            health = 0;
        }

        if (Input.GetKeyDown(KeyCode.E) && !isHealing && medkitsHealingAmount.Count > 0)
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

            medkitsHealingAmount.Add(other.gameObject.GetComponent<MedkitScript>().getHealAmount());
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
