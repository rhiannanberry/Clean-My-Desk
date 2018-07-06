using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour {
	public float time = 30;
	public GameObject endMenu, pauseMenu;
	public GameObject itemsContainer;
	private TextMeshProUGUI text;
	private bool continueCount = true;
	private bool start = false;
	// Use this for initialization
	void Start () {
		endMenu.transform.Find("MainModeUI").gameObject.SetActive(false);
		endMenu.transform.localScale = new Vector3(0.0f,1,1);
		text = transform.GetComponent<TextMeshProUGUI>();
		SetTime();
		StartCoroutine(InitCountdown());
	}

	//software design is my passion
	IEnumerator InitCountdown() {
		GameController.Instance.canMove = false;
		text.text = "3";
		yield return new WaitUntil(IsNotPaused);
		yield return new WaitForSeconds(1);
		yield return new WaitUntil(IsNotPaused);
		text.text = "2";
		yield return new WaitUntil(IsNotPaused);
		yield return new WaitForSeconds(1);
		yield return new WaitUntil(IsNotPaused);
		text.text = "1";
		yield return new WaitUntil(IsNotPaused);
		yield return new WaitForSeconds(1);
		yield return new WaitUntil(IsNotPaused);
		text.text = "GO!";
		GetComponent<Animator>().SetBool("Start", true);
		GameController.Instance.canMove = true;
		yield return new WaitUntil(IsNotPaused);
		yield return new WaitForSeconds(0.5f);
		yield return new WaitUntil(IsNotPaused);
		text.text = time.ToString();
		start = true;
	}

	private bool IsNotPaused() {
		return !GameController.Instance.paused;
	}	
	// Update is called once per frame
	void Update() {
		
		if (continueCount && start) {
			if (Input.GetKeyDown(KeyCode.R)) {
				endMenu.GetComponent<MenuManager>().InvokeScene(3);
			}
			if (itemsContainer.transform.childCount == 0) {
				continueCount = false;
				endMenu.transform.Find("TimeModeUI/Fail").gameObject.SetActive(false);
				endMenu.transform.Find("TimeModeUI/Success").gameObject.SetActive(true);
				SaveData.sd.timeModeLevel++;
				//trigger menu
				endMenu.GetComponent<Menuing>().ToggleMenu();
				GameObject.Find("Mouse").GetComponent<Image>().enabled = true;
			} else if (time < 0) {
				time = 0;
				continueCount = false;
				endMenu.transform.Find("TimeModeUI/Fail").gameObject.SetActive(true);
				endMenu.transform.Find("TimeModeUI/Success").gameObject.SetActive(false);
				GameObject.Find("Mouse").GetComponent<Image>().enabled = true;
				//trigger menu
				endMenu.GetComponent<Menuing>().ToggleMenu();
			}
			if (time >= 10) {
				text.text = (time.ToString()).Substring(0, 2);
			} else {
				text.text = time==0 ? "RIP" : (time.ToString()).Substring(0, 1);
			}
			if (IsNotPaused()) {
				time -= Time.deltaTime;
				GameController.Instance.timer = time;
			}
		}
	}

	private void SetTime() {
		time -= (SaveData.sd.timeModeLevel * 10f);
		GameController.Instance.timer = time;
	}
}
