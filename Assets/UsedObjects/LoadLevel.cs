using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public struct BuildingRow
{
    public List<GameObject> m_Buildings;
    public GameObject m_Floor;
    public int m_ZOffset;
    public void SetZOffset(int offset)
    {
        foreach(GameObject obj in m_Buildings)
        {
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, offset);
        }

        m_Floor.transform.localPosition = new Vector3(m_Floor.transform.localPosition.x, m_Floor.transform.localPosition.y, offset);
    }
    public void SetInitialZ(int offset)
    {
        m_ZOffset = offset;
    }
}

public class LoadLevel : MonoBehaviour {
    public GameObject m_obst;
    public GameObject m_Floor;
    public GameObject m_Road;
    public GameObject m_Ship;
    public GameObject[] m_Buildings;
    public GameObject m_BuildingBase;
    int m_ObstPointer = -500;
    float[] m_RoadOffsets;
	
    // Use this for initialization
    Quaternion shipRotation = Quaternion.Euler(270, 0, 180);

    void Place1Obst(int z, ref List<GameObject> m_Ships)
    {
        int BlockLoc = (int)(UnityEngine.Random.value * 5);
        m_Ships.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc], UnityEngine.Random.Range(-15, 20), z), shipRotation));
        m_Ships[m_Ships.Count - 1].GetComponent<shipMove>().GiveIndex(m_Ships.Count - 1);
    }

    void Place2Obst(int z, ref List<GameObject> m_Ships)
    {
        int BlockLoc = (int)(UnityEngine.Random.value * 5);
        m_Ships.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc], UnityEngine.Random.Range(-15, 20), z), shipRotation));
        m_Ships[m_Ships.Count - 1].GetComponent<shipMove>().GiveIndex(m_Ships.Count - 1);

        int BlockLoc2 = BlockLoc;
        while(BlockLoc2 == BlockLoc)
        {
            BlockLoc2 = (int)(UnityEngine.Random.value * 5);
        }
        m_Ships.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc2], UnityEngine.Random.Range(-15, 20), z), shipRotation));
        m_Ships[m_Ships.Count - 1].GetComponent<shipMove>().GiveIndex(m_Ships.Count - 1);

    }

    void Place3Obst(int z, ref List<GameObject> m_Ships)
    {
        int BlockLoc = (int)(UnityEngine.Random.value * 5);
        m_Ships.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc], UnityEngine.Random.Range(-15,20), z), shipRotation));
        m_Ships[m_Ships.Count - 1].GetComponent<shipMove>().GiveIndex(m_Ships.Count - 1);

        int BlockLoc2 = BlockLoc;
        while (BlockLoc2 == BlockLoc)
        {
            BlockLoc2 = (int)(UnityEngine.Random.value * 5);
        }
        m_Ships.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc2], UnityEngine.Random.Range(-15, 20), z), shipRotation));
        m_Ships[m_Ships.Count - 1].GetComponent<shipMove>().GiveIndex(m_Ships.Count - 1);

        int BlockLoc3 = BlockLoc;
        while (BlockLoc3 == BlockLoc || BlockLoc3 == BlockLoc2)
        {
            BlockLoc3 = (int)(UnityEngine.Random.value * 5);
        }
        m_Ships.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc3], UnityEngine.Random.Range(-15, 20), z), shipRotation));
        m_Ships[m_Ships.Count - 1].GetComponent<shipMove>().GiveIndex(m_Ships.Count - 1);
    }

	public void BuildGameObjects( ref GameObjectContainer p_Objects )
	{
        m_RoadOffsets = new float[] { -12.8f, -6.4f, 0, 6.4f, 12.8f };

		
		//Build ships
        for (int i = 0; i < 100; ++i)
        {
            int AmountOfBlocks = (int)(UnityEngine.Random.value * 3) + 1;
            if (AmountOfBlocks == 1)
            {
                Place1Obst(m_ObstPointer,ref p_Objects.ships);
            }
            else if (AmountOfBlocks == 2)
            {
                Place2Obst(m_ObstPointer,ref p_Objects.ships);
            }
            else if (AmountOfBlocks == 3)
            {
                Place3Obst(m_ObstPointer,ref p_Objects.ships);
            }
            m_ObstPointer += (int)p_Objects.shipZOffset;
        }
        p_Objects.lastShipInQueue = p_Objects.ships.Count - 1;
		
		//Build ScoreObjects
		//TODO: Needs a z counter. Just 0 now.
		for (int i = 0; i < 100; ++i)
        {
        int BlockLoc = (int)(UnityEngine.Random.value * 5);
        p_Objects.scoreObjects.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc], UnityEngine.Random.Range(-15, 20), 0), shipRotation));
        p_Objects.scoreObjects[p_Objects.scoreObjects.Count - 1].GetComponent<shipMove>().GiveIndex(p_Objects.scoreObjects.Count - 1);
        }
		
		//Build Buildings
        for ( int i = 0 ; i < 30 ; ++i )
        { 
            p_Objects.buildingRows[i] = AddNewRow();
            p_Objects.buildingRows[i].SetZOffset(p_Objects.BuildingZOffset);
            p_Objects.buildingRows[i].SetInitialZ(p_Objects.BuildingZOffset);
            p_Objects.BuildingZOffset += 70;       
        }

        p_Objects.buildingFrontQueue = p_Objects.buildingRows[p_Objects.buildingFrontPointer].m_Buildings[0].transform.position.z;
		++p_Objects.buildingFrontPointer;

		//Instantiate RoadManager
        p_Objects.roadManager = (GameObject)Instantiate(m_Road, new Vector3(0, -89, -91), Quaternion.identity);
    }
	
    BuildingRow AddNewRow()
    {
        BuildingRow hold = new BuildingRow();
        hold.m_Buildings = new List<GameObject>();

        int xOffset = -265;
        for (int i = 0; i < 8; ++i)
        {
            hold.m_Buildings.Add((GameObject)Instantiate(m_Buildings[UnityEngine.Random.Range(0, 17)], new Vector3(0, 0, 0), Quaternion.identity));
            hold.m_Buildings[hold.m_Buildings.Count-1].transform.parent = m_BuildingBase.transform;
            hold.m_Buildings[hold.m_Buildings.Count - 1].transform.localPosition = new Vector3(xOffset, 0, 0);
            if (i == 3)
            {
                xOffset += 100;
            }
            else
            {
                xOffset += 70;
            }
        }

        hold.m_Floor = (GameObject)Instantiate(m_Floor, new Vector3(0, 0, 0), Quaternion.identity);
        hold.m_Floor.transform.parent = m_BuildingBase.transform;
        hold.m_Floor.transform.localPosition = new Vector3(0, -3, 0);

        return hold;
    }

 //   void WraparoundShip(int index)
 //   {
 //       m_ShipsInScene[index].transform.position = new Vector3(m_ShipsInScene[index].transform.position.x, UnityEngine.Random.Range(-15, 20), m_ShipsInScene[m_LastShipInQueue].transform.position.z + (int)m_Obstoffset);
 //       m_LastShipInQueue = index;
 //       m_ShipsInScene[index].GetComponent<shipMove>().Wrapped();
 //       //Debug.Log(index);
 //   }
	
	//void CheckBuildingRowWrap(float p_TriggerZPos)
	//{
	//	if(p_TriggerZPos > m_BuildingFrontQueue)
	//	{
	//		m_BuildingRowsInScene[m_BuildingFrontPointer].SetZOffset(m_ZOffset);
 //           m_ZOffset += 70;
	//		++m_BuildingFrontPointer;
 //           if(m_BuildingFrontPointer == m_BuildingRowsInScene.Length)
 //           {
 //               m_BuildingFrontPointer = 0;
 //           }
 //           m_BuildingFrontQueue = m_BuildingRowsInScene[m_BuildingFrontPointer].m_Buildings[0].transform.position.z;
 //       }
	//}

 //   void ResetEverything()
 //   {
 //       m_BuildingFrontPointer = 0;
 //       for (int i = 0; i < m_BuildingRowsInScene.Length; ++i)
 //       {
 //           m_BuildingRowsInScene[i].SetZOffset(m_BuildingRowsInScene[i].m_ZOffset);
 //       }
 //       m_BuildingFrontQueue = m_BuildingRowsInScene[m_BuildingFrontPointer].m_Buildings[0].transform.position.z;
 //       ++m_BuildingFrontPointer;
 //       m_ZOffset = (int)m_BuildingRowsInScene[m_BuildingRowsInScene.Length - 1].m_ZOffset + 70;
		
		
	//	foreach(GameObject ship in m_ShipsInScene)
	//	{
	//		ship.GetComponent<shipMove>().ResetShip();
	//	}
	//	m_LastShipInQueue = m_ShipsInScene.Count - 1;

 //       m_RoadManager.GetComponent<RoadController>().ResetRoad();


 //   }

 //   void StartShips()
 //   {
 //       foreach (GameObject ship in m_ShipsInScene)
 //       {
 //           ship.GetComponent<shipMove>().StartShip();
 //       }
 //   }

 //   void StopShips()
 //   {
 //       foreach (GameObject ship in m_ShipsInScene)
 //       {
 //           ship.GetComponent<shipMove>().StopShip();
 //       }
 //   }

}
