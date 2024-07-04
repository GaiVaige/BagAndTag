using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EN_DETECTIVE : MonoBehaviour
{
    NavMeshAgent m_ma;
    [SerializeField] float m_suspicion;
    [SerializeField] float m_susIncreaseMod;
    bool m_hitThisFrame;

    [SerializeField] float m_resetTime;
    [SerializeField] float m_pointCheckRadius;
    [SerializeField] float m_stopDist;
    [SerializeField] float m_timer = 0;
    bool m_walking;
    [SerializeField]Slider m_slider;

    [SerializeField] float m_lookDegrees;
    [SerializeField] float m_checkRadius;
    [Range(1, 1000)]
    [Tooltip("Clamped for performance.")]
    [SerializeField] int m_lookResolution;
    PC_MOVEMENT m_pc;
    [SerializeField] bool m_drawVisibilityGizmos;


    [SerializeField] TextMeshPro m_tm;
    bool m_doNotUpdateText;
    void Start()
    {
        m_tm.text = "";
        m_ma = GetComponent<NavMeshAgent>();
        m_ma.SetDestination(Random.insideUnitSphere * m_pointCheckRadius);
        m_pc = FindObjectOfType<PC_MOVEMENT>();
    }

    // Update is called once per frame
    void Update()
    {
        m_tm.gameObject.transform.SetPositionAndRotation(m_tm.transform.position, Quaternion.identity);
        if(m_suspicion >= 100)
        {
            Gamemanager.g_instance.gameRunning = false;
            SceneManager.LoadScene(3);
        }

        m_hitThisFrame = false;
        UpdatePoint();
        CheckForPlayer();
        if (m_hitThisFrame)
        {
            UpdateSuspicion();
        }
        else if (m_suspicion > 0)
        {
            m_suspicion -= m_susIncreaseMod * Time.deltaTime;
        }
        else
        {
            m_suspicion = 0;
        }
        m_slider.value = m_suspicion / 100;
        if(m_slider.value > 1) { m_slider.value = 1; }
        if(m_slider.value == 0)
        {
            m_slider.fillRect.gameObject.SetActive(false);
        }
        else {
            m_slider.fillRect.gameObject.SetActive(true);
        }
    }

    void UpdatePoint()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_resetTime && m_ma.remainingDistance <= m_stopDist)
        {
            NavMeshPath p = new NavMeshPath();

            if(m_ma.CalculatePath(Random.insideUnitSphere * m_pointCheckRadius, p) && p.status == NavMeshPathStatus.PathComplete)
            {
                m_ma.SetPath(p);
            }
            else
            {
                return;
            }
            m_timer = 0;
            m_walking = true;
        }
        else
        {
            m_walking = false;

        }
    }

    void CheckForPlayer()
    {
        float intermittentRes = m_lookDegrees / m_lookResolution;
        float resAdder = intermittentRes * 2;

        Vector3 min = Quaternion.AngleAxis(-m_lookDegrees, transform.up) * transform.forward;

        RaycastHit minHit;
        if (Physics.Raycast(transform.position, min, out minHit, m_checkRadius))
        {
            if (m_drawVisibilityGizmos) { Debug.DrawLine(transform.position, minHit.point, Color.red); }
            CheckIfPlayerHasItems(minHit);
        }
        else
        {
            if (m_drawVisibilityGizmos) { Debug.DrawRay(transform.position, min * m_checkRadius, Color.green); }
        }
        for (int i = 0; i < m_lookResolution; i++)
        {
            Vector3 nV = Quaternion.AngleAxis(-m_lookDegrees + intermittentRes, transform.up) * transform.forward;
            intermittentRes += resAdder;
            RaycastHit loopHit;
            if (Physics.Raycast(transform.position, nV, out loopHit, m_checkRadius))
            {
                if (m_drawVisibilityGizmos) { Debug.DrawLine(transform.position, loopHit.point, Color.red); }
                CheckIfPlayerHasItems(loopHit);
            }
            else
            {
                if (m_drawVisibilityGizmos) { Debug.DrawRay(transform.position, nV * m_checkRadius, Color.green); }
            }
        }
        RaycastHit maxHit;
        Vector3 max = Quaternion.AngleAxis(m_lookDegrees, transform.up) * transform.forward;
        if (Physics.Raycast(transform.position, max * m_checkRadius, out maxHit, m_checkRadius))
        {
            if (m_drawVisibilityGizmos) { Debug.DrawLine(transform.position, maxHit.point, Color.red); }
            CheckIfPlayerHasItems(maxHit);
        }
        else
        {
            if (m_drawVisibilityGizmos) { Debug.DrawRay(transform.position, max * m_checkRadius, Color.green); }
        }
    }

    void CheckIfPlayerHasItems(RaycastHit hit)
    {
        if(hit.collider.gameObject.tag == "Player")
        {
            if (m_pc.m_hasItems)
            {
                m_hitThisFrame = true;
                if (!m_doNotUpdateText)
                {
                    m_tm.text = "Hey! What have you got there!";
                m_doNotUpdateText = true;
                }

            }
        }
    }

    void UpdateSuspicion()
    {
        m_suspicion += (m_pc.m_itemsCollected * m_susIncreaseMod) * Time.deltaTime;
    }

    IEnumerator TextTimer()
    {
        yield return new WaitForSeconds(3.0f);
        m_tm.text = "";
        m_doNotUpdateText = false;
    }
}
