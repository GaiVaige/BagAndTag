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



    void Start()
    {
        m_ma = GetComponent<NavMeshAgent>();
        m_ma.SetDestination(Random.insideUnitSphere * m_pointCheckRadius);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePoint();
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
}
