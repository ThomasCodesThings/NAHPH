using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifeTime = 5f;

    private float lastShotTime = 0f;
    private float shotDelay = 0.5f;
    private float bulletOffset = 0.4f;
    private int ammo = 8;
    private int magazine = 64;
    private int damage = 10;
    private string name = "Basic Pistol";

    public int getAmmo()
    {
        return ammo;
    }

    public int getMagazine()
    {
        return magazine;
    }

    public int getDamage()
    {
        return damage;
    }

    public string getName()
    {
        return name;
    }

    public void addAmmo(int amount)
    {
        magazine += amount;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
{
    if (Time.timeScale == 0)
    {
        return;
    }

    if (Input.GetKeyDown(KeyCode.R))
    {
        if (magazine > 0)
        {
            int missingAmmo = 8 - ammo;
            if (magazine >= missingAmmo)
            {
                ammo = 8;
                magazine -= missingAmmo;
            }
            else
            {
                ammo += magazine;
                magazine = 0;
            }
        }
    }

    if(Input.GetMouseButton(0) && Time.time - lastShotTime > shotDelay && ammo > 0)
    {
        lastShotTime = Time.time;

        Vector3 bulletSpawnPosition = transform.position + transform.right * bulletOffset;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        ammo--;
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
        Destroy(bullet, bulletLifeTime);
    }
}

}
