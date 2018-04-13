﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ObjectButton : MonoBehaviour {
	private GameObject prefab, phantomPrefab;
	private TextMeshProUGUI counterText;

	[HideInInspector]
	public int itemCount;
	private bool itemLimit = true;

	void Start() {
		if (SceneManager.GetActiveScene().name == "GodMode") {
			itemLimit = false;
			transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
		}
	}
	
	public void SetSelected() {
		GameController.Instance.selected = gameObject;
	}

	public void SetValues(GameObject prefab, int itemCount) {
		this.prefab = prefab;
		this.prefab.GetComponent<Obj>().buttonReference = gameObject;
		this.itemCount = itemCount;
		counterText = transform.Find("Counter/CountText").GetComponent<TextMeshProUGUI>();
		UpdateCounter();
	}

	//grey out button when none left

	public Transform SpawnObject(Vector3 pos) {
		if (itemCount >= 1) {
			if (itemLimit) {
				itemCount--;
				UpdateCounter();
			}
			SaveData.SpawnCount++;
			prefab.GetComponent<Obj>().buttonReference = gameObject;
			return Instantiate(prefab, pos, Random.rotation).transform;
		}
			GameController.Instance.audioManager.PlaySound("error");
			GetComponent<Animator>().SetTrigger("Disabled");
			//animate counter
			//play roblox death sound
		
		return null;
	}

	public void DespawnObject(GameObject obj) {
		Destroy(obj); //continue move away from Spawn item and related stuff
		SaveData.DespawnCount++;
		if (itemLimit) {
			itemCount++;
			UpdateCounter();
		}
	}

	public GameObject InstantiateObject() {
		prefab.GetComponent<Obj>().buttonReference = gameObject;
		return Instantiate(prefab);
	}

	private void UpdateCounter() {
		counterText.text = itemCount.ToString();
	}

}
