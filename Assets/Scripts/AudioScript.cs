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


}
