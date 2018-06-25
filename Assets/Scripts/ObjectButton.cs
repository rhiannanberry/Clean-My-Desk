using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class ObjectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	private GameObject prefab, phantomPrefab;
	private TextMeshProUGUI counterText, itemName, descriptionScroller;

	[HideInInspector]
	public int itemCount;
	private bool itemLimit = true;
	private string description;

	void Start() {
		descriptionScroller = GameObject.Find("ItemDescription").GetComponent<TextMeshProUGUI>();
		if (SceneManager.GetActiveScene().name == "GodMode") {
			itemLimit = false;
			transform.Find("Counter/CountText").gameObject.SetActive(false);
			transform.Find("Counter/NoCountText").gameObject.SetActive(true);
		}
	}

	void Update() {
		if (GameController.Instance.itemMenuOpen) {
			if (itemName.isActiveAndEnabled) {
				itemName.transform.position = GameController.Instance.screenPos.position;
			}
			if (GameController.Instance.selected != gameObject) {
				GetComponent<Animator>().SetBool("Selected", false);
			}
		}
	}

	public void OnPointerEnter(PointerEventData e) {
		itemName.enabled = true;
		itemName.transform.position = GameController.Instance.screenPos.position;
	}

	public void OnPointerExit(PointerEventData e) {
		itemName.enabled = false;		
	}
	
	public void SetSelected() {
		GameController.Instance.selected = gameObject;
		GetComponent<Animator>().SetBool("Selected", true);
		GetComponent<Animator>().SetTrigger("Pressed");
		descriptionScroller.text = description;
	}

	public void SetValues(GameObject prefab) {
		this.prefab = prefab;
		this.prefab.GetComponent<Obj>().buttonReference = gameObject;
		this.itemCount = prefab.GetComponent<Obj>().itemCount;
		description = prefab.GetComponent<Obj>().description;
		counterText = transform.Find("Counter/CountText").GetComponent<TextMeshProUGUI>();
		itemName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
		itemName.text = prefab.GetComponent<Obj>().itemName;
		itemName.enabled = false;
		UpdateCounter();
	}


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
		transform.GetComponent<Animator>().SetTrigger("Error");
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
