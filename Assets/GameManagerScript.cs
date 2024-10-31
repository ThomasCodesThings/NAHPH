using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
   public int gameDifficulty = 0;
   public TextMeshProUGUI difficultyText;

   void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;
        difficultyText.text = "EASY";
        difficultyText.color = new Color32(0, 255, 0, 255);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void IncreaseDifficulty()
    {
        gameDifficulty++;
        if(gameDifficulty > 2){
            gameDifficulty = 2;
        }
        
        switch(gameDifficulty){
            case 0:
                difficultyText.text = "EASY";
                //set color to green
                difficultyText.color = new Color32(0, 255, 0, 255);

                break;
            case 1:
                difficultyText.text = "MEDIUM";
                //set color to yellow
                difficultyText.color = new Color32(255, 255, 0, 255);
                break;
            case 2:
                difficultyText.text = "HARD";
                //set color to red
                difficultyText.color = new Color32(255, 0, 0, 255);
                break;
            default:
                difficultyText.text = "EASY";
                //set color to green
                difficultyText.color = new Color32(0, 255, 0, 255);
                break;
        }
        
    }

    public void DecreaseDifficulty(){
        gameDifficulty--;
        if(gameDifficulty < 0){
            gameDifficulty = 0;
        }
        
        switch(gameDifficulty){
            case 0:
                difficultyText.text = "EASY";
                //set color to green
                difficultyText.color = new Color32(0, 255, 0, 255);

                break;
            case 1:
                difficultyText.text = "MEDIUM";
                //set color to yellow
                difficultyText.color = new Color32(255, 255, 0, 255);
                break;
            case 2:
                difficultyText.text = "HARD";
                //set color to red
                difficultyText.color = new Color32(255, 0, 0, 255);
                break;
            default:
                difficultyText.text = "EASY";
                //set color to green
                difficultyText.color = new Color32(0, 255, 0, 255);
                break;
        }
        
    }

}
