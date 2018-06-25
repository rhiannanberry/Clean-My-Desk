using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedDeath : MonoBehaviour {

	private bool move = false;
	public float time = 5f;
	public float speed = 40f;
	
	// Update is called once per frame
	void Update () {
		if (move && transform.position.y < 2000) {
			transform.Translate(Vector3.up * Time.deltaTime * speed);
		}
	}

	public void Close() {
		move = true;
	}
}
