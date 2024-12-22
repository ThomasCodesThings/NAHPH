using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGunScript : MonoBehaviour
{
    GameObject player;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate angle between player and this weapon and set angle of weapon to that
        Vector3 difference = player.transform.position - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        // Flip sprite if player is on the left side
        if (player.transform.position.x < transform.position.x)
        {
             transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + + 360);
        }else{
            transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
        }
    
       
    

    }
}
