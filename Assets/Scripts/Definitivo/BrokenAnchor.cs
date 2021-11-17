using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenAnchor : MonoBehaviour {
	SpriteRenderer rend;
	// Use this for initialization
	void Start () {
		rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Color newColor = new Color(rend.color.r, rend.color.g, rend.color.b, Random.Range(0, 1.0f));
		rend.color = newColor;
	}
}
