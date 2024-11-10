using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitScript : MonoBehaviour
{
     private int healAmount = 10;
    private Rigidbody2D rb;

    public void updateMedkit(string type){
        switch(type){
            case "small":
                healAmount = 10;
                gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case "medium":
                healAmount = 25;
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                break;
            case "large":
                healAmount = 50;
                gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            default:
                healAmount = 10;
                break;
        }
    }

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

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Floor") && rb != null){
            rb.isKinematic = true;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
    }
}
