using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GP_CALCULATESCORE : MonoBehaviour
{
    [SerializeField] Image m_evidence1;
    [SerializeField] Image m_evidence2;
    [SerializeField] Image m_evidence3;
    [SerializeField] Image m_evidence4;
    [SerializeField] Image m_evidence5;

    [SerializeField] TextMeshProUGUI m_scoreText;
    [SerializeField] float m_textDelay;
    float m_timer = 0;
    int m_displayedScore = 0;

    void Start()
    {
        if (Gamemanager.g_instance != null)
        {
            if (Gamemanager.g_instance.m_itemIds.Contains(1))
            {
                m_evidence1.color = Color.white;
            }
            if (Gamemanager.g_instance.m_itemIds.Contains(2))
            {
                m_evidence2.color = Color.white;
            }
            if (Gamemanager.g_instance.m_itemIds.Contains(3))
            {
                m_evidence3.color = Color.white;
            }
            if (Gamemanager.g_instance.m_itemIds.Contains(4))
            {
                m_evidence4.color = Color.white;
            }
            if (Gamemanager.g_instance.m_itemIds.Contains(5))
            {
                m_evidence5.color = Color.white;
            }
        }
    }

    void Update()
    {
        if (Gamemanager.g_instance != null)
        {
            if (m_displayedScore < Gamemanager.g_instance.m_finalScore)
            {
                if (m_timer <= 0)
                {
                    m_timer = m_textDelay;
                    m_displayedScore++;
                    m_scoreText.text = m_displayedScore.ToString();
                }
                m_timer -= Time.deltaTime;
            }
        }
    }
}
