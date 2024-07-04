using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GP_ENDSCREEN : MonoBehaviour
{
    public void OnRetryPressed()
    {
        if (Gamemanager.g_instance != null)
        {
            Gamemanager.g_instance.gameRunning = true;
            Gamemanager.g_instance.m_currentTime = Gamemanager.g_instance.m_maxTime;
        }
        SceneManager.LoadScene("SampleScene");
    }

    public void OnMenuPressed()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
