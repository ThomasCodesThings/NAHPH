using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitScript : MonoBehaviour
{
     private int healAmount = 10;

    public void updateMedkit(string type){
        switch(type){
            case "small":
                healAmount = 10;
                gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case "medium":
                healAmount = 25;
                break;
            case "large":
                healAmount = 50;
                gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
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
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
