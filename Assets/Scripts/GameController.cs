using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController> {

    //reference to the object button
    [HideInInspector]
    public GameObject selected, phantomSelected, phantomItem = null;
    
    [HideInInspector]
    public bool paused, canMove, musicPaused, setPhantom, itemMenuOpen = false;
    
    [HideInInspector]
    public float masterVolume, musicVolume, sfxVolume = 1;
    

    [HideInInspector]
    public AudioManager audioManager;

    [HideInInspector]
    public RectTransform screenPos;


    private MenuManager pauseMenu = null;
	
    void Start () {
        audioManager = GetComponent<AudioManager>();
        audioManager.AudioSetup();
        audioManager.PlaySong();
        pauseMenu = (SceneManager.GetActiveScene().buildIndex != 0) ? GameObject.Find("PauseMenu").GetComponent<MenuManager>() : null;
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (paused) {
                    pauseMenu.Continue();
                } else {
                    pauseMenu.OpenPauseMenu();
                }
            }
        }
	}

    public Transform SpawnSelected(Vector3 spawnPosition) {
        return (selected != null) ? selected.GetComponent<ObjectButton>().SpawnObject(spawnPosition) : null;
    }

    void OnLevelWasLoaded() {
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            pauseMenu = GameObject.Find("PauseMenu").GetComponent<MenuManager>();
            paused = false;
        }
    }

    private void CheckMenu() {

    } 
}
