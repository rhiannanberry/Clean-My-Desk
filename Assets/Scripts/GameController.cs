using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController> {

    //reference to the object button
    public GameObject selected = null;
    public GameObject phantomSelected = null;
    public GameObject phantomItem = null;
    public bool setPhantom = false;
    public bool itemMenuOpen = false;
    private MenuManager pauseMenu = null;

    public AudioManager audioManager;

    [HideInInspector]
    public float masterVolume = 1;
    [HideInInspector]
    public float musicVolume = 1;
    [HideInInspector]
    public float sfxVolume = 1;

    public bool paused;

    [HideInInspector]
    public bool canMove = false;
    public bool musicPaused = false;
    [HideInInspector]
    public RectTransform screenPos;

	void Start () {
        audioManager = GetComponent<AudioManager>();
        audioManager.AudioSetup();
        audioManager.PlaySong();
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            pauseMenu = GameObject.Find("PauseMenu").GetComponent<MenuManager>();
            paused = pauseMenu.paused;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                pauseMenu = GameObject.Find("PauseMenu").GetComponent<MenuManager>();
                if (!pauseMenu.paused) {
                    pauseMenu.OpenPauseMenu();
                } else if (pauseMenu.paused) {
                    pauseMenu.Continue();
                }
            }
            paused = pauseMenu.paused;
        }
	}

    public Transform SpawnSelected(Vector3 spawnPosition) {
        if (selected != null) {//&& itemCounts[selected] > 0) {
            return selected.GetComponent<ObjectButton>().SpawnObject(spawnPosition);
        }
        return null;
    }

    void OnLevelWasLoaded() {
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            pauseMenu = GameObject.Find("PauseMenu").GetComponent<MenuManager>();
            paused = pauseMenu.paused;
        }
    }

    private void CheckMenu() {

    } 
}
