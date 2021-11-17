using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	float hitPoints;
	float speed;

	Animator animator;

	public int damageStrength;
	[SerializeField]
	bool upsideDown = false;

	bool playerInTrigger;

	void OnEnable(){
		ResetCharacter();

	}

	void Start (){
		animator = GetComponent<Animator>();
	}

	void Update () {
		LookToPlayer();

	}
	#region CHARACTER_MNG
	public override void ResetCharacter(){
		hitPoints = startingHitPoints;
		GetComponent<SpriteRenderer>().color = Color.white;
	}
	public override IEnumerator DamageCharacter(int damage, float interval){
		while(true){
			StartCoroutine(FlickerCharacter());
			hitPoints -= damage;
			if(hitPoints < float.Epsilon){
				KillCharacter();
				break;
			}
			if (interval > float.Epsilon){
				yield return new WaitForSeconds(interval);
			}else{
				break;
			}
		}
	}

	#endregion

	#region COLLISIONS

	private void OnTriggerEnter2D (Collider2D other){
		if(other.CompareTag("Player")){
				animator.SetBool("isAttacking", true);
				playerInTrigger = true;
		}
	}

	private void OnTriggerExit2D (Collider2D other){
		if(other.CompareTag("Player")){
			animator.SetBool("isAttacking", false);
			playerInTrigger = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D other){
		if(other.collider.CompareTag("Player")){
			Player player = GameObject.Find("Player").GetComponent<Player>();
			if(player.DamageCoroutine != null){
				StopCoroutine(player.DamageCoroutine);
			}
			player.DamageCoroutine = StartCoroutine(player.DamageCharacter(1, 1));
		}
	}
	private void OnCollisionExit2D(Collision2D other){
		if(other.collider.CompareTag("Player")){
			Player player = GameObject.Find("Player").GetComponent<Player>();
			if (player.DamageCoroutine != null){
				StopCoroutine(player.DamageCoroutine);
				player.DamageCoroutine = null;
			}
		}
	}

	#endregion

	void DamagePlayer(){
		if (playerInTrigger){
			Player player = GameObject.Find("Player").GetComponent<Player>();
			StartCoroutine(player.DamageCharacter(1, 0));
		}
	}

	void LookToPlayer(){
		GameObject player = GameObject.Find("Player");
		Vector3 playerPos = player.transform.position;
		float yAngle;
		if(playerPos.x < transform.position.x){
			yAngle = upsideDown ? 0 : 180;
			transform.rotation = Quaternion.Euler(new Vector3(0, yAngle, transform.rotation.eulerAngles.z));
		}
		if(playerPos.x > transform.position.x){
			yAngle = upsideDown ? 180 : 0;
			transform.rotation = Quaternion.Euler(new Vector3(0, yAngle, transform.rotation.eulerAngles.z));;
		}

	}

}
