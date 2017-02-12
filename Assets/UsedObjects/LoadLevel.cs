using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

struct BuildingRow
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

public struct CallbackFunctions
{
    public Action<int> m_WrapShip;
    public Action m_ResetGame;
    public Action m_StartShip;
    public Action m_StopShip;
    public Action<float> m_WrapBuildingRow;
}


public class LoadLevel : MonoBehaviour {
    public GameObject m_obst;
    public GameObject m_Floor;
    public GameObject m_Ship;
    public GameObject[] m_Buildings;
    public GameObject m_BuildingBase;
    int m_ObstPointer = -500;
    float m_Obstoffset = 70;
    float[] m_RoadOffsets;
    List<GameObject> m_ShipsInScene = new List<GameObject>();
    int m_LastShipInQueue = 0;
    BuildingRow[] m_BuildingRowsInScene = new BuildingRow[30];
    float m_BuildingFrontQueue;
	int m_ZOffset = -55;
	int m_BuildingFrontPointer = 0;
	
    // Use this for initialization
    Quaternion shipRotation = Quaternion.Euler(270, 0, 180);

    void Place1Obst(int z)
    {
        int BlockLoc = (int)(UnityEngine.Random.value * 5);
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc], UnityEngine.Random.Range(-15, 20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);
    }

    void Place2Obst(int z)
    {
        int BlockLoc = (int)(UnityEngine.Random.value * 5);
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc], UnityEngine.Random.Range(-15, 20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);

        int BlockLoc2 = BlockLoc;
        while(BlockLoc2 == BlockLoc)
        {
            BlockLoc2 = (int)(UnityEngine.Random.value * 5);
        }
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc2], UnityEngine.Random.Range(-15, 20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);

    }

    void Place3Obst(int z)
    {
        int BlockLoc = (int)(UnityEngine.Random.value * 5);
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc], UnityEngine.Random.Range(-15,20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);

        int BlockLoc2 = BlockLoc;
        while (BlockLoc2 == BlockLoc)
        {
            BlockLoc2 = (int)(UnityEngine.Random.value * 5);
        }
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc2], UnityEngine.Random.Range(-15, 20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);

        int BlockLoc3 = BlockLoc;
        while (BlockLoc3 == BlockLoc || BlockLoc3 == BlockLoc2)
        {
            BlockLoc3 = (int)(UnityEngine.Random.value * 5);
        }
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc3], UnityEngine.Random.Range(-15, 20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);
    }

    void Start () {
        m_RoadOffsets = new float[] { -12.8f, -6.4f, 0, 6.4f, 12.8f };

        //build blocks
        for (int i = 0; i < 100; ++i)
        {
            int AmountOfBlocks = (int)(UnityEngine.Random.value * 3) + 1;
            if (AmountOfBlocks == 1)
            {
                Place1Obst(m_ObstPointer);
            }
            else if (AmountOfBlocks == 2)
            {
                Place2Obst(m_ObstPointer);
            }
            else if (AmountOfBlocks == 3)
            {
                Place3Obst(m_ObstPointer);
            }
            m_ObstPointer += (int)m_Obstoffset;
        }
        m_LastShipInQueue = m_ShipsInScene.Count - 1;

        //lets build some buildings

        //for now. 8 per row. 30 rows 240
        for ( int i = 0 ; i < 30 ; ++i )
        { 
            m_BuildingRowsInScene[i] = AddNewRow();
            m_BuildingRowsInScene[i].SetZOffset(m_ZOffset);
            m_BuildingRowsInScene[i].SetInitialZ(m_ZOffset);
            m_ZOffset += 70;       
        }
        CallbackFunctions CallbacksforPlayerMovement = new CallbackFunctions();
        CallbacksforPlayerMovement.m_ResetGame = ResetEverything;
        CallbacksforPlayerMovement.m_StartShip = StartShips;
        CallbacksforPlayerMovement.m_StopShip = StopShips;
        CallbacksforPlayerMovement.m_WrapBuildingRow = f => CheckBuildingRowWrap(f);
        CallbacksforPlayerMovement.m_WrapShip = i => WraparoundShip(i);

        PlayerMovement playermov = GameObject.FindGameObjectWithTag("PlayerCont").GetComponent<PlayerMovement>();
        playermov.SetAllCallbacks(CallbacksforPlayerMovement);

        m_BuildingFrontQueue = m_BuildingRowsInScene[m_BuildingFrontPointer].m_Buildings[0].transform.position.z;
		++m_BuildingFrontPointer;
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
