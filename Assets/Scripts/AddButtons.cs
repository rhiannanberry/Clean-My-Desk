using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButtons : MonoBehaviour {
    public GameObject buttonPrefab;

    // Use this for initialization
    void Start() {
        int index = 0;
        GameController.Instance.itemCounts = new int[GameController.Instance.interactables.Count];
        foreach (GameController.Interactable i in GameController.Instance.interactables) {
            GameObject b = Instantiate(buttonPrefab);
            b.transform.SetParent(transform, false);
            b.transform.GetComponent<SpawnItem>().SetDefaults(i.item, index);
            GameController.Instance.itemCounts[index] = i.count;
            index++;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
