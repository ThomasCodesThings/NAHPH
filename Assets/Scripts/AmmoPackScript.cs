using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackScript : MonoBehaviour
{
    [SerializeField] int ammoAmount = 10;
    private Rigidbody2D rb;

    // Update the ammo amount and size of the ammo pack
    public void updateAmmoPack(string type){
        switch(type){
            case "small":
                ammoAmount = 8;
                gameObject.transform.localScale = gameObject.transform.localScale * 0.5f;
                break;
            case "medium":
                ammoAmount = 12;
            
                break;
            case "large":
                ammoAmount = 16;
                gameObject.transform.localScale = gameObject.transform.localScale * 1.5f;
                break;
            default:
                ammoAmount = 8;
                break;
        }
    }

    // Get the ammo amount
    public int getAmmoAmount(){
        return ammoAmount;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When the ammo pack collides with the floor, stop the ammo pack from moving
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Floor")){
            if(collision.gameObject == null){
                return;
            }

            if(rb == null){
                return;
            }
            rb.isKinematic = true;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
    }
}
