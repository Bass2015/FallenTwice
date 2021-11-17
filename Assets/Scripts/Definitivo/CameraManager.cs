using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public static CameraManager instance = null;
	Player player;
	Transform playerTransform;
	public Transform PlayerTransform{
		set{
			playerTransform = value;
		}
	}

	bool turning;

	public bool Turning	{
		get	{
			return turning;
		}
	}

	Vector3 upOffset = new Vector3(0, 1.5f, -10);
	Vector3 currentOffset;


	Quaternion newRotation;
	public float rotateDuration;
	public float angularSpeed;

	void Awake(){
		if(instance != null && instance != this){
			Destroy(gameObject);
		}else{
			instance = this;
		}
	}
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		playerTransform = player.gameObject.transform;
		currentOffset = upOffset;
	}
	
	// Update is called once per frame
	void Update () {
		if (!turning)
			FollowPlayer();

		if(Input.GetKeyDown(KeyCode.P) && !turning){
			#if UNITY_EDITOR 
			StartCoroutine(TurnUpsideDown());
			#endif
		}
	}


	void FollowPlayer(){
		if (playerTransform != null){
			Vector3 newPos = playerTransform.position + currentOffset;
			newPos = Vector3.Slerp(transform.position, newPos, Time.deltaTime * 5);
			if (player.gameObject.GetComponent<Rigidbody2D>().velocity.y != 0){
				newPos = new Vector3(newPos.x, playerTransform.position.y + currentOffset.y, newPos.z);
			}
			transform.position = newPos;
		}
	}

	public IEnumerator TurnUpsideDown(){
		float percentComplete = 0;
		float fromAngle = transform.eulerAngles.z;
		float toAngle = transform.eulerAngles.z < 0.001 ? 180 : 0;
		Vector3 anchor = new Vector3(playerTransform.position.x, 0, 0);
		player.Freeze(true);
		turning = true;
		while (percentComplete < 1){
			percentComplete += Time.deltaTime / rotateDuration;
			float previousRotation = transform.eulerAngles.z;
			float totalRotation = Mathf.SmoothStep(fromAngle, toAngle, percentComplete);
			float angle = totalRotation - previousRotation;
			transform.RotateAround(anchor, Vector3.forward, angle);
			playerTransform.RotateAround(anchor, Vector3.forward, angle);

			yield return null;
		}
		player.InverseGravity();
		currentOffset.x *= -1;
		currentOffset.y *= -1;
		turning = false;
		player.Freeze(false);
	}

	public void Reset(){
		transform.rotation = Quaternion.identity;
		currentOffset = upOffset;
		turning = false;
	}


}
