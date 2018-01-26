using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour {
    private GameObject itemPrefab;
    public int count;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPrefab(GameObject prefab) {
        itemPrefab = prefab;
    }
    public void SetCount(int cnt) {
        count = cnt;
    }

    public void SetSelected() {
        GameObject.Find("Sphere").GetComponent<CursorControl>().SetSelected(itemPrefab.transform);
    }
}
