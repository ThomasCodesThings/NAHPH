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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
{
    if(Input.GetMouseButton(0) && Time.time - lastShotTime > shotDelay)
    {
        lastShotTime = Time.time;

        Vector3 bulletSpawnPosition = transform.position + transform.right * bulletOffset;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
        Destroy(bullet, bulletLifeTime);
    }
}

}
