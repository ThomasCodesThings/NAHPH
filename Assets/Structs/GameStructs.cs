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
        public int medkits;
        public int medkitsUsed;
        public int enemiesKilled;

      public PlayerStats(int health, int maxHealth, int xp, int ammo, int magazine, string weaponName, int medkits, int medkitsUsed, int enemiesKilled){
            this.health = health;
            this.maxHealth = maxHealth;
            this.xp = xp;
            this.ammo = ammo;
            this.magazine = magazine;
            this.weaponName = weaponName;
            this.medkits = medkits;
            this.medkitsUsed = medkitsUsed;
            this.enemiesKilled = enemiesKilled;
      }
    }

    public struct Level{
        public int xpToNextLevel;

        public Level(int xpToNextLevel){
            this.xpToNextLevel = xpToNextLevel;
        }
    }

    public struct GameStats{
        public string time;
        public string totalEnemiesKilled;
        public string totalMedkitsUsed;
        public string totalXp;

        public GameStats(string time, string totalEnemiesKilled, string totalMedkitsUsed, string totalXp){
            this.time = time;
            this.totalEnemiesKilled = totalEnemiesKilled;
            this.totalMedkitsUsed = totalMedkitsUsed;
            this.totalXp = totalXp;
        }
    }
}
