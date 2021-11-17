using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public float openingTime;
	public float openingSpeed;
	bool opened;
	SpriteRenderer wire;
	Color wireColor;

	// Use this for initialization
	void Start () {
		wire = transform.parent.GetChild(1).GetComponent<SpriteRenderer>();
		wireColor = wire.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)) {
			StartCoroutine(Open());
		}
	}

	public IEnumerator Open(){
		float percentageComplete = 0;
		Vector3 direction = opened ? Vector3.down : Vector3.up;
		while(percentageComplete < openingTime){
			wire.color = Color.yellow;
			transform.Translate(direction * openingSpeed * Time.deltaTime);
			percentageComplete += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		wire.color = wireColor;
		opened = !opened;
	}
}
