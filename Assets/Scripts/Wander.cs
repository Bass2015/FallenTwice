using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class Wander : MonoBehaviour {
	public float pursuitSpeed;
	public float wanderSpeed;
	float currentSpeed;

	public float directionChangeInterval;

	public bool followPlayer;
	bool touchingPlayer;

	Coroutine moveCoroutine;
	Rigidbody2D rb2d;
	Animator animator;
	Transform targetTransform = null;
	Vector3 endPosition;

	void Start(){
		animator = GetComponent<Animator>();
		rb2d = GetComponent < Rigidbody2D>();
		currentSpeed = wanderSpeed;
		StartCoroutine(WanderRoutine());
	}

	public IEnumerator WanderRoutine(){
		while (!touchingPlayer)
		{
			ChooseNewEndpoint();
			if(moveCoroutine != null){
				StopCoroutine(moveCoroutine);
			}
			moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}

	void ChooseNewEndpoint(){
		float angle = Random.Range(0, 360);
		angle = Mathf.Repeat(angle, 360);
		endPosition = transform.position - Vector3FromAngle(angle);
	}

	Vector3 Vector3FromAngle(float inputAngle){
		float angleRadians = Mathf.Deg2Rad * inputAngle;
		return new Vector3(Mathf.Cos(angleRadians) * 3, Mathf.Sin(angleRadians) * 3, 0);
	}

	public IEnumerator Move (Rigidbody2D rbToMove, float speed){
		float remainingDistance = (transform.position - endPosition).sqrMagnitude;

		while(remainingDistance > float.Epsilon){
			if(targetTransform != null){
				endPosition = targetTransform.position;
			}
			if(rbToMove != null){
				animator.SetBool("isMoving", true);
				Vector3 newPosition = Vector3.MoveTowards(rbToMove.position, endPosition, speed * Time.deltaTime);
				rb2d.MovePosition(newPosition);
				animator.SetFloat("LookingAt", endPosition.x - transform.position.x);
				remainingDistance = (transform.position - endPosition).sqrMagnitude;
			}
			yield return new WaitForFixedUpdate();
		}
		animator.SetBool("isMoving", false);
	}

	void OnCollisionEnter2D (Collision2D other){
		if(other.gameObject.CompareTag("Wall")){
			ChooseNewEndpoint();
		}
		if(other.gameObject.CompareTag("Player")){
			touchingPlayer = true;
			if(moveCoroutine != null){
				StopCoroutine(moveCoroutine);
			}
		}
	}
	void OnCollisionExit2D (Collision2D other){
		if(other.gameObject.CompareTag("Player")){
			touchingPlayer = false;
		}
	}

	void OnTriggerEnter2D (Collider2D other){
		if(other.CompareTag("Player") && followPlayer && !touchingPlayer){
			currentSpeed = pursuitSpeed;
			targetTransform = other.transform;
			if(moveCoroutine != null){
				StopCoroutine(moveCoroutine);
			}
			moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.CompareTag("Player")){
			currentSpeed = wanderSpeed;
			targetTransform = null;
			animator.SetBool("isMoving", false);
			if(moveCoroutine != null){
				StopCoroutine(moveCoroutine);
			}
		}
	}
}
