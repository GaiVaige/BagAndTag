using JetBrains.Annotations;
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

        m_maxTime = m_currentTime;
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
        m_currentTime -= Time.deltaTime;
        m_text.text = m_currentTime.ToString();
    }
}
