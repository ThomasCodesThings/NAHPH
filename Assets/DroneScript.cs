using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    private GameObject gameManager;
    private float smooth = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        int width = gameManager.GetComponent<GameManager>().getBaseWidth();    
        (int newX, int newY) = gameManager.GetComponent<GameManager>().getNextBlock(transform.position.x, transform.position.y);
       Vector3 target = new Vector3(newX - width, newY, 0);
       Debug.Log(target);
       transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smooth);

    }
}
