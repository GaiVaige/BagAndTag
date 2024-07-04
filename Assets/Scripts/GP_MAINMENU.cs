using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GP_MAINMENU : MonoBehaviour
{
    public void OnStartPressed()
    {
        if (Gamemanager.g_instance != null)
        {
            Gamemanager.g_instance.gameRunning = true;
        }
        SceneManager.LoadScene("Intro cutscene");
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
