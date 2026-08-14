using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer Audio;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider AmbSlider;
    public Slider SFXSlider;
    public MainMenuEvents mainMenuEvents;
    
    public void SetUp()
    {
        gameObject.SetActive(true);
    }

    public void setMasterVolume()
    {
        float volume = masterSlider.value;
        Audio.SetFloat("Master", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("Master", volume);
    }

    public void setMusicVolume()
    {
        float volume = musicSlider.value;
        Audio.SetFloat("BGM", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("BGM", volume);
    }

    public void setSFXVolume()
    {
        float volume = SFXSlider.value;
        Audio.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFX", volume);
    }

    public void setAMBVolume()
    {
        float volume = SFXSlider.value;
        Audio.SetFloat("AMB", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("AMB", volume);
    }

    public void loadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("BGM", 1f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFX", 1f);
        AmbSlider.value = PlayerPrefs.GetFloat("AMB", 1f);

        setMasterVolume();
        setMusicVolume();
        setSFXVolume();
        setAMBVolume();
    }

    public void LoadMenu()
    {
        mainMenuEvents.uIDocument.enabled = true;
        gameObject.SetActive(false);
    }
}