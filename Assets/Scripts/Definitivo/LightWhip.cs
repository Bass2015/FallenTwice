using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]

public class LightWhip : MonoBehaviour {

	DistanceJoint2D whip;
	Rigidbody2D body;
	PlayerMovement playerMovement;

	public GameObject eslabon;
	GameObject whipSprite;
	public float range = Mathf.Infinity;
	public float acuracyModifier;


	Quaternion upsideRotation = new Quaternion(0, 0, 0, 1);
	Quaternion upsideDownRotation = new Quaternion(0, 0, 1, 0);

	public bool WhipEnabled(){
		return whip.enabled;
	}
	public float WhipDistance(){
		return whip.distance;
	}

	// Use this for initialization
	void Start () {
		playerMovement = GetComponent<PlayerMovement>();
		body = GetComponent<Rigidbody2D>();
		whip = GetComponent<DistanceJoint2D>();
		whipSprite = Instantiate(eslabon);
		whipSprite.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")){
			StartCoroutine(DrawWhipCoroutine(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
			EngancharOSoltarCuerda();
		}
		if(whip.enabled){
			
			DrawWhip(whip.connectedBody.transform.position);
		}
		
	}

	
	private Quaternion originalRotation;

	void EngancharOSoltarCuerda(){
		GameObject hitObject = HitObject();
		if (whip.enabled){
			Detach();
		}else if (hitObject != null){
			originalRotation = transform.rotation;
			if (hitObject.CompareTag("Anchor") && !whip.enabled){
				Attach(hitObject.GetComponent<Rigidbody2D>());
			}else if (hitObject.CompareTag("Switch")){
				hitObject.GetComponent<Switch>().Activate();
				StartCoroutine(FeedSwitch(hitObject.GetComponent<Rigidbody2D>()));
			}
		}
	}

	IEnumerator FeedSwitch(Rigidbody2D switchBody){
		Attach(switchBody);
		yield return new WaitForSeconds(5);
		Detach();
	}


	void Attach(Rigidbody2D rigidBodyToConnect){
		whip.connectedBody = rigidBodyToConnect;
		whip.distance = Vector3.Distance(transform.position, rigidBodyToConnect.transform.position);
		whip.enabled = true;
		if(!playerMovement.CheckOnGround()){
			originalRotation = transform.rotation;
			body.freezeRotation = false;
		}
	}

	GameObject HitObject(){
		Vector3 toMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		Vector3 direction = toMousePos.normalized;
		int mask = LayerMask.GetMask("WhipTarget");
		RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position + direction * 0.5f, acuracyModifier, direction, range, playerMovement.anyLayerMask);
//		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, range, playerScript.anyLayerMask);
		if (hitInfo.collider == null){
			return null;
		}else{
			return hitInfo.collider.gameObject;
		}
	}

	public void Detach(){
		
		whip.enabled = false;
		body.freezeRotation = true;
		if(gameObject.GetComponent<Player>().UpsideDown){
			transform.rotation = upsideDownRotation;
		}else{
			transform.rotation = upsideRotation;
		}
		/*Añade un pequeño impulso hacia arriba al soltarse, para que pueda subir a sitios
		un poco más altos.*/
		body.AddForce(new Vector2(0f, body.velocity.y) / whip.distance, ForceMode2D.Impulse);
		if(whipSprite.activeSelf){
			whipSprite.SetActive(false);
		}
	}

	//Draws a light whip from position to 'target';
	void DrawWhip(Vector3 target){
		target = new Vector3(target.x, target.y, 0);
		Vector3 direction = target - transform.position;
		whipSprite.transform.position = transform.position + direction / 2	;
		whipSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, FindAngleDeg(direction, transform.position)));
		if(whip.enabled){
			whipSprite.transform.localScale = new Vector3(Vector3.Distance(target, transform.position), 1, 1);
		}
		if(!whipSprite.activeSelf)
			whipSprite.SetActive(true);
	}

	IEnumerator DrawWhipCoroutine (Vector3 target){
		DrawWhip(target);
		yield return new WaitForSeconds(0.07f);
		whipSprite.SetActive(false);
	}

	float FindAngleDeg(Vector3 direction, Vector3 position)	{
		Vector3 right = new Vector3(position.x + 1, position.y, 0) - position;
		Debug.DrawRay(transform.position, right);
		float cos = Vector3.Dot(right, direction) / (right.magnitude * direction.magnitude);
		float angle;
		if(direction.y < 0){
			//Estoy arriba
			angle = Mathf.Acos(	cos * -1);
		}else{
			angle = Mathf.Acos(cos);
		}
		return angle * Mathf.Rad2Deg;
	}


	void OnDrawGizmos(){
//		Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), acuracyModifier);
//		Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), acuracyModifier);
	}
}
