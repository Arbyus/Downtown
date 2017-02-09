using UnityEngine;
using System.Collections;

public class shipMove : MonoBehaviour {

    Vector3 m_CurrentPos;
    Vector3 m_StartPos;
    public float m_OffsetPos { get; set; }
    int m_Index;
    PlayerMovement m_playermov;

	// Use this for initialization
	void Start () {
        m_CurrentPos = this.transform.position;
        m_StartPos = this.transform.position;
        m_OffsetPos = this.transform.position.z;
        m_playermov = GameObject.FindGameObjectWithTag("PlayerCont").GetComponent<PlayerMovement>();
        m_playermov.SetResetShipCallback(() => ResetShip());
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
        m_CurrentPos.z -= m_playermov.m_ShipMoveAmount;
        this.transform.position = m_CurrentPos;
    }

    public void Wrapped()
    {
        m_CurrentPos = this.transform.position;
    }

    void ResetShip()
    {
        this.transform.position = m_StartPos;
        m_CurrentPos = this.transform.position;
    }

}
