using UnityEngine;
using System.Collections;

public class RoadController : MonoBehaviour {

    public GameObject r0;
    public GameObject r1;
    bool m_CurrentlyInsideR0 = true;
    bool m_InitialSpawn = true;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerCont" && !m_InitialSpawn)
        {
            if( m_CurrentlyInsideR0 )
            {
                r0.transform.Find("SP");
                //Move R0, we have left and entered R1
                m_CurrentlyInsideR0 = false;
            }
            else
            {
                //move R1
                m_CurrentlyInsideR0 = true;
            }
        }
    }

}
