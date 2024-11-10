using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackScript : MonoBehaviour
{
    [SerializeField] int ammoAmount = 10;
    private Rigidbody2D rb;

    public void updateAmmoPack(string type){
        switch(type){
            case "small":
                ammoAmount = 8;
                gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case "medium":
                ammoAmount = 12;
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                break;
            case "large":
                ammoAmount = 16;
                gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            default:
                ammoAmount = 8;
                break;
        }
    }
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

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Floor")){
            rb.isKinematic = true;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
    }
}