using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{

    [HideInInspector]public static Gamemanager g_instance;
    [SerializeField] TextMeshProUGUI m_text;

    [SerializeField] float m_maxTime;
    float m_currentTime;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(m_currentTime < 0)
        {
            m_currentTime = 0;
            //end game
        }
        int minutes = Mathf.FloorToInt(m_currentTime / 60);
        int seconds = Mathf.FloorToInt(m_currentTime % 60);
        m_text.text = string.Format("{0}:{1:00}", minutes, seconds);
        m_currentTime -= Time.deltaTime;
    }
}
