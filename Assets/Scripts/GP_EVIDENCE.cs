using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP_EVIDENCE : MonoBehaviour
{
    public uint m_evidenceID;
    public Tuple<string, int> m_scoringDetails;
    [HideInInspector] public bool m_includeInScore;
}
