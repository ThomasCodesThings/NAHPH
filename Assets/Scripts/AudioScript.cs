using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioScript : MonoBehaviour
{

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;
    [SerializeField] public AudioClip backgroundMusic;
    [SerializeField] public TMP_Text volumeText;
    private float volume = 0.5f;

    /************************************************************************
     * 
     *  Sound Effects
     * 
     * *********************************************************************/

    [SerializeField] public AudioClip ammoPickup;
    [SerializeField] public AudioClip medkitPickup;
    [SerializeField] public AudioClip playerReload;
    [SerializeField] public AudioClip playerHeal;
    [SerializeField] public AudioClip playerJump;

    [SerializeField] public AudioClip meleeAttack;
    [SerializeField] public AudioClip basicPistolFire;
    [SerializeField] public AudioClip smgFire;
    [SerializeField] public AudioClip shotgunFire;
    [SerializeField] public AudioClip laserGunFire;
    [SerializeField] public AudioClip plasmaCannonFire;

    [SerializeField] public AudioClip soldierAttack;
    [SerializeField] public AudioClip droneAttack;
    [SerializeField] public AudioClip bossAttack;

    [SerializeField] public AudioClip levelUp;
    [SerializeField] public AudioClip playerWeaponPickup;
    [SerializeField] public AudioClip graceTimeCountdown;
    [SerializeField] public AudioClip victorySound;
    [SerializeField] public AudioClip gameOverSound;



    // Awake 
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        musicSource.volume = volume;
        if (volumeText != null)
        volumeText.text = ((int)(volume * 100)).ToString();
    }

    public void IncreaseVolume()
    {
        if (volume < 1)
        {
            volume += 0.01f;
        }
    }

    public void DecreaseVolume()
    {
        if (volume > 0)
        {
            volume -= 0.01f;
        }
    }

    public void playAmmoPickup()
    {
        sfxSource.PlayOneShot(ammoPickup);
    }

    public void playMedkitPickup()
    {
        sfxSource.PlayOneShot(medkitPickup);
    }

    public void playPlayerReload()
    {
        sfxSource.PlayOneShot(playerReload);
    }

    public void playPlayerHeal()
    {
        sfxSource.PlayOneShot(playerHeal);
    }

    public void playPlayerJump()
    {
        sfxSource.PlayOneShot(playerJump);
    }

    public void playMeleeAttack()
    {
        sfxSource.PlayOneShot(meleeAttack);
    }

    public void playRangeGunFire(string weaponName){
        switch(weaponName){
            case "Basic Pistol":
                sfxSource.PlayOneShot(basicPistolFire);
                break;
            case "Smg":
                sfxSource.PlayOneShot(smgFire);
                break;
            case "Shotgun":
                sfxSource.PlayOneShot(shotgunFire);
                break;
            case "Laser Gun":
                sfxSource.PlayOneShot(laserGunFire);
                break;
            case "Plasma Cannon":
                sfxSource.PlayOneShot(plasmaCannonFire);
                break;
        }
    }
    

    public void playSoldierAttack()
    {
        sfxSource.PlayOneShot(soldierAttack);
    }

    public void playDroneAttack()
    {
        sfxSource.PlayOneShot(droneAttack);
    }

    public void playBossAttack()
    {
        sfxSource.PlayOneShot(bossAttack);
    }

    public void playLevelUp()
    {
        sfxSource.PlayOneShot(levelUp);
    }

    public void playPlayerWeaponPickup()
    {
        sfxSource.PlayOneShot(playerWeaponPickup);
    }
}
