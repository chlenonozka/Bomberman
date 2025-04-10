using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlSelManager : MonoBehaviour
{
    public Text instructionText; // ����� ��� ���������� (��������, "����� 1: �������� �������")
    public GameObject[] levelFrames; // ������ ����� ��� ������� (Level1Frame�Level8Frame)

    private bool isPlayer1Turn = true; // ��� ������� ��������
    private string player1LevelChoice; // ����� ������ ������� 1
    private string player2LevelChoice; // ����� ������ ������� 2
    private int player1FrameIndex = -1; // ������ ����� ��� ������ 1
    private int player2FrameIndex = -1; // ������ ����� ��� ������ 2

    private void Start()
    {
        
        UpdateInstructionText();

        // �������� ��� ����� ��� ������
        foreach (GameObject frame in levelFrames)
        {
            frame.SetActive(false);
        }
    }

    // ����� ��� ������ ������ (���������� �������� �������)
    public void SelectLevel(int levelIndex)
    {
        string levelName = $"Level{levelIndex}";

        if (isPlayer1Turn)
        {
            // ����� 1 �������� �������
            player1LevelChoice = levelName;
            player1FrameIndex = levelIndex - 1; // ������ ����� (0�7)

            // ���������� ����� ��� ���������� ������
            if (player1FrameIndex >= 0 && player1FrameIndex < levelFrames.Length)
            {
                levelFrames[player1FrameIndex].SetActive(true);
            }

            // ����������� ������� �� ������ 2
            isPlayer1Turn = false;
            UpdateInstructionText();
        }
        else
        {
            // ����� 2 �������� �������
            player2LevelChoice = levelName;
            player2FrameIndex = levelIndex - 1; // ������ ����� (0�7)

            // ���������� ����� ��� ���������� ������
            if (player2FrameIndex >= 0 && player2FrameIndex < levelFrames.Length)
            {
                levelFrames[player2FrameIndex].SetActive(true);
            }

            // ��� ������ ������� ������, ���������� ��������� �������
            DetermineFinalLevel();
        }
    }

    private void UpdateInstructionText()
    {
        if (instructionText != null)
        {
            instructionText.text = isPlayer1Turn ? "����� 1: �������� �������" : "����� 2: �������� �������";
        }
    }

    private void DetermineFinalLevel()
    {
        string finalLevel = player1LevelChoice;

        // ���� ������ ������� ������ ������, �������� ���������
        if (player1LevelChoice != player2LevelChoice)
        {
            string[] levels = { player1LevelChoice, player2LevelChoice };
            finalLevel = levels[Random.Range(0, 2)];
            Debug.Log($"������ ������� ������ ������. ��������� �����: {finalLevel}");
        }
        else
        {
            Debug.Log($"������ ������� ���� � ��� �� �������: {finalLevel}");
        }

        
        SceneManager.LoadScene(finalLevel);
    }

    
    
}