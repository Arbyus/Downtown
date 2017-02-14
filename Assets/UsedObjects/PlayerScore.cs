using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour {

    int m_Score;

	void Start()
	{
		m_Score = 0;
	}
	
	//Amount to add is optional, there incase a double points system
    public void AddScore(int p_ScoreToAdd = 1)
    {
        m_Score += p_ScoreToAdd;
    }

    public void ResetScore()
    {
        m_Score = 0;
    }

	public string SerializeScore()
	{
		return "PScore:" + Convert.ToString(m_Score);
	}
	
}