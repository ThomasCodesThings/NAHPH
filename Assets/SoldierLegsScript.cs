using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierLegsScript : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
