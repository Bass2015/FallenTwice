using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	Player player;
	GameObject item;
	GameObject slot;
	Image itemImage;

	public bool HasItem(){
		return item != null;
	}

	// Use this for initialization
	void Start () {
		slot = transform.GetChild(0).gameObject;
		itemImage = slot.transform.GetChild(1).GetComponent<Image>();
		itemImage.enabled = false;
		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool AddItem(GameObject itemToAdd){
		if (item == null){
			item = itemToAdd;
			itemImage.sprite = itemToAdd.GetComponent<SpriteRenderer>().sprite;
			itemImage.enabled = true;
			item.SetActive(false);
			return true;
		}
		return false;
	}

	public void UseItem(){
		itemImage.enabled = false;
		Destroy(item);
		StartCoroutine(CameraManager.instance.TurnUpsideDown());	
	}
}