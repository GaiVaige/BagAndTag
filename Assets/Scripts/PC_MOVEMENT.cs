using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PC_MOVEMENT : MonoBehaviour
{

    [SerializeField] float m_MoveSpeed;
    CharacterController m_cc;

    [SerializeField] float checkRadius;
    [SerializeField] GameObject m_itemPrompt;
    [SerializeField] GameObject m_toiletPrompt;
    [SerializeField] TextMeshPro m_text;
    [SerializeField] List<GP_EVIDENCE> m_heldItems = new List<GP_EVIDENCE>();
    [SerializeField] AudioSource m_as;
    [SerializeField] LayerMask itemLayer;
    [SerializeField] AudioClip m_itemSound;
    [SerializeField] LayerMask toiletLayer;
    [SerializeField] AudioClip m_toiletSound;

    [HideInInspector] public int m_totalScore;
    [HideInInspector] public int m_itemsCollected = 0;

    public bool m_hasItems;
    int m_itemCount;

    // Start is called before the first frame update
    void Start()
    {
        m_as = GetComponent<AudioSource>();
        m_cc = GetComponent<CharacterController>();
        m_text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        m_cc.Move(CalculateMovementDirection() * Time.deltaTime);
            m_itemCount = m_heldItems.Count;
        if (m_itemCount != 0)
        {
            m_hasItems = true;
        }
        else
        {
            m_hasItems = false;
        }
        CheckForItems();
        DumpItems();
    }

    Vector3 CalculateMovementDirection()
    {
        Vector3 newMove = new Vector3(m_MoveSpeed * PollForHInput(), 0, m_MoveSpeed * PollForVInput());
        return newMove;
    }


    float PollForHInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    float PollForVInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    void CheckForItems()
    {
        //scan for items in capsule (pretend cylinder)
        //add to array
        Collider[] itemsInRange = Physics.OverlapCapsule(transform.position, transform.position, checkRadius, itemLayer);
        //if array != 0, show pickup input as a UI element
        if (itemsInRange.Length != 0)
        {
            float shortestDist = 0;
            int closestItem = 0;
            for (int i = 0; i < itemsInRange.Length; i++)
            {
                float itemDist = Vector3.Distance(transform.position, itemsInRange[i].transform.position);
                if (shortestDist > itemDist)
                {
                    shortestDist = itemDist;
                    closestItem = i;
                }
            }
            Debug.DrawLine(transform.position, itemsInRange[closestItem].transform.position);
            if(Physics.Linecast(transform.position, itemsInRange[closestItem].transform.position, LayerMask.GetMask("Default")))
            {
                m_itemPrompt.SetActive(false);
                return;
            }
            else
            {

                m_itemPrompt.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //iterate over the colliders returned above, and grab the closest

                    //add to the pile you carry in front of you
                    //add to a List<Evidence>
                    //Evidence has a name and score saved in a tuple
                    GP_EVIDENCE item = itemsInRange[closestItem].GetComponent<GP_EVIDENCE>();
                    if (item != null)
                    {
                        StopAllCoroutines();
                        m_text.text = "";
                        m_heldItems.Add(item); // Add item to m_heldItems
                        item.gameObject.SetActive(false); // Make item inactive (can't be seen, collided with or picked up again)

                        m_itemsCollected++;
                        m_as.clip = m_itemSound;
                        m_as.Play();
                        m_text.gameObject.SetActive(true);
                        m_text.text = item.m_text;
                        StartCoroutine(TextTimer());
                    }
                }
            }
            
        }
        else
        {
            m_itemPrompt.SetActive(false);
        }
    }

    void DumpItems()
    {
        Collider[] collidersFound = Physics.OverlapSphere(transform.position, checkRadius, toiletLayer);
        if (collidersFound.Length != 0 && m_heldItems.Count != 0)
        {
            if(Physics.Linecast(transform.position, collidersFound[0].transform.position, LayerMask.GetMask("Default")))
            {
                return;
            }
            else
            {
                m_toiletPrompt.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_totalScore += m_heldItems[0].score;
                    Gamemanager.g_instance.m_itemIds.Add(m_heldItems[0].m_evidenceID);
                    m_as.clip = m_toiletSound;
                    m_as.Play();
                    if (m_heldItems.Count == 1)
                    {
                        m_heldItems.Clear();
                    }
                    else if (m_heldItems.Count > 1)
                    {
                        m_heldItems.RemoveAt(0);
                    }

                    // Play dump sound effect/ animation
                }
            }

            
        }
        else
        {
            m_toiletPrompt.SetActive(false);
        }
    }

    IEnumerator TextTimer()
    {

        yield return new WaitForSeconds(5.0f);
        m_text.gameObject.SetActive(false);
    }
}
