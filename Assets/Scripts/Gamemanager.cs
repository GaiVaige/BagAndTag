using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{

    [HideInInspector]public static Gamemanager g_instance;
    [SerializeField] TextMeshProUGUI m_text;
    PC_MOVEMENT m_pc;
    [SerializeField] float m_maxTime;
    float m_currentTime;
    public bool gameRunning = true;

    public List<int> m_itemIds = new List<int>();
    public int m_finalScore;

    // Start is called before the first frame update
    void Start()
    {
        m_currentTime = m_maxTime;
        g_instance = FindObjectOfType<Gamemanager>() ;
        if(g_instance != this && g_instance != null)
        {
            Destroy(this);
        }
        else
        {
            g_instance = this;
            DontDestroyOnLoad(this);
        }
        m_pc = FindObjectOfType<PC_MOVEMENT>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            if (m_currentTime < 0)
            {
                m_currentTime = 0;
                EndGame();
            }
            int minutes = Mathf.FloorToInt(m_currentTime / 60);
            int seconds = Mathf.FloorToInt(m_currentTime % 60);
            m_text.text = string.Format("{0}:{1:00}", minutes, seconds);
            m_currentTime -= Time.deltaTime;
            m_finalScore = m_pc.m_totalScore;
        }
    }

    void EndGame()
    {
        gameRunning = false;
        SceneManager.LoadScene(2);
    }
}
