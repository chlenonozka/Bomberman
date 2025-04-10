using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    public Text player1NameText; 
    public Text player2NameText; 
    public TMP_InputField player1LevelInput; 
    public TMP_InputField player2LevelInput; 

    private void Start()
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

        
        if (player1LevelInput != null)
        {
            player1LevelInput.text = "";
        }
        if (player2LevelInput != null)
        {
            player2LevelInput.text = "";
        }
    }

   
    public void OnStartGameButtonPressed()
    {
   
        string player1Input = player1LevelInput.text;
        string player2Input = player2LevelInput.text;


        if (string.IsNullOrEmpty(player1Input) || string.IsNullOrEmpty(player2Input))
        {
            Debug.LogWarning("Оба игрока должны выбрать уровень!");
            return;
        }


        if (!int.TryParse(player1Input, out int player1Level) || !int.TryParse(player2Input, out int player2Level))
        {
            Debug.LogWarning("Введите корректные номера уровней (числа)!");
            return;
        }


        if (player1Level < 1 || player1Level > 8 || player2Level < 1 || player2Level > 8)
        {
            Debug.LogWarning("Номер уровня должен быть от 1 до 8!");
            return;
        }


        string finalLevel = DetermineFinalLevel(player1Level, player2Level);

        
        SceneManager.LoadScene(finalLevel);
    }

    private string DetermineFinalLevel(int player1Level, int player2Level)
    {
        string finalLevel = $"Level{player1Level}";

        
        if (player1Level != player2Level)
        {
            string[] levels = { $"Level{player1Level}", $"Level{player2Level}" };
            finalLevel = levels[Random.Range(0, 2)];
            Debug.Log($"Игроки выбрали разные уровни. Случайный выбор: {finalLevel}");
        }
        else
        {
            Debug.Log($"Игроки выбрали один и тот же уровень: {finalLevel}");
        }

        return finalLevel;
    }

    
    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("Menu");
    }
}