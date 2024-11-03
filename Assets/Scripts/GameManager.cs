using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{

    [SerializeField] GameObject stonePrefab;
    [SerializeField] GameObject grassPrefab;
    [SerializeField] GameObject dirtPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] int width;
    [SerializeField] int height;

    private List<int> heights = new List<int>();

    private int wallSize = 20;

    public List<int> getHeights(){
        return heights;
    }

    public int getBaseWidth(){
        return width;
    }

    public void generate(){
        for(int x = -width; x < width; x++){
            int minHeigth = height - 1;
            int maxHeigth = height + 2;
            height = Random.Range(minHeigth, maxHeigth);
            int minStoneSpawnDistance = height - 3;
            int maxStoneSpawnDistance = height - 5;
            int stoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);

            for(int y = 0; y < height; y++){
                if(y < stoneSpawnDistance){
                    Instantiate(stonePrefab, new Vector3(x, y, 0), Quaternion.identity);
                } else {
                    Instantiate(dirtPrefab, new Vector3(x, y, 0), Quaternion.identity);
                }

            }
            Instantiate(grassPrefab, new Vector3(x, height, 0), Quaternion.identity);

            if(x == -width || x == width - 1){
                for(int y = height + 1; y < height + wallSize; y++){
                    Instantiate(stonePrefab, new Vector3(x, y, 0), Quaternion.identity);
                }
                heights.Add(height + wallSize);
            }else{
                heights.Add(height);
            }
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
