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

    public class RangeWeapon{
        private int damage;
        private int maxAmmo;
        private int ammo;
        private int magazine;
        private string name;

        private float offsetX;
        private float offsetY;

        private float bulletSpeed;
        private float bulletLifeTime;
        private float shotDelay;

        private GameObject bulletPrefab;
        private Texture2D weaponIcon;

        public int getDamage(){
            return damage;
        }

        public int getMaxAmmo(){
            return maxAmmo;
        }

        public int getAmmo(){
            return ammo;
        }

        public int getMagazine(){
            return magazine;
        }

        public string getName(){
            return name;
        }

        public float getOffsetX(){
            return offsetX;
        }

        public float getOffsetY(){
            return offsetY;
        }

        public float getBulletSpeed(){
            return bulletSpeed;
        }

        public float getBulletLifeTime(){
            return bulletLifeTime;
        }

        public float getShotDelay(){
            return shotDelay;
        }

        public GameObject getBulletPrefab(){
            return bulletPrefab;
        }

        public Texture2D getWeaponIcon(){
            return weaponIcon;
        }

        public void addAmmo(int amount){
            this.magazine += amount;
        }

        public void setAmmo(int ammo){
            this.ammo = ammo;
        }

        public void decrementMagazine(int amount){
            this.magazine -= amount;
        }

        public void decrementAmmo(){
            this.ammo--;
        }

        public void setMagazine(int magazine){
            this.magazine = magazine;
        }

        public RangeWeapon(int damage, int maxAmmo, int ammo, int magazine, string name, float offsetX, float offsetY, float bulletSpeed, float bulletLifeTime, float shotDelay, GameObject bulletPrefab, Texture2D weaponIcon){
            this.damage = damage;
            this.maxAmmo = maxAmmo;
            this.ammo = ammo;
            this.magazine = magazine;
            this.name = name;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.bulletSpeed = bulletSpeed;
            this.bulletLifeTime = bulletLifeTime;
            this.shotDelay = shotDelay;
            this.bulletPrefab = bulletPrefab;
            this.weaponIcon = weaponIcon;
        }
    }
}
