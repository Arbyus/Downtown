using UnityEngine;
using System.Collections;

public class shipMove : MonoBehaviour {

    Vector3 m_CurrentPos;
    Vector3 m_StartPos;
    float m_ShipSpeed = 0;
    public float m_OffsetPos { get; set; }
    int m_Index;

	// Use this for initialization
	void Start () {
        m_CurrentPos = this.transform.position;
        m_StartPos = this.transform.position;
        m_OffsetPos = this.transform.position.z;
    }

    public void GiveIndex(int p_ShipIndex)
    {
        m_Index = p_ShipIndex;
    }

    public int GetIndex()
    {
        return m_Index;
    }

	// Update is called once per frame
	void Update ()
    {
        m_CurrentPos.z -= m_ShipSpeed;
        this.transform.position = m_CurrentPos;
    }

    public void Wrapped()
    {
        m_CurrentPos = this.transform.position;
    }

    public void ResetShip()
    {
        this.transform.position = m_StartPos;
        m_CurrentPos = this.transform.position;
    }

    public void StartShip()
    {
        m_ShipSpeed = 0.8f;
    }

    public void StopShip()
    {
        m_ShipSpeed = 0;
    }

}
