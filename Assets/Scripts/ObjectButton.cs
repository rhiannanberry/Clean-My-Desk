using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObjectButton : MonoBehaviour {
	private GameObject prefab;
	private Text counterText;

	private int itemCount;
	private bool itemLimit = true;

	void Start() {
		if (SceneManager.GetActiveScene().name == "GodMode") {
			itemLimit = false;
			transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
			transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
		}
	}
	
	public void SetSelected() {
		GameController.Instance.selected = gameObject;
	}

	public void SetValues(GameObject prefab, int itemCount) {
		this.prefab = prefab;
		this.itemCount = itemCount;
		counterText = transform.Find("Counter/CountText").GetComponent<Text>();
		UpdateCounter();
	}

	//grey out button when none left

	public Transform SpawnObject(Vector3 pos) {
		if (itemCount >= 1) {
			if (itemLimit) {
				itemCount--;
				UpdateCounter();
			}
			prefab.GetComponent<Obj>().buttonReference = gameObject;
			return Instantiate(prefab, pos, Random.rotation).transform;
		}
		return null;
	}

	public void DespawnObject(GameObject obj) {
		Destroy(obj); //continue move away from Spawn item and related stuff
		if (itemLimit) {
			itemCount++;
			UpdateCounter();
		}
	}

	private void UpdateCounter() {
		counterText.text = itemCount.ToString();
	}
}
