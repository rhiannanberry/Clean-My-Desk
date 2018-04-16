using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public bool startMenu = true;
	public bool subMenu = false;
	private Canvas monitor1 = null;
	private Canvas monitor2 = null;
	private GameObject pauseMenu = null;

	private Image pauseImage = null;
	private GameObject optionsMenu = null;
	public bool paused = false;

	public bool animDisable = false;
	public bool animDisabled = false;

	private bool unpausing = false;
	private bool pausing = false;

	private bool exitingScene = false;
	// Use this for initialization
	void Awake () {
		FadeInScene();
		if (startMenu) {
			StartMenuSetUp();
		} else if (!subMenu) {
			PauseMenuSetUp();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (exitingScene) {
			Debug.Log("Alpha: " + GameObject.Find("Canvas/Black").GetComponent<Image>().color.a.ToString());
		}
		if (!startMenu) {
			if (pausing) {
				Debug.Log(pauseImage.canvasRenderer.GetAlpha());
				if (pauseImage.canvasRenderer.GetAlpha() >= 1.0f) {
					pausing = false;
				}
			}
		} else if (animDisable && !animDisabled) {
			animDisabled = true;
			GetComponent<Animator>().enabled = false;
		}
	}
	public void GotoSceneIndex(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}

	public void GotoSceneIndex(int buildNum) {
		SceneManager.LoadScene(buildNum);
	}

	public void RestartCurrentScene() {
		InvokeScene(SceneManager.GetActiveScene().buildIndex);
	}

	private IEnumerator FadeOutScene(int i) {
		if (SceneManager.GetActiveScene().name == "StartMenu") {
			yield return new WaitForSeconds(1f);
		}
		GameObject.Find("MenusCanvas/Black").GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
		GameObject.Find("MenusCanvas/Black").GetComponent<Image>().color = Color.black;
		GameObject.Find("MenusCanvas/Black").GetComponent<Image>().CrossFadeAlpha(1.0f, 0.5f, false);
		yield return new WaitForSeconds(.5f);
		SceneManager.LoadScene(i);
	}

	private void FadeInScene() {
		GameObject.Find("MenusCanvas/Black").GetComponent<Image>().canvasRenderer.SetAlpha(1.0f);
		GameObject.Find("MenusCanvas/Black").GetComponent<Image>().color = Color.black;
		GameObject.Find("MenusCanvas/Black").GetComponent<Image>().CrossFadeAlpha(0.0f, 0.5f, false);
	
	}

	public void InvokeScene(int i) {
		StartCoroutine(FadeOutScene(i));
	}

	public void OpenPauseMenu() {
		if (!startMenu) {
			optionsMenu.SetActive(false);
			pauseMenu.SetActive(true);

			//pauseImage.canvasRenderer.SetAlpha(0.0f);
			//pauseImage.CrossFadeAlpha(1.0f, 5f, false);

			//pausing = true;
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
			GotoSceneIndex("StartMenu");
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
		pauseImage = pauseMenu.GetComponent<Image>();
		optionsMenu = GameObject.Find("OptionsMenu");
		pauseMenu.SetActive(false);
		optionsMenu.SetActive(false);
	}

}
