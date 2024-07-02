using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP_CAMERACONTROLLER : MonoBehaviour
{

    public Transform m_pc;
    [SerializeField] float m_zDisplace;



    [SerializeField] private float m_maxX;
    [SerializeField] private float m_maxZ;
    [SerializeField] private float m_minX;
    [SerializeField] private float m_minZ;

    // Start is called before the first frame update
    void Start()
    {
        m_pc = FindObjectOfType<PC_MOVEMENT>().transform;
    }

    // Update is called once per frame
    void Update()
    {

        float nX = transform.position.x;
        float nZ = transform.position.z;
        float tZD = 0;

        if (m_pc.position.x < m_maxX && m_pc.position.x > m_minX)
        {
            nX = m_pc.position.x;
        }
        if (m_pc.position.z < m_maxZ && m_pc.position.z > m_minZ)
        {
            nZ = m_pc.position.z;
            tZD = m_zDisplace;
        }

        transform.position = new Vector3(nX, transform.position.y, nZ - tZD);
    }
}
