using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public struct GameObjectContainer
{
   List<GameObject> m_Ships;
   BuildingRow[] m_BuildingRows;
   GameObject m_RoadManager;
}

public class ObjectsController : MonoBehaviour {
    public GameObject[] m_Buildings;
    public GameObject m_BuildingBase;
	GameObjectContainer m_Objects;
    int m_ObstPointer = -500;
    float m_Obstoffset = 70;
    float[] m_RoadOffsets;
    List<GameObject> m_ShipsInScene = new List<GameObject>();
    GameObject m_RoadManager;
    int m_LastShipInQueue = 0;
    BuildingRow[] m_BuildingRowsInScene = new BuildingRow[30];
    float m_BuildingFrontQueue;
	int m_ZOffset = -55;
	int m_BuildingFrontPointer = 0;

    void Start () {
       
    }

    void WraparoundShip(int index)
    {
        m_ShipsInScene[index].transform.position = new Vector3(m_ShipsInScene[index].transform.position.x, UnityEngine.Random.Range(-15, 20), m_ShipsInScene[m_LastShipInQueue].transform.position.z + (int)m_Obstoffset);
        m_LastShipInQueue = index;
        m_ShipsInScene[index].GetComponent<shipMove>().Wrapped();
        //Debug.Log(index);
    }
	
	void CheckBuildingRowWrap(float p_TriggerZPos)
	{
		if(p_TriggerZPos > m_BuildingFrontQueue)
		{
			m_BuildingRowsInScene[m_BuildingFrontPointer].SetZOffset(m_ZOffset);
            m_ZOffset += 70;
			++m_BuildingFrontPointer;
            if(m_BuildingFrontPointer == m_BuildingRowsInScene.Length)
            {
                m_BuildingFrontPointer = 0;
            }
            m_BuildingFrontQueue = m_BuildingRowsInScene[m_BuildingFrontPointer].m_Buildings[0].transform.position.z;
        }
	}

    void ResetEverything()
    {
        m_BuildingFrontPointer = 0;
        for (int i = 0; i < m_BuildingRowsInScene.Length; ++i)
        {
            m_BuildingRowsInScene[i].SetZOffset(m_BuildingRowsInScene[i].m_ZOffset);
        }
        m_BuildingFrontQueue = m_BuildingRowsInScene[m_BuildingFrontPointer].m_Buildings[0].transform.position.z;
        ++m_BuildingFrontPointer;
        m_ZOffset = (int)m_BuildingRowsInScene[m_BuildingRowsInScene.Length - 1].m_ZOffset + 70;
		
		
		foreach(GameObject ship in m_ShipsInScene)
		{
			ship.GetComponent<shipMove>().ResetShip();
		}
		m_LastShipInQueue = m_ShipsInScene.Count - 1;

        m_RoadManager.GetComponent<RoadController>().ResetRoad();


    }

    void StartShips()
    {
        foreach (GameObject ship in m_ShipsInScene)
        {
            ship.GetComponent<shipMove>().StartShip();
        }
    }

    void StopShips()
    {
        foreach (GameObject ship in m_ShipsInScene)
        {
            ship.GetComponent<shipMove>().StopShip();
        }
    }

}
