using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        int currentMaxWidth = Screen.currentResolution.width; // Get the current screen width
        int currentMaxHeight = Screen.currentResolution.height; // Get the current screen height
        Screen.SetResolution(currentMaxWidth, currentMaxHeight, Screen.fullScreen); // Set the resolution to the current screen resolution
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Change the screen resolution based on the input value of dropdown menu
    public void HandleInputData(int value){
        switch(value){
            case 0:
                Screen.SetResolution(800, 600, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1024, 768, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1366, 768, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;
            case 4:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            default:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
        }
    }
}
