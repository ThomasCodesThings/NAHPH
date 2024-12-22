using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneProjectileScript : MonoBehaviour
{

    public int getDamage()
    {
        return GameObject.FindGameObjectWithTag("Drone").GetComponent<DroneScript>().getDamage();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*GameObject[] medkits = GameObject.FindGameObjectsWithTag("Medkit");
        GameObject[] ammopacks = GameObject.FindGameObjectsWithTag("AmmoPack");
        GameObject[] weakEnemies = GameObject.FindGameObjectsWithTag("WeakEnemy");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach(GameObject medkit in medkits){
            if(medkit == null){
                continue;
            }
            Physics2D.IgnoreCollision(medkit.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        foreach(GameObject ammopack in ammopacks){
            if(ammopack == null){
                continue;
            }
            Physics2D.IgnoreCollision(ammopack.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        foreach(GameObject weakEnemy in weakEnemies){
            if(weakEnemy == null){
                continue;
            }
            Physics2D.IgnoreCollision(weakEnemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        foreach(GameObject bullet in bullets){
            if(bullet == null){
                continue;
            }
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
    }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Enemy"){
            Destroy(gameObject);
        }
    }
}
