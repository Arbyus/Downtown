using UnityEngine;
using System.Collections;

public class RoadController : MonoBehaviour {

	public GameObject[] m_roads;
	int m_CurrentlyInside = 1;
    bool m_InitialSpawn = true;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerCont" && !m_InitialSpawn)
        {
			++m_CurrentlyInside;
			int currentLead;
			int currentBack;
			switch( m_CurrentlyInside )
			{
				case 0:
					currentLead = 2;
					currentBack = 3;	
					;break;
				case 1:
					currentLead = 3;
					currentBack = 4;	
					;break;
				case 2:
				    currentLead = 4;
					currentBack = 0;	
					;break;
				case 3:
				    currentLead = 0;
					currentBack = 1;	
					;break;
				case 4:
					currentLead = 1;
					currentBack = 2;	
					;break;
			}
			
			float newRoadPos = m_roads[currentLead].transform.Find("nextRoadPointer").transform.position.z;
			m_roads[currentBack].transform.position = new Vector3(m_roads[currentBack].transform.position.x, m_roads[currentBack].transform.position.y, newRoadPos);
        }
    }

}
