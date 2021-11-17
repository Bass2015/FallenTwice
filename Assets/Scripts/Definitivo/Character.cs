using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

	public float startingHitPoints;
	public float maxHitPoints;
	protected Coroutine damageCoroutine;

	public Coroutine DamageCoroutine {
		get {
			return damageCoroutine;
		}
		set {
			this.damageCoroutine = value;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void KillCharacter(){
		if(gameObject != null){
			gameObject.SetActive(false);
		}
	}

	public abstract void ResetCharacter();

	public abstract IEnumerator DamageCharacter(int damage, float interval);

	public virtual IEnumerator FlickerCharacter(){
		GetComponent<SpriteRenderer>().color = Color.red;
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer>().color = Color.white;
	}
}
