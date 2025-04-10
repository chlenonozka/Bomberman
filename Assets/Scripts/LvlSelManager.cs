using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlSelManager : MonoBehaviour
{
    public Text instructionText; // Текст для инструкции (например, "Игрок 1: Выберите уровень")
    public GameObject[] levelFrames; // Массив рамок для уровней (Level1Frame–Level8Frame)

    private bool isPlayer1Turn = true; // Чья очередь выбирать
    private string player1LevelChoice; // Выбор уровня Игроком 1
    private string player2LevelChoice; // Выбор уровня Игроком 2
    private int player1FrameIndex = -1; // Индекс рамки для Игрока 1
    private int player2FrameIndex = -1; // Индекс рамки для Игрока 2

    private void Start()
    {
        
        UpdateInstructionText();

        // Скрываем все рамки при старте
        foreach (GameObject frame in levelFrames)
        {
            frame.SetActive(false);
        }
    }

    // Метод для выбора уровня (вызывается кнопками уровней)
    public void SelectLevel(int levelIndex)
    {
        string levelName = $"Level{levelIndex}";

        if (isPlayer1Turn)
        {
            // Игрок 1 выбирает уровень
            player1LevelChoice = levelName;
            player1FrameIndex = levelIndex - 1; // Индекс рамки (0–7)

            // Показываем рамку для выбранного уровня
            if (player1FrameIndex >= 0 && player1FrameIndex < levelFrames.Length)
            {
                levelFrames[player1FrameIndex].SetActive(true);
            }

            // Переключаем очередь на Игрока 2
            isPlayer1Turn = false;
            UpdateInstructionText();
        }
        else
        {
            // Игрок 2 выбирает уровень
            player2LevelChoice = levelName;
            player2FrameIndex = levelIndex - 1; // Индекс рамки (0–7)

            // Показываем рамку для выбранного уровня
            if (player2FrameIndex >= 0 && player2FrameIndex < levelFrames.Length)
            {
                levelFrames[player2FrameIndex].SetActive(true);
            }

            // Оба игрока выбрали уровни, определяем финальный уровень
            DetermineFinalLevel();
        }
    }

    private void UpdateInstructionText()
    {
        if (instructionText != null)
        {
            instructionText.text = isPlayer1Turn ? "Игрок 1: Выберите уровень" : "Игрок 2: Выберите уровень";
        }
    }

    private void DetermineFinalLevel()
    {
        string finalLevel = player1LevelChoice;

        // Если игроки выбрали разные уровни, выбираем случайный
        if (player1LevelChoice != player2LevelChoice)
        {
            string[] levels = { player1LevelChoice, player2LevelChoice };
            finalLevel = levels[Random.Range(0, 2)];
            Debug.Log($"Игроки выбрали разные уровни. Случайный выбор: {finalLevel}");
        }
        else
        {
            Debug.Log($"Игроки выбрали один и тот же уровень: {finalLevel}");
        }

        
        SceneManager.LoadScene(finalLevel);
    }

    
    
}