using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	CameraManager mainCamera;

	public static GameManager instance = null;
	public SpawnPoint playerSpawnPoint;
	public SpawnPoint enemySpawnPonint;
	Player player;

	void Awake () {
		if(instance != null && instance != this){
			Destroy(gameObject);
		}else{
			instance = this;
		}
		Cursor.visible = false;
	}

	void Start(){
		FindCamera();
		SetupScene();
	}

	void FindCamera(){
		GameObject camera = GameObject.Find("Main Camera");
		if (camera != null){
			mainCamera = camera.GetComponent <CameraManager>();
		}
	}

	void SetupScene(){
		SpawnPlayer();
	}

	void SpawnPlayer(){
		if (playerSpawnPoint != null){
			GameObject playerObject = playerSpawnPoint.SpawnObject();
			player = playerObject.GetComponent<Player>();
			mainCamera.PlayerTransform = player.transform;
		}
	}
}
