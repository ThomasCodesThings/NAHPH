using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisBulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //set gameobject sprite renderer to false
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] medkits = GameObject.FindGameObjectsWithTag("Medkit");
        GameObject[] ammopacks = GameObject.FindGameObjectsWithTag("AmmoPack");
        GameObject[] energyProjectiles = GameObject.FindGameObjectsWithTag("EnergyProjectile");
        GameObject[] ballisticProjectiles = GameObject.FindGameObjectsWithTag("BallisticProjectile");

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

        foreach(GameObject energyProjectile in energyProjectiles){
            if(energyProjectile == null){
                continue;
            }
            Physics2D.IgnoreCollision(energyProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        foreach(GameObject ballisticProjectile in ballisticProjectiles){
            if(ballisticProjectile == null){
                continue;
            }
            Physics2D.IgnoreCollision(ballisticProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor")){
            Debug.Log("Bullet hit the floor");
            Destroy(gameObject);
        }
    }
}
}
