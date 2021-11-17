using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public GameObject enemyToActivate;
	public float repeatInterval;
	bool alreadyActivated;

	// Use this for initialization
	void Start () {
//		if(repeatInterval > 0){
//			InvokeRepeating("SpawnObject", 0.0f, repeatInterval);
//		}
	}


	void Update(){
		if(!enemyToActivate.activeSelf && !alreadyActivated){
			Invoke("ActivateEnemy", repeatInterval);
			alreadyActivated = true;
		}else if(enemyToActivate.activeSelf){
			alreadyActivated = false;
		}
	}

	void ActivateEnemy(){
		enemyToActivate.SetActive(true);
	}

	public GameObject SpawnObject(){
		if(enemyToActivate != null){
			return Instantiate(enemyToActivate, transform.position, Quaternion.identity);
		}
		return null;
	}
}
