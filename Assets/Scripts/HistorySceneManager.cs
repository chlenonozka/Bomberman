using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; // Добавляем пространство имён для ScrollRect

public class HistorySceneManager : MonoBehaviour
{
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player2NameText;
    public Text player1ScoreText;
    public Text player2ScoreText;
    public TextMeshProUGUI historyText;
    public ScrollRect scrollView; // Заменяем ScrollView на ScrollRect

    private void Start()
    {
        DisplayNicknamesAndScores();
        DisplayMatchHistory();
    }

    private void DisplayNicknamesAndScores()
    {
        string player1Name = PlayerPrefs.GetString("player1", "Player1");
        string player2Name = PlayerPrefs.GetString("player2", "Player2");

        if (player1NameText != null)
        {
            player1NameText.text = player1Name;
        }
        if (player2NameText != null)
        {
            player2NameText.text = player2Name;
        }

        int player1Wins = 0;
        int player2Wins = 0;
        int matchCount = PlayerPrefs.GetInt("MatchCount", 0);

        for (int i = 0; i < matchCount; i++)
        {
            string matchData = PlayerPrefs.GetString($"Match_{i}", "");
            if (!string.IsNullOrEmpty(matchData))
            {
                string[] parts = matchData.Split('|');
                if (parts.Length == 4)
                {
                    string winner = parts[2];
                    if (winner == player1Name)
                    {
                        player1Wins++;
                    }
                    else if (winner == player2Name)
                    {
                        player2Wins++;
                    }
                }
            }
        }

        if (player1ScoreText != null)
        {
            player1ScoreText.text = player1Wins.ToString();
        }
        if (player2ScoreText != null)
        {
            player2ScoreText.text = player2Wins.ToString();
        }
    }

    private void DisplayMatchHistory()
    {
        int matchCount = PlayerPrefs.GetInt("MatchCount", 0);
        string history = "";
        for (int i = 0; i < matchCount; i++)
        {
            string matchData = PlayerPrefs.GetString($"Match_{i}", "");
            if (!string.IsNullOrEmpty(matchData))
            {
                string[] parts = matchData.Split('|');
                if (parts.Length == 4)
                {
                    string player1Name = parts[0];
                    string player2Name = parts[1];
                    string winner = parts[2];
                    string timestamp = parts[3];

                    history += $"{timestamp}: {player1Name} vs {player2Name} - Победитель: {winner}\n";
                }
            }
        }

        if (string.IsNullOrEmpty(history))
        {
            history = "История матчей пуста.";
        }

        if (historyText != null)
        {
            historyText.text = history;

            // Прокручиваем к последней записи
            if (scrollView != null)
            {
                Canvas.ForceUpdateCanvases(); // Обновляем UI, чтобы размеры пересчитались
                scrollView.verticalNormalizedPosition = 0f; // Прокручиваем вниз
            }
        }
        else
        {
            Debug.LogWarning("HistoryText is not assigned in the Inspector!");
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ClearHistory()
    {
        PlayerPrefs.DeleteKey("MatchCount");
        for (int i = 0; i < 1000; i++)
        {
            PlayerPrefs.DeleteKey($"Match_{i}");
        }
        PlayerPrefs.Save();
        DisplayNicknamesAndScores();
        DisplayMatchHistory();
    }
}