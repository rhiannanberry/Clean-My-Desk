using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
	public Transform parentCamera;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();		
	}
	
	// Update is called once per frame
	void Update () {
		float fwd = Input.GetAxis("Vertical") * 1.0f;
		float horiz = Input.GetAxis("Horizontal") * 1.0f;
		Debug.DrawRay(transform.position, transform.forward, Color.red);
		Debug.DrawRay(parentCamera.position, parentCamera.forward, Color.red);
		rb.AddTorque(parentCamera.right * fwd);
		rb.AddTorque(parentCamera.forward * -horiz);

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Item") {
			Debug.Log("HEUGHGHH");
			Transform item = GetItemRootParent(other.transform);
			Destroy(item.GetComponent<Rigidbody>());
			item.SetParent(transform);
		}
	}

	Transform GetItemRootParent(Transform item) {
		if (item.parent != null && item.parent.tag == "Item") {
			return GetItemRootParent(item.parent);
		} else {
			return item;
		}
		
	}
}
