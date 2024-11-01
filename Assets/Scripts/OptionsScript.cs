using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScript : MonoBehaviour
{
    [SerializeField] GameObject fullScreenOn;
    [SerializeField] GameObject fullScreenOff;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchFullScreen()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
            fullScreenOn.SetActive(false);
            fullScreenOff.SetActive(true);
        }
        else
        {
            Screen.fullScreen = true;
            fullScreenOn.SetActive(true);
            fullScreenOff.SetActive(false);
        }
    }

    public void Close(){
        gameObject.SetActive(false);
    }


}
