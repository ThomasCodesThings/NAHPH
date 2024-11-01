using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStructs
{
    
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public class Medkit{
        private int healAmount = 10;
        public Medkit(string type){
            switch(type){
                case "small":
                    healAmount = 10;
                    break;
                case "medium":
                    healAmount = 25;
                    break;
                case "large":
                    healAmount = 35;
                    break;
                default:
                    healAmount = 10;
                    break;
            }
        }
    }
}
