using UnityEngine;
using System.Collections;

public class Jumping : MonoBehaviour
{

    enum JumpState
    {
        RISING,
        DECENT,
        GROUNDED
    }
    JumpState m_CurrentState;

    short m_Jumping;

    void Start()
    {
        m_CurrentState = JumpState.GROUNDED;
        m_Jumping = -1;
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Ground" && m_CurrentState == JumpState.DECENT)
        {
            m_CurrentState = JumpState.GROUNDED;
        }
        else if (coll.gameObject.tag == "Ground" && m_CurrentState != JumpState.DECENT)
        {
         //   Debug.Log(m_CurrentState);
        }
    }

    public bool Jump()
    {
        if (m_CurrentState == JumpState.GROUNDED)
        {
            m_Jumping = 0;
            m_CurrentState = JumpState.RISING;
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        if(m_Jumping > -1 )
        {
            float pushforce = 19f * this.GetComponent<Rigidbody>().mass;
            this.GetComponent<Rigidbody>().AddForce(new Vector3(0, pushforce, 0), ForceMode.Impulse);
            ++m_Jumping;
            if(m_Jumping == 2)
            {
                m_Jumping = -1;
            }
        }
        else if(m_CurrentState == JumpState.RISING)
        {
            if( this.GetComponent<Rigidbody>().velocity.y < 0 )
            {
                m_CurrentState = JumpState.DECENT;
                float pushforce = 19f * this.GetComponent<Rigidbody>().mass;
                this.GetComponent<Rigidbody>().AddForce(new Vector3(0, -pushforce, 0), ForceMode.Impulse);
            }
        }
        else if(m_CurrentState == JumpState.DECENT)
        {
            if (this.GetComponent<Rigidbody>().velocity.y == 0)
            {
                m_CurrentState = JumpState.GROUNDED;
            }
        }
    }

}
