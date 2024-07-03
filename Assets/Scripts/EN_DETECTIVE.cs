using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EN_DETECTIVE : MonoBehaviour
{
    NavMeshAgent m_ma;

    [SerializeField] float m_resetTime;
    [SerializeField] float m_pointCheckRadius;
    [SerializeField] float m_stopDist;
    [SerializeField] float m_timer = 0;
    bool m_walking;

    [SerializeField] float m_lookDegrees;
    [SerializeField] float m_checkRadius;
    [Range(1, 1000)]
    [Tooltip("Clamped for performance.")]
    [SerializeField] int m_lookResolution;
    PC_MOVEMENT m_pc;
    [SerializeField] bool m_drawVisibilityGizmos;

    void Start()
    {
        m_ma = GetComponent<NavMeshAgent>();
        m_ma.SetDestination(Random.insideUnitSphere * m_pointCheckRadius);
        m_pc = FindObjectOfType<PC_MOVEMENT>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePoint();
        CheckForPlayer();
    }

    void UpdatePoint()
    {
        m_timer += Time.deltaTime;
        Debug.Log(m_walking);
        if (m_timer >= m_resetTime && m_ma.remainingDistance <= m_stopDist)
        {
            m_ma.SetDestination(Random.insideUnitSphere * m_pointCheckRadius);
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
                Debug.Log("HEY!");
            }
        }
    }
}
