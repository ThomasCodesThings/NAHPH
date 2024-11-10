using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackScript : MonoBehaviour
{
    [SerializeField] int ammoAmount = 10;

    public void updateAmmoPack(string type){
        switch(type){
            case "small":
                ammoAmount = 8;
                gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case "medium":
                ammoAmount = 12;
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                break;
            case "large":
                ammoAmount = 16;
                gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            default:
                ammoAmount = 8;
                break;
        }
    }
    public int getAmmoAmount(){
        return ammoAmount;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
