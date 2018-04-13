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
		if (GameController.Instance.canMove) {
			float fwd = Input.GetAxis("Vertical") * 2.0f;
			float horiz = Input.GetAxis("Horizontal") * 2.0f;
			Debug.DrawRay(transform.position, transform.forward, Color.red);
			Debug.DrawRay(parentCamera.position, parentCamera.forward, Color.red);
			rb.AddTorque(2f * parentCamera.right * fwd);
			rb.AddTorque(2f * parentCamera.forward * -horiz);
			if (transform.position.y < -2) {
				GameController.Instance.audioManager.PlaySound("error");
				transform.position = new Vector3(0,1,0);

			}
		}
	}

	void OnCollisionEnter(Collision otherCol) {
		Collider other = otherCol.collider;
		if (other.gameObject.tag == "Item") {
			Transform item = GetItemRootParent(other.transform);
			//Get collision point distance from item origin
			//move along the contact vector 
			float distFromOrigin = Vector3.Distance(otherCol.contacts[0].point, item.position);
			Debug.Log(distFromOrigin);
			if (item.GetComponent<Rigidbody>().mass <= weightMax && distFromOrigin < 1.5f) {
				float dist = Vector3.Distance(transform.position, item.position);
					weightMax += 0.4f * (item.GetComponent<Rigidbody>().mass);
					distMax += 0.4f * (item.GetComponent<Rigidbody>().mass);
					//transform.localScale += ()
					transform.localScale = transform.localScale + (Vector3.one * (distFromOrigin*0.1f)/transform.localScale.x );
					Destroy(item.GetComponent<Rigidbody>());
					foreach (Collider col in item.GetComponentsInChildren<Collider>()) {
						col.enabled = false;
					}
					item.transform.position = Vector3.Lerp(transform.position, otherCol.contacts[0].point, 0.8f);
					item.SetParent(transform);
					//TODO: if center distance is too far from acceptable, offset to be closer
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
