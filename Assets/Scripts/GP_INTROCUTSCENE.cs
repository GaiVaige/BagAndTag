using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GP_INTROCUTSCENE : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_text;
    [SerializeField] GameObject m_nameBox;
    [SerializeField] TextMeshProUGUI m_name;
    [SerializeField] Image m_character;
    [SerializeField] GameObject m_characterBox;

    float m_timer;
    float m_textDelay;

    int m_currentLine = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            Continue();
        }
    }

    void Continue()
    {
        switch (m_currentLine)
        {
            case 1:
                m_nameBox.SetActive(true);
                m_characterBox.SetActive(true);
                m_text.text = "Cleaning service, how can I help you?";
                m_character.color = Color.white;
                m_name.text = "You";
                break;
            case 2:
                m_text.text = "Apartment 13, Bourke street. Five minutes and be careful of the detective.";
                m_character.color = Color.black;
                m_name.text = "???";
                break;
            case 3:
                m_text.text = "U-understood, I’ll be there right away. An urgent clean normally costs-";
                m_character.color = Color.white;
                m_name.text = "You";
                break;
            case 4:
                m_nameBox.SetActive(false);
                m_characterBox.SetActive(false);
                m_text.text = "The line clicks, there’s no one talking anymore. You’d best make the most of what time you have.";
                break;
            default:
                LoadLevel();
                break;

        }
        m_currentLine++;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
