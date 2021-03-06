﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

    public GameObject m_Ship;
    Vector3 m_PlayerPos;
    Vector3 m_StartPos;
    public bool m_TriggerStart;
    bool m_GameStart;
    //bool m_IsJumping;
    //bool m_TwiceTick;
    CallbackFunctions m_CallbackFunctions;
    float push = 3;
    float thrust = 100;
    public Light m_HealthLight;
	float TriggerPosZOffset = 132;
	
    // Use this for initialization
    void Start () {
        m_StartPos = this.transform.position;
        m_PlayerPos = this.transform.position;
        m_TriggerStart = false;
        m_GameStart = false;
        //m_IsJumping = false;
        //m_TwiceTick = false;
    }

    void OnTriggerEnter(Collider other)
    {      
        if(other.gameObject.layer == 8)
        {
            m_CallbackFunctions.m_WrapShip(other.GetComponent<shipMove>().GetIndex());
        }
    }

	void ResetGame()
	{
        m_CallbackFunctions.m_ResetGame();
        this.transform.position = m_StartPos;
        m_HealthLight.intensity = 2;
		push = 3;
		thrust = 100;
		this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
	}
	
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Obst")
        {
            ResetGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !m_GameStart)
        {
            m_GameStart = true;
        }

        else if (GvrController.State == GvrConnectionState.Connected && m_GameStart)
        {
            Vector3 v = GvrController.Orientation * Vector3.forward;

            if (!m_TriggerStart)
            {
                if (v.y < 0.1 && v.y > -0.1)
                {
                    m_CallbackFunctions.m_StartShip();
                    m_TriggerStart = true;
                }
            }
        }

        // if ((GvrController.ClickButtonDown || Input.GetKeyDown(KeyCode.Space)) && !m_IsJumping && !m_TwiceTick && m_TriggerStart)
        // {
            // m_JumpLogic.Jump();
            // m_IsJumping = true;
        // }

        if (m_TriggerStart)
        {
            m_CallbackFunctions.m_WrapBuildingRow(this.transform.position.z - TriggerPosZOffset);
        }
    }

    void FixedUpdate()
    {
        if (m_TriggerStart)
        {
            //this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 16 * this.GetComponent<Rigidbody>().mass));
            //if (this.GetComponent<Rigidbody>().velocity.z > 20)
            //{
            //    this.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity.normalized * 20;
            //}
            // if (m_IsJumping)
            // {
                // if(!m_TwiceTick)
                // {
                    // float pushforce = 18f * (float)this.GetComponent<Rigidbody>().mass;
                    // this.GetComponent<Rigidbody>().AddForce(new Vector3(0, pushforce, 0), ForceMode.Impulse);
                    // m_TwiceTick = true;
                // }
                // else
                // {
                    // float pushforce = 18f * (float)this.GetComponent<Rigidbody>().mass;
                    // this.GetComponent<Rigidbody>().AddForce(new Vector3(0, pushforce, 0), ForceMode.Impulse);
                    // m_TwiceTick = false;
                // }
            // }
            // if(Math.Abs(this.GetComponent<Rigidbody>().velocity.y)>5 && m_IsJumping == true)
            // {
                // m_IsJumping = false;
            // }
            
			this.GetComponent<Rigidbody>().velocity = new Vector3( this.GetComponent<Rigidbody>().velocity.x, this.GetComponent<Rigidbody>().velocity.y, thrust);

            Vector3 v = GvrController.Orientation * Vector3.forward;

            Vector3 v2 = GvrController.Orientation * Vector3.up;
            if(v2.y < 0)
            {
                v2.y = 0;
            }

            v2.y -= 0.5f;

            m_PlayerPos = this.transform.position;
            m_PlayerPos.x += v.y;

            m_PlayerPos.y += -(v2.y*1.5f) + (push - 3);

            if (m_PlayerPos.y > 20)
            {
                m_PlayerPos.y = 20;
            }
            else if (m_PlayerPos.y < -15)
            {
                m_PlayerPos.y = -15;
            }

                Quaternion srot = m_Ship.transform.rotation;
            srot.x = (v.y/1.9f);
            srot.z = (v2.y / 2.9f);
            m_Ship.transform.rotation = srot;

            //m_PlayerPos.x = 0;

            this.transform.position = m_PlayerPos;
            Vector3 shipOffset = m_PlayerPos;
            shipOffset.y += 1;
            shipOffset.z -= 2;
            m_Ship.transform.position = shipOffset;

            CheckOffRoad();
			
            if(thrust < 1)
			{
				ResetGame();
			}
        }
    }

    public void SetAllCallbacks(CallbackFunctions p_Callbacks)
    {
        m_CallbackFunctions = p_Callbacks;
    }

    private void CheckOffRoad()
    {
        if (m_Ship.transform.position.x < -22 || m_Ship.transform.position.x > 11)
        {
            //m_Health -= 0.1f;
            m_HealthLight.intensity -= 0.01f;
            push -= 0.05f;
            thrust -= 0.2f;
        }
        else
        {
            if(m_HealthLight.intensity < 2)
            {
                m_HealthLight.intensity += 0.01f;
            }
			if(push < 3)
			{
				push += 0.05f;
			}
			if(thrust < 100)
			{
				thrust += 0.2f;
			}
        }
    }

}
