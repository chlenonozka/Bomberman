using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersNickname : MonoBehaviour
{
    public string player1_nickname;
    public string player2_nickname;

    public InputField player1;
    public InputField player2;

    private void Awake()
    {
        LoadNickname();
        noneNick();
        player1.text = player1_nickname;
        player2.text = player2_nickname;
    }

    public void noneNick()
    {
        if (player1_nickname is null)
        {
            player1_nickname = "Игрок 1";
        }

        if (player2_nickname is null)
        {
            player2_nickname = "Игрок 2";
        }
    }

    public void closeApply()
    {
        if (player1.text == "")
        {
            player1_nickname = "Игрок 1";
        }
        else { player1_nickname = player1.text; }

        if (player2.text == "")
        {
            player2_nickname = "Игрок 2";
        }
        else { player2_nickname = player2.text; }
        saveNickname();
        player1.text = player1_nickname;
        player2.text = player2_nickname;
    }

    //Save
    public void saveNickname()
    {
        PlayerPrefs.SetString("player1", player1_nickname);
        PlayerPrefs.SetString("player2", player2_nickname);
        PlayerPrefs.Save();
        Debug.Log("Nicknames saved!");
    }

    public void LoadNickname()
    {
        if (PlayerPrefs.HasKey("player1") || PlayerPrefs.HasKey("player2"))
        {
            player1_nickname = PlayerPrefs.GetString("player1");
        }
        else { player1_nickname = "Игрок 1"; }

        if (PlayerPrefs.HasKey("player1") || PlayerPrefs.HasKey("player2"))
        {
            player2_nickname = PlayerPrefs.GetString("player2");
        }
        else { player2_nickname = "Игрок 2"; }
    }
}
