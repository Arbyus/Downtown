using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct BuildingRow
{
    public List<GameObject> m_Buildings;
    public GameObject m_Floor;
    public float m_ZOffset;
    public void SetZOffset(int offset)
    {
        foreach(GameObject obj in m_Buildings)
        {
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, offset);
        }

        m_Floor.transform.localPosition = new Vector3(m_Floor.transform.localPosition.x, m_Floor.transform.localPosition.y, offset);
    }
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
    float lastIncrease = -700;
    List<GameObject> m_ShipsInScene = new List<GameObject>();
    List<GameObject> m_BuildingsInScene = new List<GameObject>();
    BuildingRow[] m_BuildingRowsInScene = new BuildingRow[30];
    float m_BuildingFrontQueue;

    // Use this for initialization
    Quaternion shipRotation = Quaternion.Euler(270, 0, 180);

    void Place1Obst(int z)
    {
        int BlockLoc = (int)(Random.value * 5);
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc], Random.Range(-15, 20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);
    }

    void Place2Obst(int z)
    {
        int BlockLoc = (int)(Random.value * 5);
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc], Random.Range(-15, 20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);

        int BlockLoc2 = BlockLoc;
        while(BlockLoc2 == BlockLoc)
        {
            BlockLoc2 = (int)(Random.value * 5);
        }
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc2], Random.Range(-15, 20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);

    }

    void Place3Obst(int z)
    {
        int BlockLoc = (int)(Random.value * 5);
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc], Random.Range(-15,20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);

        int BlockLoc2 = BlockLoc;
        while (BlockLoc2 == BlockLoc)
        {
            BlockLoc2 = (int)(Random.value * 5);
        }
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc2], Random.Range(-15, 20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);

        int BlockLoc3 = BlockLoc;
        while (BlockLoc3 == BlockLoc || BlockLoc3 == BlockLoc2)
        {
            BlockLoc3 = (int)(Random.value * 5);
        }
        m_ShipsInScene.Add((GameObject)Instantiate(m_obst, new Vector3(m_RoadOffsets[BlockLoc3], Random.Range(-15, 20), z), shipRotation));
        m_ShipsInScene[m_ShipsInScene.Count - 1].GetComponent<shipMove>().GiveIndex(m_ShipsInScene.Count - 1);
    }

    void Start () {
        m_RoadOffsets = new float[] { -12.8f, -6.4f, 0, 6.4f, 12.8f };

        //build blocks
        for (int i = 0; i < 100; ++i)
        {
            int AmountOfBlocks = (int)(Random.value * 3) + 1;
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

        //lets build some buildings
        int zOffset = -55;
        //for now. 8 per row. 30 rows 240
        for( int i = 0 ; i < 30 ; ++i )
        { 
            m_BuildingRowsInScene[i] = AddNewRow();
            m_BuildingRowsInScene[i].SetZOffset(zOffset);
            zOffset += 70;
            
        }

        PlayerMovement playermov = GameObject.FindGameObjectWithTag("PlayerCont").GetComponent<PlayerMovement>();
        playermov.SetWraparoundCallback(i => WraparoundObject(i));


        m_BuildingFrontQueue = m_BuildingRowsInScene[0].m_Buildings[0].transform.position.z;
        Debug.Log(m_BuildingFrontQueue);
    }

    BuildingRow AddNewRow()
    {
        BuildingRow hold = new BuildingRow();
        hold.m_Buildings = new List<GameObject>();

        int xOffset = -265;
        for (int i = 0; i < 8; ++i)
        {
            hold.m_Buildings.Add((GameObject)Instantiate(m_Buildings[Random.Range(0, 17)], new Vector3(0, 0, 0), Quaternion.identity));
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

    void WraparoundObject(int index)
    {
        m_ShipsInScene[index].GetComponent<shipMove>().m_OffsetPos += m_ObstPointer;
        m_ShipsInScene[index].transform.position = new Vector3(m_ShipsInScene[index].transform.position.x, Random.Range(-15, 20), m_ShipsInScene[index].GetComponent<shipMove>().m_OffsetPos);
        m_ShipsInScene[index].GetComponent<shipMove>().Wrapped();

        Debug.Log(index);
    }
}
