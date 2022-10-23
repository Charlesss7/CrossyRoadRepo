using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string musicPref = "musicPref";
    private static readonly string sfxPref = "sfxPref";
    private int firstPlayInt;
    public Slider musicSlider, sfxSlider;
    private float musicFloat, sfxFloat;
    public AudioSource musicAudio;
    public AudioSource[] sfxAudio;
    //jika sfx lebih dari 1

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);
        if(firstPlayInt == 0)
        {
            musicFloat = .125f;
            sfxFloat = .75f;
            musicSlider.value = musicFloat;
            sfxSlider.value = sfxFloat;
            PlayerPrefs.SetFloat(musicPref,musicFloat);
            PlayerPrefs.SetFloat(sfxPref, sfxFloat);
            PlayerPrefs.SetInt(FirstPlay,-1);
        }
        else
        {
            musicFloat = PlayerPrefs.GetFloat(musicPref);
            musicSlider.value = musicFloat;
            sfxFloat = PlayerPrefs.GetFloat(sfxPref);
            sfxSlider.value = sfxFloat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(musicPref,musicSlider.value);
            PlayerPrefs.SetFloat(sfxPref, sfxSlider.value);
    }

    //jika game di pause / minimize = lost focus, 
    //pref dari sound ttp akan disave jika lost focus
    void OnApplicationFocus(bool inFocus)
    {
        if(!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        musicAudio.volume = musicSlider.value;

        for(int i=0; i<sfxAudio.Length;i++)
        {
            sfxAudio[i].volume = sfxSlider.value;
        }
    }
}
