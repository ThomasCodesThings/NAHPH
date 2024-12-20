using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructs;

public class GunScript : MonoBehaviour
{
    [SerializeField] private GameObject baseBulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifeTime = 5f;

    private float lastShotTime = 0f;

    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
{
    if (Time.timeScale == 0)
    {
        return;
    }

    RangeWeapon weapon = player.GetComponent<PlayerScript>().getCurrentRangeWeapon();
    

   if (Input.GetKeyDown(KeyCode.R))
   {
        if (weapon.getMagazine() > 0)
        {
            int missingAmmo = weapon.getMaxAmmo() - weapon.getAmmo();
            int ammoToReload = Mathf.Min(missingAmmo, weapon.getMagazine());

            weapon.setAmmo(weapon.getAmmo() + ammoToReload);
            weapon.decrementMagazine(ammoToReload);
        }
    }


    if(Input.GetMouseButton(0) && Time.time - lastShotTime > weapon.getShotDelay() && weapon.getAmmo() > 0)
    {
        lastShotTime = Time.time;

        Vector3 bulletSpawnPosition = transform.position + transform.right * weapon.getOffsetX() + transform.up * weapon.getOffsetY();

        GameObject bullet = Instantiate(weapon.getBulletPrefab(), bulletSpawnPosition, Quaternion.identity);
        weapon.decrementAmmo();
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * weapon.getBulletSpeed();
        Destroy(bullet, weapon.getBulletLifeTime());
    }
   
}

}
