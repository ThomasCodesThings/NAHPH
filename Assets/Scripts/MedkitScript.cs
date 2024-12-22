using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitScript : MonoBehaviour
{
    private int healAmount = 10;
    private Rigidbody2D rb;

    // Update medkit size and heal amount based on type
    public void updateMedkit(string type){
        switch(type){
            case "small":
                healAmount = 10;
                gameObject.transform.localScale = gameObject.transform.localScale * 0.5f;
                break;
            case "medium":
                healAmount = 25;
                //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                break;
            case "large":
                healAmount = 50;
                gameObject.transform.localScale = gameObject.transform.localScale * 1.5f;
                break;
            default:
                healAmount = 10;
                break;
        }
    }

    // Get heal amount
    public int getHealAmount(){
        return healAmount;
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

    // Stop medkit from moving when it hits the floor
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Floor") && rb != null){
            rb.isKinematic = true;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
    }
}
