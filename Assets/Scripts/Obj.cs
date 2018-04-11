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
            //Uh. I hope this doesn't introduce too much overhead.
            //As a backup plan I could only track the last item the mouse interacted with? But that would miss out on all the lovely glitchy collisions
            int velocityMagnitude = (int) (Vector3.Magnitude(GetComponent<Rigidbody>().velocity));
            if (velocityMagnitude >= SaveData.MaxVelocity)
                SaveData.MaxVelocity = velocityMagnitude;
		}
	}
}
