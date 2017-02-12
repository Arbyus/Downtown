using UnityEngine;
using System.Collections;

public class RoadController : MonoBehaviour {

	public GameObject[] m_roads;
	int m_CurrentlyInside = 0;
    bool m_InitialSpawn = true;

    void Start()
    {
        for( int i = 0; i < m_roads.Length; ++i )
        {
            m_roads[i].GetComponent<ChildCollider>().SetStartPos( new Vector3(0,0, 1975 * i ));
        }
    }


    public void EnteredTrigger()
    {
        if (!m_InitialSpawn)
        {
            //Debug.Log("NA");
            ++m_CurrentlyInside;
            if(m_CurrentlyInside == 5)
            {
                m_CurrentlyInside = 0;
            }
            int currentLead;
            int currentBack;
            switch (m_CurrentlyInside)
            {
                case 0:
                    currentLead = 2;
                    currentBack = 3;
                    ; break;
                case 1:
                    currentLead = 3;
                    currentBack = 4;
                    ; break;
                case 2:
                    currentLead = 4;
                    currentBack = 0;
                    ; break;
                case 3:
                    currentLead = 0;
                    currentBack = 1;
                    ; break;
                case 4:
                    currentLead = 1;
                    currentBack = 2;
                    ; break;
                default:
                    currentLead = 1;
                    currentBack = 2;
                    ; break;
            }

            float newRoadPos = m_roads[currentLead].transform.Find("nextRoadPointer").transform.position.z;
            m_roads[currentBack].transform.position = new Vector3(m_roads[currentBack].transform.position.x, m_roads[currentBack].transform.position.y, newRoadPos);
        }
        else if (m_InitialSpawn)
        {
           // Debug.Log("FUSRT");
            m_InitialSpawn = false;         
        }
    }

    public void ResetRoad()
    {
         m_CurrentlyInside = 0;
         m_InitialSpawn = true;
         foreach(GameObject segment in m_roads)
        {
            segment.GetComponent<ChildCollider>().ResetRoadSegment();
        }  
    }

}
