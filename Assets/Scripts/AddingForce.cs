using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddingForce : MonoBehaviour {

	enum ModeSwitching {Start, Impulse, Force};
	ModeSwitching m_ModeSwitching;

	ForceMode2D m_ForceMode;
	Vector2 m_StartPosition;

	Vector2 m_NewForce;

	Rigidbody2D m_Rigidbody;

	// Use this for initialization
	void Start () {
		m_Rigidbody = GetComponent <Rigidbody2D>();
		m_ModeSwitching = ModeSwitching.Start;
		m_NewForce = new Vector2(-5.0f, 1.0f);

		m_StartPosition = m_Rigidbody.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		switch(m_ModeSwitching) {
			case ModeSwitching.Start:
				m_Rigidbody.transform.position = m_StartPosition;
				m_Rigidbody.velocity = Vector2.zero;
				break;
			case ModeSwitching.Force:
				m_NewForce = new Vector2(0, 1.0f);
				m_Rigidbody.AddForce(m_NewForce, ForceMode2D.Force);
				break;
			case ModeSwitching.Impulse:
				if (Input.GetButtonDown("Jump"))
				{
					m_NewForce = new Vector2(0, 1.0f);
					m_Rigidbody.AddForce(m_NewForce, ForceMode2D.Impulse);
				}
				break;
		
		}
	}

	void OnGUI(){
		if(GUI.Button(new Rect(100, 0, 150, 30), "reset")){
			m_ModeSwitching = ModeSwitching.Start;
		}
		if(GUI.Button(new Rect(100, 60, 150, 30), "Impulse")){
			m_ModeSwitching = ModeSwitching.Impulse;
		}
		if(GUI.Button(new Rect(100, 90, 150, 30), "Force")){
			m_ModeSwitching = ModeSwitching.Force;
		}
	}
}
