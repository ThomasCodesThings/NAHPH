using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneScript : MonoBehaviour
{
    private GameObject gameManager;
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
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar.value = health;
        healthBar.maxValue = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * healthBarOffset);
        healthBar.value = health;
        healthBar.fillRect.GetComponent<Image>().color = Color.Lerp(minHealthColor, maxHealthColor, healthBar.normalizedValue);
        int width = gameManager.GetComponent<GameManager>().getBaseWidth();    
        (int newX, int newY) = gameManager.GetComponent<GameManager>().getNextBlock(transform.position.x, transform.position.y);
        Vector3 target = new Vector3(newX - width, newY, 0);
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smooth);

    }
}
