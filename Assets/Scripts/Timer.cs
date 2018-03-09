using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	public float time = 30;
	private Text text;
	// Use this for initialization
	void Start () {
		text = transform.GetComponent<Text>();
		text.text = time.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if (time < 0 ) {
			time = 0;
		}
		if (time >= 10) {
			text.text = (time.ToString()).Substring(0, 4);
		} else {
			text.text = ' ' + (time.ToString()).Substring(0, 3);
		}
	}
}
