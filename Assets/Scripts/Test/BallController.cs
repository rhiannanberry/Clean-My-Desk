using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
	public Transform parentCamera;
	private Rigidbody rb;

	private float weightMax = 0.1f;
	private float distMax = 0.7f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();		
	}
	
	// Update is called once per frame
	void Update () {
		float fwd = Input.GetAxis("Vertical") * 2.0f;
		float horiz = Input.GetAxis("Horizontal") * 2.0f;
		Debug.DrawRay(transform.position, transform.forward, Color.red);
		Debug.DrawRay(parentCamera.position, parentCamera.forward, Color.red);
		rb.AddTorque(2f * parentCamera.right * fwd);
		rb.AddTorque(2f * parentCamera.forward * -horiz);

	}

	void OnCollisionEnter(Collision otherCol) {
		Collider other = otherCol.collider;
		if (other.gameObject.tag == "Item") {
			Transform item = GetItemRootParent(other.transform);
			if (item.GetComponent<Rigidbody>().mass <= weightMax) {
				if (Vector3.Distance(transform.position, item.position) < distMax) { //grow this number as it gets bigger
					weightMax += 0.4f * (item.GetComponent<Rigidbody>().mass);
					distMax += 0.4f * (item.GetComponent<Rigidbody>().mass);
					transform.localScale = transform.localScale + (Vector3.one * 0.01f );
					Debug.Log(weightMax);
					Destroy(item.GetComponent<Rigidbody>());
					item.transform.position = Vector3.Lerp(transform.position, item.transform.position, 0.8f);
					item.SetParent(transform);
					//TODO: if center distance is too far from acceptable, offset to be closer
				}
			}
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
