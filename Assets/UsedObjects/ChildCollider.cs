using UnityEngine;
using System.Collections;

public class ChildCollider : MonoBehaviour {

    Vector3 m_StartPos;

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ResetCollider")
        {
            gameObject.GetComponentInParent<RoadController>().EnteredTrigger();
        }
    }

    public void SetStartPos(Vector3 p_SP)
    {
        m_StartPos = p_SP;
    }

    public void ResetRoadSegment()
    {
        this.transform.position = m_StartPos;
    }

}
