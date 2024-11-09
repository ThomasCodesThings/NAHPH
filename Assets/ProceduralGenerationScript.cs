using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerationScript : MonoBehaviour
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
    // Start is called before the first frame update

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
                    //Instantiate(stonePrefab, new Vector3(x, y, 0), Quaternion.identity);
                } else {
                    //Instantiate(dirtPrefab, new Vector3(x, y, 0), Quaternion.identity);
                }

            }
            //Instantiate(grassPrefab, new Vector3(x, height, 0), Quaternion.identity);

            if(x == -width || x == width - 1){
                for(int y = height + 1; y < height; y++){
                    //Instantiate(stonePrefab, new Vector3(x, y, 0), Quaternion.identity);
                }
                //heights.Add(height + wallSize);
            }else{
                //heights.Add(height);
            }
        }
    } 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
