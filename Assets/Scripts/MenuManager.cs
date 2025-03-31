using SavedSettings.GUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public Animator menuAnim;
    public MusicLogic ml;
    private int music;
    public AudioSource musicSource;
    public UnityEngine.UI.Toggle musicToggle;
    void Start()
    {
        Time.timeScale = 1f;
        menuAnim.SetTrigger("Open");
        loadMusicSettings();

        musicToggle.onValueChanged.AddListener(musicChecker);

        if (music == 0)
        {
            musicSource.Stop();
            musicToggle.isOn = false;
        }
    }

    public void goToMulti()
    {
        SceneManager.LoadScene("MenuMultiPlayer");
    }

    public void checkMusicOn()
    {
        if (music == 0)
        {
            musicSource.Stop();
        }
        else { musicSource.Play(); }
    }

    public void exit()
    {
        ml.enabled = true;
        Application.Quit();
    }

    public void level1()
    {
        ml.enabled = true;
        StartCoroutine(levelStartUp1());
    }

    public void level2()
    {
        ml.enabled = true;
        StartCoroutine(levelStartUp2());
    }

    public void level3()
    {
        ml.enabled = true;
        StartCoroutine(levelStartUp3());
    }

    public void level4()
    {
        ml.enabled = true;
        StartCoroutine(levelStartUp4());
    }

    public void level5()
    {
        ml.enabled = true;
        StartCoroutine(levelStartUp5());
    }

    public void level6()
    {
        ml.enabled = true;
        StartCoroutine(levelStartUp6());
    }

    public void level7()
    {
        ml.enabled = true;
        StartCoroutine(levelStartUp7());
    }

    public void level8()
    {
        ml.enabled = true;
        StartCoroutine(levelStartUp8());
    }

    private IEnumerator levelStartUp1()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level1");
    }

    private IEnumerator levelStartUp2()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level2");
    }

    private IEnumerator levelStartUp3()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level3");
    }

    private IEnumerator levelStartUp4()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level4");
    }

    private IEnumerator levelStartUp5()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level5");
    }

    private IEnumerator levelStartUp6()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level6");
    }

    private IEnumerator levelStartUp7()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level7");
    }

    private IEnumerator levelStartUp8()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level8");
    }

    public void musicChecker(bool isOn)
    {
        if(musicToggle.isOn)
        {
            music = 1;
        }
        else { music = 0; }
        checkMusicOn();
        saveMusicSettings();
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
