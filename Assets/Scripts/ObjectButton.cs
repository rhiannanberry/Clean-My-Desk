using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectButton : MonoBehaviour {
	private GameObject prefab;

	private int itemCount;
	
	public void SetSelected() {
		GameController.Instance.selected = gameObject;
	}

	public void SetValues(GameObject prefab, int itemCount) {
		this.prefab = prefab;
		this.itemCount = itemCount;
	}

	//grey out button when none left

	public Transform SpawnObject(Vector3 pos) {
		if (itemCount >= 1) {
			itemCount--;
			return Instantiate(prefab, pos, Random.rotation).transform;
		}
		return null;
	}

	public void DespawnObject(GameObject obj) {
		Destroy(obj); //continue move away from Spawn item and related stuff
		itemCount++;
	}
}
