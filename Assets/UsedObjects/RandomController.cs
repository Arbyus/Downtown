﻿using UnityEngine;
using System.Collections;

public class RandomController : MonoBehaviour {

    int m_Seed;
	string m_StringSeed;
	
    // Use this for initialization
    void Start () {
        Object.DontDestroyOnLoad(this);
        m_Seed = Random.Range(-int.MaxValue, int.MaxValue);
        Random.InitState(m_Seed);
    }

    void SetSeed( int p_NewSeed )
    {
        Random.InitState(p_NewSeed);
        m_Seed = p_NewSeed;
    }

    int GetSeed()
    {
        return m_Seed;
    }       
	
	void AddToSeed(char p_SeedAddition)
	{
		m_Seed += p_SeedAddition;
	}
	
}
