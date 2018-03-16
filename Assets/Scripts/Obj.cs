using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class Obj: MonoBehaviour {
	public GameObject buttonReference;
	public bool util = false;

	// Update is called once per frame

	void Update () {
		if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().name != "TimeMode"){
			if (!util && transform.position.y < -40) {
				buttonReference.GetComponent<ObjectButton>().DespawnObject(gameObject);
			}
			if (util && transform.localPosition.x <= -10) {
				Destroy(gameObject);
			}
		}
	}
}
