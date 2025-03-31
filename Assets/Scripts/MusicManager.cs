using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private int music;
    public AudioSource musicSource;
    public UnityEngine.UI.Toggle musicToggle;

    void Start()
    {
        loadMusicSettings();

        musicToggle.onValueChanged.AddListener(musicChecker);

        if (music == 0)
        {
            musicSource.Stop();
            musicToggle.isOn = false;
        }
    }

    public void musicChecker(bool isOn)
    {
        if (musicToggle.isOn)
        {
            music = 1;
        }
        else { music = 0; }
        checkMusicOn();
        saveMusicSettings();
    }

    public void checkMusicOn()
    {
        if (music == 0)
        {
            musicSource.Stop();
        }
        else { musicSource.Play(); }
    }

    public void saveMusicSettings()
    {
        PlayerPrefs.SetInt("music", music);
        PlayerPrefs.Save();
        Debug.Log("Music settings saved");
    }

    public void loadMusicSettings()
    {
        if (PlayerPrefs.HasKey("music"))
        {
            music = PlayerPrefs.GetInt("music");
        }
        else
        {
            music = 1;
            Debug.LogError("There is no save data!");
        }
    }
}
