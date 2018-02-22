using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {

    public int selected = -1;
    public List<Interactable> interactables = new List<Interactable>();
    public int[] itemCounts;
    private MenuManager pauseMenu = null;

	// Use this for initialization
	void Start () {
        itemCounts = new int[interactables.Count];
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<MenuManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!pauseMenu.paused) {
                pauseMenu.OpenPauseMenu();
            } else if (pauseMenu.paused) {
                pauseMenu.Continue();
            }
        }
	}

    public Transform SpawnSelected(Vector3 spawnPosition) {
        if (selected != -1 && interactables != null ) {//&& itemCounts[selected] > 0) {
            //itemCounts[selected]--;
            Item i = Instantiate(interactables[selected].item, spawnPosition, Random.rotation);
            i.index = selected;
            return i.transform;
        }
        return null;
    }

    private void CheckMenu() {

    } 

    [System.Serializable]
    public struct Interactable {
        public Item item;
        public int count;

        public Interactable(Item ii, int ccount) {
            item = ii;
            count = ccount;
        }
    }
}
