using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GlobalStateManager : MonoBehaviour
{
    private int lifes;
    public int deadPlayers = 0;
    private int deadPlayerNumber = -1;
    public GameObject endGamePanel;
    public Text endGamePanelText;
    public GameObject[] inGameTexts;
    public UnityEvent endGame;
    private bool gameIsEnded = false;
    public GameObject cameraMain;
    public GameObject drawCamera;

    public GameObject pauseMenu;
    public bool isPaused;

    public GameObject counter;

    public Animator inblack;

    public string player1_nickname;
    public string player2_nickname;

    public GameObject fireworks;

    public MusicLogic ml;

    public BombPickUp_Spawner healthSpawner;

    private float time;
    public Text timer;
    public Text endTimer;

    private int second;
    private int minute;
    private string secondS;
    private string minuteS;
    public bool timerPaused = false;

    private void Awake()
    {
        counter = GameObject.FindWithTag("Counter");
        Cursor.visible = false;

        loadLifesSettings();

        if (lifes == 0)
        {
            healthSpawner.enabled = false;
        }
        else
        {
            healthSpawner.enabled = true;
        }

        LoadNickname();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && isPaused == false && gameIsEnded == false)
        {
            isPaused = true;
            Cursor.visible = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (timerPaused == false)
        {
            time += Time.deltaTime;
            second = (int)(time % 60);
            minute = (int)(time / 60);
            if (second < 10) { secondS = "0" + second.ToString(); } else { secondS = second.ToString(); }
            if (minute < 10) { minuteS = "0" + minute.ToString(); } else { minuteS = minute.ToString(); }
            timer.text = minuteS + ":" + secondS;
        }
        else { endTimer.text = minuteS + ":" + secondS; }
    }

    public void PlayerDied(int playerNumber)
    {
        deadPlayers++;

        if (deadPlayers == 1)
        {
            deadPlayerNumber = playerNumber;
            Invoke("CheckPlayersDeath", .3f);
        }
    }

    void CheckPlayersDeath()
    {
        string winner = "";
        if (deadPlayers == 1)
        {
            endGame.Invoke();
            Time.timeScale = 0.5f;
            if (deadPlayerNumber == 1)
            {
                offTexts();
                endGamePanel.SetActive(true);
                Cursor.visible = true;
                endGamePanelText.text = player2_nickname + " победил!";
                counter.GetComponent<CounterManager>().winP2();
                gameIsEnded = true;
                fireworks.SetActive(true);
                winner = player2_nickname;
            }
            else
            {
                offTexts();
                endGamePanel.SetActive(true);
                Cursor.visible = true;
                endGamePanelText.text = player1_nickname + " победил!";
                counter.GetComponent<CounterManager>().winP1();
                gameIsEnded = true;
                fireworks.SetActive(true);
                winner = player1_nickname;
            }
        }
        else
        {
            endGame.Invoke();
            Time.timeScale = 0.5f;
            drawCamera.SetActive(true);
            Cursor.visible = true;
            offTexts();
            endGamePanel.SetActive(true);
            endGamePanelText.text = "Игра окончена ничьей!";
            Debug.Log("The game ended in a draw!");
            gameIsEnded = true;
            winner = "Ничья";
        }

        counter.GetComponent<CounterManager>().inTotal();
        timerPaused = true;

        SaveMatchResult(winner);
    }

    private void SaveMatchResult(string winner)
    {
        
        int matchCount = PlayerPrefs.GetInt("MatchCount", 0);

        
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        string matchData = $"{player1_nickname}|{player2_nickname}|{winner}|{timestamp}";

        PlayerPrefs.SetString($"Match_{matchCount}", matchData);
        PlayerPrefs.SetInt("MatchCount", matchCount + 1);
        PlayerPrefs.Save();

        Debug.Log($"Match saved: {matchData}, MatchCount: {matchCount + 1}");
    }

    private void offTexts()
    {
        cameraMain.SetActive(false);
        foreach (GameObject n in inGameTexts)
        {
            n.gameObject.SetActive(false);
        }
    }

    public void restartLevel()
    {
        StartCoroutine(restartStartUp());
    }

    public void allRestartLevel()
    {
        StartCoroutine(allrestartStartUp());
    }

    public void menuPressed()
    {
        StartCoroutine(menuStartUp());
    }

    public void continuePressed()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
    }

    private IEnumerator restartStartUp()
    {
        ml.enabled = true;
        inblack.SetTrigger("Enter");
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator allrestartStartUp()
    {
        ml.enabled = true;
        inblack.SetTrigger("Enter");
        counter.GetComponent<CounterManager>().clearP();
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator menuStartUp()
    {
        ml.enabled = true;
        inblack.SetTrigger("Enter");
        counter.GetComponent<CounterManager>().clearP();
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Menu");
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

    public void LoadNickname()
    {
        player1_nickname = PlayerPrefs.GetString("player1");
        player2_nickname = PlayerPrefs.GetString("player2");
    }
}