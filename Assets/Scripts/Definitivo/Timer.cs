using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

	Player player;
	//SpriteRenderer mRenderer;
	Coroutine timerCoroutine;
	public GameObject timerPrefab;
	GameObject timer;
	Image timerBar;
	bool flashing;

	Color originalColor;

	public Coroutine TimerCoroutine	{
		get	{
			return timerCoroutine;
		}
	}

	public float maxTime;
	float actualTime;

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		//mRenderer = GetComponent<SpriteRenderer>();
		actualTime = maxTime;
		timer = Instantiate(timerPrefab);
		timerBar = timer.transform.GetChild(1).GetComponent<Image>();
		originalColor = timerBar.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(timerBar.fillAmount < 0.15f && !flashing){
			StartCoroutine(HighlightTimer());
		} else if(timerBar.fillAmount > 0.15f){
			timerBar.color = originalColor;
		}
	}

	public void Activate(){
		if(timerCoroutine != null){
			StopCoroutine(timerCoroutine);
		}
		timerCoroutine = StartCoroutine(StartCountdown());
	}
	public void Deactivate(bool reset){
		if(reset){
			actualTime = maxTime;
			timerBar.fillAmount = 1;
		}else if(timerCoroutine != null){
			StopCoroutine(timerCoroutine);
			timerCoroutine = StartCoroutine(ReverseCountdown());
		}
	}

	IEnumerator StartCountdown(){
		while(actualTime > float.Epsilon){
			actualTime -= Time.deltaTime;
			timerBar.fillAmount = actualTime / maxTime;
			yield return new WaitForEndOfFrame();
		}
		SceneManager.LoadScene(0);
		//player.ResetCharacter();
	}

	IEnumerator ReverseCountdown(){
		while(actualTime < maxTime){
			actualTime += Time.deltaTime;
			timerBar.fillAmount = actualTime / maxTime;

			yield return new WaitForEndOfFrame();
		}
	}



	IEnumerator HighlightTimer(){
		flashing = true;
		while(timerBar.fillAmount < 0.2f){
			if(timerBar.color == originalColor){
				timerBar.color = Color.red;
			}else{
				timerBar.color = originalColor;			
			}
			yield return new WaitForSeconds(0.4f);
		}
		flashing = false;
	}
}
