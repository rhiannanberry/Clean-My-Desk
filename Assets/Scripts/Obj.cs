using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class Obj: MonoBehaviour {
	public GameObject buttonReference;

	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().buildIndex != 1 && (buttonReference != null)){
			if (transform.position.y < -3) {
				buttonReference.GetComponent<ObjectButton>().DespawnObject(gameObject);
			}
		}
	}
}
