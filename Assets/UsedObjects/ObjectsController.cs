using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public struct GameObjectContainer
{
   public List<GameObject> ships;
   public BuildingRow[] buildingRows;
   public GameObject roadManager;
   public float buildingFrontQueue;
   public int BuildingZOffset;
   public float shipZOffset;
   public int buildingFrontPointer;
   public int lastShipInQueue;
    public void Initialise()
    {
        ships = new List<GameObject>();
        buildingRows = new BuildingRow[30];
        shipZOffset = 70;
        BuildingZOffset = -55;
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


public class ObjectsController : MonoBehaviour {
	GameObjectContainer m_Objects;

    void Start () {
        m_Objects = new GameObjectContainer();
        m_Objects.Initialise();
        GetComponent<LoadLevel>().BuildGameObjects(ref m_Objects);

        CallbackFunctions CallbacksforPlayerMovement = new CallbackFunctions();
        CallbacksforPlayerMovement.m_ResetGame = ResetEverything;
        CallbacksforPlayerMovement.m_StartShip = StartShips;
        CallbacksforPlayerMovement.m_StopShip = StopShips;
        CallbacksforPlayerMovement.m_WrapBuildingRow = f => CheckBuildingRowWrap(f);
        CallbacksforPlayerMovement.m_WrapShip = i => WraparoundShip(i);

        PlayerMovement playermov = GameObject.FindGameObjectWithTag("PlayerCont").GetComponent<PlayerMovement>();
        playermov.SetAllCallbacks(CallbacksforPlayerMovement);
    }

    void WraparoundShip(int index)
    {
        m_Objects.ships[index].transform.position = new Vector3(m_Objects.ships[index].transform.position.x, UnityEngine.Random.Range(-15, 20), m_Objects.ships[m_Objects.lastShipInQueue].transform.position.z + (int)m_Objects.shipZOffset);
        m_Objects.lastShipInQueue = index;
        m_Objects.ships[index].GetComponent<shipMove>().Wrapped();
    }
	
	void CheckBuildingRowWrap(float p_TriggerZPos)
	{
		if(p_TriggerZPos > m_Objects.buildingFrontQueue)
		{
            m_Objects.buildingRows[m_Objects.buildingFrontPointer].SetZOffset(m_Objects.BuildingZOffset);
            m_Objects.BuildingZOffset += 70;
			++m_Objects.buildingFrontPointer;
            if(m_Objects.buildingFrontPointer == m_Objects.buildingRows.Length)
            {
                m_Objects.buildingFrontPointer = 0;
            }
            m_Objects.buildingFrontQueue = m_Objects.buildingRows[m_Objects.buildingFrontPointer].m_Buildings[0].transform.position.z;
        }
	}

    void ResetEverything()
    {
        m_Objects.buildingFrontPointer = 0;
        for (int i = 0; i < m_Objects.buildingRows.Length; ++i)
        {
            m_Objects.buildingRows[i].SetZOffset(m_Objects.buildingRows[i].m_ZOffset);
        }
        m_Objects.buildingFrontQueue = m_Objects.buildingRows[m_Objects.buildingFrontPointer].m_Buildings[0].transform.position.z;
        ++m_Objects.buildingFrontPointer;
        m_Objects.shipZOffset = (int)m_Objects.buildingRows[m_Objects.buildingRows.Length - 1].m_ZOffset + 70;
		
		
		foreach(GameObject ship in m_Objects.ships)
		{
			ship.GetComponent<shipMove>().ResetShip();
		}
        m_Objects.lastShipInQueue = m_Objects.ships.Count - 1;

        m_Objects.roadManager.GetComponent<RoadController>().ResetRoad();


    }

    void StartShips()
    {
        foreach (GameObject ship in m_Objects.ships)
        {
            ship.GetComponent<shipMove>().StartShip();
        }
    }

    void StopShips()
    {
        foreach (GameObject ship in m_Objects.ships)
        {
            ship.GetComponent<shipMove>().StopShip();
        }
    }

}
