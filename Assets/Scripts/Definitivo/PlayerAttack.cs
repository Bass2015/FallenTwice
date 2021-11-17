using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	public GameObject lightShotPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire2")){
			ShootLight();
		}
	}

	void ShootLight(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 direction = mousePos - transform.position;
		GameObject lightShot = Instantiate(lightShotPrefab, transform.position, Quaternion.identity);
		lightShot.GetComponent<LightShot>().SetDirection(direction);
	}
}

