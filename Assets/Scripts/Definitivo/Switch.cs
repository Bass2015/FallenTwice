using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

	public GameObject door;

	public void Activate(){
		StartCoroutine(door.GetComponent<Door>().Open());
	}
}
