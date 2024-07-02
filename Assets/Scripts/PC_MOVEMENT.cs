using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PC_MOVEMENT : MonoBehaviour
{

    [SerializeField] float m_MoveSpeed;
    CharacterController m_cc;
    [SerializeField] float checkRadius;

    // Start is called before the first frame update
    void Start()
    {
        m_cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_cc.Move(CalculateMovementDirection() * Time.deltaTime);
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
            //if array != 0, show pickup input as a UI element

        if (Input.GetKeyDown(KeyCode.E))
        {
            //iterate over the colliders returned above, and grab the closest
            //add to the pile you carry in front of you
            //add to a List<Evidence>
                                //Evidence has a name and score saved in a tuple
        }
    }

    void DumpItems()
    {
        //If at toilet (trigger box?), show space key (greyed if 0 items)
        //every time you mash the key you dump evidence item 0
        //at this point mark evidence as scored
    }
}
