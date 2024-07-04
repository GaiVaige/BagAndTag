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
        }
        SceneManager.LoadScene("SampleScene");
    }

    public void OnMenuPressed()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
