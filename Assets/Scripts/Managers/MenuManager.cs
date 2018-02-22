using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public bool startMenu = true;
	private Canvas monitor1 = null;
	private Canvas monitor2 = null;
	private GameObject pauseMenu = null;
	private GameObject optionsMenu = null;
	public bool paused = false;
	// Use this for initialization
	void Awake () {
		if (startMenu) {
			StartMenuSetUp();
		} else {
			PauseMenuSetUp();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GotoSceneIndex(int i) {
		SceneManager.LoadScene(i);
	}

	public void OpenPauseMenu() {
		if (!startMenu) {
			ExitOptions(); //same desired effect
			paused = true;
		}
	}

	public void OpenOptions() {
		if (startMenu) {
			monitor1.enabled = false;
			monitor2.enabled = true;
		} else {
			optionsMenu.SetActive(true);
			pauseMenu.SetActive(false);
		}
	}

	public void ExitOptions() {
		if (startMenu) {
			monitor1.enabled = true;
			monitor2.enabled = false;
		} else {
			optionsMenu.SetActive(false);
			pauseMenu.SetActive(true);
		}
	}

	//Save and cancel options shouldn't need specific code for start vs pause
	public void CancelOptions() {

	}

	public void SaveOptions() {

	}

	public void Continue() {
		if (!startMenu) {
			//Close both menus
			pauseMenu.SetActive(false);
			optionsMenu.SetActive(false);
			paused = false;
		}
	}

	public void Exit() {
		if (startMenu) {
			//should add exit animation probably
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		} else {
			//TODO: Confirmation sub menu
			paused = false;
			GotoSceneIndex(1);
		}
	}

	private void StartMenuSetUp() {
		monitor1 = GameObject.Find("Monitor1/Canvas").GetComponent<Canvas>();
		monitor2 = GameObject.Find("Monitor2/Canvas").GetComponent<Canvas>();
		monitor2.enabled = false;
	}

	private void PauseMenuSetUp() {
		gameObject.SetActive(true);
		foreach (Transform child in gameObject.GetComponentInChildren<Transform>()) {
			child.gameObject.SetActive(true);
		}
		pauseMenu = GameObject.Find("PrimaryMenu");
		optionsMenu = GameObject.Find("OptionsMenu");
		pauseMenu.SetActive(false);
		optionsMenu.SetActive(false);
	}
}
