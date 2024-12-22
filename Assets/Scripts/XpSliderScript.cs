using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpSliderScript : MonoBehaviour
{
    private GameObject gameManager;
    private GameObject xpSlider;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        xpSlider = GameObject.FindWithTag("XpSlider");
        if(xpSlider != null)
        {
            xpSlider.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Activate the xp slider if the player has some xp
        int currentXp = gameManager.GetComponent<GameManager>().getPlayerXP();
        if(currentXp > 0)
        {
            xpSlider.SetActive(true);
        }
    }
}
