using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalStateMultiPlayerManager : MonoBehaviour
{
    public int deadPlayers = 0;
    private int deadPlayerNumber = -1;
    private string deadPlayerName;
    private string alivePlayerName;
    public GameObject endGamePanel;
    public GameObject[] inGameTexts;
    public Text overText;
    public UnityEvent endGame;
    public GameObject cameraMain;
    public GameObject drawCamera;

    public GameObject counter;

    public void PlayerDied(string playerName)
    {
        deadPlayers++;

        deadPlayerName = playerName;

        if (deadPlayers == 1)
        {
            Invoke("CheckPlayersDeath", .3f);
        }
    }

    void CheckPlayersDeath()
    {
        if (deadPlayers == 1)
        {
            endGame.Invoke();
            Time.timeScale = 0.5f;
            if (deadPlayerNumber == 1)
            {
                offTexts();
                //endGamePanel.SetActive(true);
                overText.text = alivePlayerName + " is the winner!";
                Debug.Log(alivePlayerName + " is the winner!");
            }
            else
            {
                offTexts();
                //endGamePanel.SetActive(true);
                overText.text = alivePlayerName + " is the winner!";
                Debug.Log(alivePlayerName + " is the winner!");
            }
        }
        else
        {
            endGame.Invoke();
            Time.timeScale = 0.5f;
            drawCamera.SetActive(true);
            offTexts();
            //endGamePanel.SetActive(true);
            Debug.Log("The game ended in a draw!");
        }
    }

    private void offTexts()
    {
        foreach (GameObject n in inGameTexts)
        {
            n.gameObject.SetActive(false);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        CrateSpawn spawn = GetComponent<CrateSpawn>();
        spawn.respawnCrates();
    }

    public void menuPressed()
    {
        SceneManager.LoadScene("Menu");
    }
}
