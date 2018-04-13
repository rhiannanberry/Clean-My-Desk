using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform toFollow;
	public float speed = 5f;
	private Vector3 offset, ogOffset;
	private float prevScale = 1f;
	// Use this for initialization
	void Start () {
		offset = transform.position - toFollow.position;
		ogOffset = offset;
		
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 position = transform.position;
		if (toFollow.localScale.x > prevScale) {
			float scale = toFollow.localScale.x;
			offset += ogOffset * (scale - prevScale);
			prevScale = scale;
		}
		transform.position = new Vector3(toFollow.position.x + offset.x, transform.position.y, toFollow.position.z + offset.z);
	}
}
