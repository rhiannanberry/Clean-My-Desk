using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour {
    public int index;
    public Material UIMat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -3) {
            GameController.Instance.itemCounts[index]++;
            Destroy(gameObject);
        }
	}
}
