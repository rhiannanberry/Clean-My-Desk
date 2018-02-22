using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class Item : MonoBehaviour {
    public int index;
    public Material UIMat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().buildIndex != 1){
			if (transform.position.y < -3) {
		
				GameController.Instance.itemCounts[index]++;
				Destroy(gameObject);
			}
		}
	}
}
