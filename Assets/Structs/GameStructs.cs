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

    public struct PlayerStats
    {
        public int health;
        public int maxHealth;
        public int xp;
        public int ammo;
        public int magazine;
        public string weaponName;
        public int aidKits;

      public PlayerStats(int health, int maxHealth, int xp, int ammo, int magazine, string weaponName, int aidKits){
            this.health = health;
            this.maxHealth = maxHealth;
            this.xp = xp;
            this.ammo = ammo;
            this.magazine = magazine;
            this.weaponName = weaponName;
            this.aidKits = aidKits;
      }
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
