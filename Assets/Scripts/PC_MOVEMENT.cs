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
    List<GP_EVIDENCE> m_heldItems = new List<GP_EVIDENCE>();
    [SerializeField] LayerMask itemLayer;

    // Start is called before the first frame update
    void Start()
    {
        m_cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_cc.Move(CalculateMovementDirection() * Time.deltaTime);
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
        Collider[] itemsInRange = Physics.OverlapCapsule(transform.position, transform.position, checkRadius, itemLayer, QueryTriggerInteraction.UseGlobal);
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
                GP_EVIDENCE item = itemsInRange[closestItem].gameObject.GetComponent<GP_EVIDENCE>();
                if (item != null)
                {
                    m_heldItems.Add(item);
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

        if (Input.GetKeyDown(KeyCode.Space) && m_heldItems.Count != 0)
        {
            //every time you mash the key you dump evidence item 0
            //at this point mark evidence as scored
            m_heldItems[0].m_includeInScore = true;
            m_heldItems.RemoveAt(0);
            Debug.Log("Item dumped");
            // Play dump sound effect/ animation
        }
    }
}
