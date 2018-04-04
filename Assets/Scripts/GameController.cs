using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {

    //reference to the object button
    public GameObject selected = null;
    private MenuManager pauseMenu = null;

    public bool paused;

	// Use this for initialization
	void Start () {
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<MenuManager>();
        paused = pauseMenu.paused;
	}
	
	// Update is called once per frame
	void Update () {
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

    public Transform SpawnSelected(Vector3 spawnPosition) {
        if (selected != null) {//&& itemCounts[selected] > 0) {
            return selected.GetComponent<ObjectButton>().SpawnObject(spawnPosition);
        }
        return null;
    }

    private void CheckMenu() {

    } 
}
