using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CounterManager : MonoBehaviour
{
    public static CounterManager Instance { get; private set; }
    public int player1, player2;
    public Text totalc;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public void winP1()
    {
        player1++;
    }

    public void winP2()
    {
        player2++;
    }

    public void clearP()
    {
        player1 = 0;
        player2 = 0;
    }

    public void inTotal()
    {
        totalc = GameObject.FindWithTag("TotalText").GetComponent<Text>();
        totalc.text = player1.ToString() + " - " + player2.ToString();
    }
}
