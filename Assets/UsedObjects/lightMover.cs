using UnityEngine;
using System.Collections;

public class lightMover : MonoBehaviour {

    Vector3 m_Pos;
    bool m_Dir;

	// Use this for initialization
	void Start () {
        m_Pos = this.transform.position;
        m_Dir = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Dir)
        {
            m_Pos.z += 0.05f;
            if(m_Pos.z > 51)
            {
                m_Dir = !m_Dir;
            }
        }
        else
        {
            m_Pos.z -= 0.05f;
            if (m_Pos.z < 44)
            {
                m_Dir = !m_Dir;
            }
        }
        this.transform.position = m_Pos;
    }
}
