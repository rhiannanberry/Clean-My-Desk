using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public bool startMenu = true;
	private Canvas monitor1 = null;
	private Canvas monitor2 = null;
	// Use this for initialization
	void Start () {
		if (startMenu) {
			startMenuSetUp();
		}	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GotoSceneIndex(int i) {
		SceneManager.LoadScene(i);
	}

	public void OpenOptions() {
		if (startMenu) {
			monitor1.enabled = false;
			monitor2.enabled = true;
		}
	}

	public void ExitOptions() {
		if (startMenu) {
			monitor1.enabled = true;
			monitor2.enabled = false;
		}
	}

	//Save and cancel options shouldn't need specific code for start vs pause
	public void CancelOptions() {

	}

	public void SaveOptions() {

	}

	public void Exit() {
		if (startMenu) {
			//should add exit animation probably
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}
	}

	private void startMenuSetUp() {
		monitor1 = GameObject.Find("Monitor1/Canvas").GetComponent<Canvas>();
		monitor2 = GameObject.Find("Monitor2/Canvas").GetComponent<Canvas>();
		monitor2.enabled = false;
	}
}
