using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifesManager : MonoBehaviour
{
    private int lifes;
    public UnityEngine.UI.Toggle lifesToggle;

    void Start()
    {
        loadLifesSettings();

        lifesToggle.onValueChanged.AddListener(lifesChecker);

        if (lifes == 0)
        {
            lifesToggle.isOn = false;
        }
        else { lifesToggle.isOn = true; }
    }

    public void lifesChecker(bool isOn)
    {
        if (lifesToggle.isOn)
        {
            lifes = 1;
        }
        else { lifes = 0; }
        saveLifesSettings();
    }

    public void saveLifesSettings()
    {
        PlayerPrefs.SetInt("lifes", lifes);
        PlayerPrefs.Save();
        Debug.Log("Lifes settings saved");
    }

    public void loadLifesSettings()
    {
        if (PlayerPrefs.HasKey("lifes"))
        {
            lifes = PlayerPrefs.GetInt("lifes");
        }
        else
        {
            lifes = 0;
            Debug.LogError("There is no save data!");
        }
    }
}
