using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {

    public int selected = -1;
    public List<Interactable> interactables = new List<Interactable>();
    public int[] itemCounts;
	// Use this for initialization
	void Start () {
        itemCounts = new int[interactables.Count];
	}
	
	// Update is called once per frame
	void Update () {
		
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
