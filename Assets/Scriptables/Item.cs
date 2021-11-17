using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject {
	public string objectName;
	public int quantity;
	public Sprite sprite;
	public bool stackable;
	public enum ItemType{
		COIN,
		HEALTH, 
		ROCK
	}
	public ItemType type;
}
