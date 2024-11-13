using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //set gameobject sprite renderer to false
        //GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] medkits = GameObject.FindGameObjectsWithTag("Medkit");
        GameObject[] ammopacks = GameObject.FindGameObjectsWithTag("AmmoPack");
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

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

        foreach(GameObject enemyBullet in enemyBullets){
            if(enemyBullet == null){
                continue;
            }
            Physics2D.IgnoreCollision(enemyBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Enemy"){
   
            Destroy(gameObject);
        }
    }
}
}
