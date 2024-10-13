using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 5;

    public float Move;
    public Rigidbody2D rb;
    private bool isGrounded;
    private int baseDamage = 10;

    public int getDamage()
    {
        return baseDamage;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(Move * speed, rb.velocity.y);
        if(Input.GetButtonDown("Jump") && isGrounded){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }
        if(Input.GetButton("Sprint")){
            speed = 10;
        }else{
            speed = 5;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Grass")){
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("Grass")){
            isGrounded = false;
        }
    }
}
