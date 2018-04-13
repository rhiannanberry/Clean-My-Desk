using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class Obj: MonoBehaviour {
	public GameObject buttonReference;
	public bool util = false;
	private Renderer[] childMats;
	private Color baseColor;

	// Update is called once per frame
	void Awake() {
		childMats = GetComponentsInChildren<Renderer>();
		baseColor = childMats[0].materials[0].GetColor("_OutlineColor");
	}

	void Start() {
		if (GameController.Instance.setPhantom && GameController.Instance.phantomSelected == buttonReference) {
			Debug.Log(name);
			GetComponent<Rigidbody>().useGravity = false;
			foreach( Collider childCol in GetComponentsInChildren<Collider>()) {
				childCol.enabled = false;
			}
			GetComponent<Obj>().enabled = false;
			GetComponent<Rigidbody>().freezeRotation = true;
			transform.rotation = Quaternion.identity;
			GameController.Instance.setPhantom = false;
		}
	}

	void Update () {
		if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().name != "TimeMode"){
			if (!util && transform.position.y < -40) {
				if (buttonReference != null) {
					buttonReference.GetComponent<ObjectButton>().DespawnObject(gameObject);
				} else {
					Destroy(gameObject);
				}
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

	public void ObjectHover(bool hovering) {
		Color col = hovering ? Color.yellow : baseColor;
		UpdateShader(col);
	}

	private void UpdateShader(Color col) {
		foreach(Renderer rend in childMats) {
			foreach(Material mat in rend.materials) {
				try {
					mat.SetColor("_OutlineColor", col);
				} catch (Exception e) {
					Debug.Log(transform.name + ": " + e.ToString());
				}
			}
		}
	}
}
