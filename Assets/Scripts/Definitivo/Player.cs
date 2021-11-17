
//#define TESTING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Player : Character {
	
	public HitPoints hitPoints;

	Rigidbody2D mBody;
	Collider2D mCollider;
	Vector3 initPosition;
	DistanceJoint2D cuerda;

	public Inventory inventoryPrefab;
	Inventory inventory;

	Timer timer;


	bool upsideDown;

	public bool UpsideDown{
		get{
			return upsideDown;
		}
	}


	//Para testear
	public GameObject winText;


	// Use this for initialization
	void Start () {
		mBody = GetComponent<Rigidbody2D>();
		mBody.centerOfMass = Vector2.zero;
		mCollider = GetComponent<Collider2D>();
		cuerda = GetComponent<DistanceJoint2D>();
		timer = GetComponent<Timer>();
		inventory = Instantiate(inventoryPrefab);

		#if TESTING && UNITY_EDITOR
		maxHitPoints = 10000;
		#elif UNITY_EDITOR
		maxHitPoints = 1;
		#endif
		hitPoints.value = maxHitPoints;
		initPosition = transform.position;
		winText.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E) && !CameraManager.instance.Turning && upsideDown){
			if(inventory.HasItem()){
				inventory.UseItem();
			}else{
				SceneManager.LoadScene(0);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

	public void Freeze(bool freeze){
		mBody.simulated = !freeze;
		mCollider.enabled = !freeze;
		if(cuerda.enabled) {
			gameObject.GetComponent<LightWhip>().Detach();
		}
	}

	public void InverseGravity(){
		mBody.gravityScale *= -1;
		upsideDown = !upsideDown;
		if(upsideDown){
			timer.Activate();
		}else{
			timer.Deactivate(false);
		}
	}

	#region Character
	public override void ResetCharacter(){
		if(upsideDown){
			InverseGravity();
		}
		transform.position = initPosition;
		transform.rotation = Quaternion.identity;
		hitPoints.value = maxHitPoints;
		CameraManager.instance.Reset();
		timer.Deactivate(true);
	}

	public override IEnumerator DamageCharacter(int damage, float interval){
		while(true && !CameraManager.instance.Turning){
			StartCoroutine(FlickerCharacter());
			if(!upsideDown){
				hitPoints.value -= damage;
				if (hitPoints.value < float.Epsilon){
					StartCoroutine(Camera.main.GetComponent<CameraManager>().TurnUpsideDown());
				}
			}else{
				ResetCharacter();
			}
			if (interval > float.Epsilon){
				yield return new WaitForSeconds(interval);
			}else{
				break;
			}
		}
	}
	#endregion

	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Potion")){
			inventory.AddItem(other.gameObject);
		}
		if(other.CompareTag("Heart")){
			winText.SetActive(true);
			//Destroy(other.gameObject);
		}
	}

	void OnDrawGizmos(){
		
	}
}
