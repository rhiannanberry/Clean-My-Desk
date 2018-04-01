using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform toFollow;
	public float speed = 5f;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = transform.position - toFollow.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 position = transform.position;
		transform.position = new Vector3(toFollow.position.x + offset.x, transform.position.y, toFollow.position.z + offset.z);
	}
}
