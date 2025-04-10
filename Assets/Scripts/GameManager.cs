using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 

    private string player1Name;
    private string player2Name;
    private bool player1Alive = true;
    private bool player2Alive = true;

    public Text winnerText; 

    private void Awake()
    {
        // Настраиваем синглтон
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player1Name = PlayerPrefs.GetString("player1", "Player1");
        player2Name = PlayerPrefs.GetString("player2", "Player2");

        
        if (winnerText != null)
        {
            winnerText.gameObject.SetActive(false);
        }

        
        ResetGame();
    }


    public void PlayerDied(string playerName)
    {
        if (playerName == player1Name)
        {
            player1Alive = false;
        }
        else if (playerName == player2Name)
        {
            player2Alive = false;
        }

 
        CheckForWinner();
    }

    private void CheckForWinner()
    {
        if (!player1Alive && !player2Alive)
        {
         
            EndMatch("Ничья");
        }
        else if (!player1Alive)
        {
            
            EndMatch(player2Name);
        }
        else if (!player2Alive)
        {
            
            EndMatch(player1Name);
        }
    }

    private void EndMatch(string winner)
    {
        
        if (winnerText != null)
        {
            winnerText.text = $"{winner} победил!";
            winnerText.gameObject.SetActive(true);
        }

        
        int matchCount = PlayerPrefs.GetInt("MatchCount", 0);

        
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        string matchData = $"{player1Name}|{player2Name}|{winner}|{timestamp}";

        
        PlayerPrefs.SetString($"Match_{matchCount}", matchData);
        PlayerPrefs.SetInt("MatchCount", matchCount + 1);
        PlayerPrefs.Save();

        Debug.Log($"Match ended: {matchData}, MatchCount: {matchCount + 1}");

        // Переходим в меню через 3 секунды, чтобы игрок увидел сообщение
        Invoke("ReturnToMenu", 3f);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    
    public void ResetGame()
    {
        player1Alive = true;
        player2Alive = true;

        
        if (winnerText != null)
        {
            winnerText.gameObject.SetActive(false);
        }
    }
}