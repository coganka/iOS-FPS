using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource bgm;
    public AudioSource gunFX;
    public AudioSource hitmarkerFX;
    public AudioSource enemyShotFX;

    public AudioSource[] soundEffects;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopBGM()
    {
        bgm.Stop();
    }
    public void PlayBGM()
    {
        bgm.Play();
    }

    public void PlayGunSound()
    {
        gunFX.Play();
    }

    public void PlayHitFX()
    {
        hitmarkerFX.Play();
    }

    public void PlayEnemyShot()
    {
        enemyShotFX.Play();
    }

    public void PlaySFX(int sfxNumber)
    {
        soundEffects[sfxNumber].Stop();
        soundEffects[sfxNumber].Play();
    }

    public void StopSFX(int sfxNumber)
    {
        soundEffects[sfxNumber].Stop();
    }
}
