using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PC_MOVEMENT : MonoBehaviour
{

    [SerializeField] float m_MoveSpeed;
    CharacterController m_cc;

    [SerializeField] float checkRadius;
    [SerializeField] GameObject m_itemPrompt;
    [SerializeField] GameObject m_toiletPrompt;
    List<GP_EVIDENCE> m_heldItems = new List<GP_EVIDENCE>();
    [SerializeField] LayerMask itemLayer;
    [SerializeField] LayerMask toiletLayer;

    int m_totalScore;
    
    public bool m_hasItems;

    // Start is called before the first frame update
    void Start()
    {
        m_cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_cc.Move(CalculateMovementDirection() * Time.deltaTime);
        if(m_heldItems.Count != 0)
        {
            m_hasItems = true;
        }
        //CheckForItems();
        //DumpItems();
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
            m_itemPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                //iterate over the colliders returned above, and grab the closest
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
                //add to the pile you carry in front of you
                //add to a List<Evidence>
                //Evidence has a name and score saved in a tuple
                GP_EVIDENCE item = itemsInRange[closestItem].GetComponent<GP_EVIDENCE>();
                if (item != null)
                {
                    m_heldItems.Add(item); // Add item to m_heldItems
                    item.gameObject.SetActive(false); // Make item inactive (can't be seen, collided with or picked up again)
                    Debug.Log("Item picked up");
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
        //If at toilet (trigger box?) & holding items, show space key (greyed if 0 items)
        if (Physics.CheckCapsule(transform.position, transform.position, checkRadius, toiletLayer) && m_heldItems.Count != 0)
        {
            m_toiletPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_totalScore += m_heldItems[0].score;
                if (m_heldItems.Count == 1)
                {
                    m_heldItems.Clear();
                }
                else if (m_heldItems.Count > 1)
                {
                    m_heldItems.RemoveAt(0);
                }
                Debug.Log("Item dumped");
                // Play dump sound effect/ animation
            }
        }
        else
        {
            m_toiletPrompt.SetActive(false);
        }
    }
}
