using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class Obj: MonoBehaviour {
	public GameObject buttonReference;

	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().buildIndex != 0){
			if (buttonReference != null && transform.position.y < -3) {
				buttonReference.GetComponent<ObjectButton>().DespawnObject(gameObject);
			}
			if (buttonReference == null && transform.localPosition.x <= -10) {
				Destroy(gameObject);
			}
		}
	}
}
