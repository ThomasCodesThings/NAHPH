using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructs;

public class ProceduralGeneration : MonoBehaviour
{

    [SerializeField] GameObject dirtPrefab;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject floorLeftPrefab;
    [SerializeField] GameObject floorRightPrefab;
    [SerializeField] GameObject platformCenterPrefab;
    [SerializeField] GameObject platformLeftPrefab;
    [SerializeField] GameObject platformRightPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] GameObject player;
    [SerializeField] GameObject difficulty;
    private GameObject difficultyManager;

    private List<int> heights = new List<int>();
    private Difficulty currentDifficulty = Difficulty.Easy;

    private int wallSize = 20;

    public List<int> getHeights(){
        return heights;
    }

    public int getBaseWidth(){
        return width;
    }


   public void generate()
    {
        // Initial random height for the first segment

        for (int x = -width; x < width; x++)
        {
            Instantiate(dirtPrefab, new Vector3(x, 2, 0), Quaternion.identity);
            Instantiate(floorPrefab, new Vector3(x, 3, 0), Quaternion.identity);
        }
    }
    
   void Awake()
{
    difficultyManager = GameObject.Find("DifficultyManager");

    if (difficultyManager != null)
    {
        currentDifficulty = difficultyManager.GetComponent<DifficultyManager>().GetDifficulty();
        Debug.Log("Current Difficulty: " + currentDifficulty);
    }
    else
    {
        Debug.LogError("DifficultyManager not found in the scene.");
    }
}
    // Start is called before the first frame update
    void Start()
    {
    
     generate();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
