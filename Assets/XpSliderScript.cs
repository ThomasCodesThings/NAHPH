using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpSliderScript : MonoBehaviour
{
    private GameObject player;
    private GameObject xpSlider;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        xpSlider = GameObject.FindWithTag("XpSlider");
        if(xpSlider != null)
        {
            xpSlider.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int currentXp = player.GetComponent<PlayerScript>().getXp();
        if(currentXp > 0)
        {
            xpSlider.SetActive(true);
        }
    }
}
