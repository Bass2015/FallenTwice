using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class LightShot : MonoBehaviour {
	
	public float speed;
	float distanceTraveled = 0;
	public float maxDistance;

	Vector3 direction;

	// Use this for initialization
	void Start () {
		 
	}

	public void SetDirection(Vector3 direction){
		this.direction = direction.normalized;
	}

	// Update is called once per frame
	void Update () {
			Vector3 distance = direction * speed * Time.deltaTime;
			transform.Translate(distance);
			distanceTraveled += distance.magnitude;
			if (distanceTraveled > maxDistance){
				Destroy(gameObject);
			}
	}

	void OnTriggerEnter2D (Collider2D other){
		if (!other.isTrigger && other.CompareTag("Enemy")){
			Enemy enemy = other.gameObject.GetComponent<Enemy>();
			if(enemy.DamageCoroutine != null){
				StopCoroutine(enemy.DamageCoroutine);
			}
			enemy.DamageCoroutine = StartCoroutine(enemy.DamageCharacter(1, 0));
		}
	}

}
