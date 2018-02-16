using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIMouse : MonoBehaviour {
	public float mouseSpeed = 20.0f;

	private Transform holding = null;
	private RectTransform screenPos;
	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		screenPos = transform.GetComponent<RectTransform>();
		screenPos.anchoredPosition = new Vector2(Screen.width/2, (Screen.height/2));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		Cursor.lockState = CursorLockMode.Locked;
		Vector3 deltaMovement = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * mouseSpeed;
		screenPos.anchoredPosition3D += deltaMovement;
		screenPos.anchoredPosition = Input.mousePosition;
		screenPos.anchoredPosition = new Vector2(Mathf.Clamp(screenPos.anchoredPosition.x, 0, Screen.width), Mathf.Clamp(screenPos.anchoredPosition.y, 0, Screen.height)); 
		Selecting();
		Holding();
	}

	void Selecting() {
		if (holding == null) {

			Ray ray = Camera.main.ScreenPointToRay(screenPos.anchoredPosition3D);
			Debug.DrawRay(ray.origin, ray.direction*50);
			RaycastHit hit;
			if (GameController.Instance.selected != -1 && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
				Debug.Log("Dab");
				holding = GameController.Instance.SpawnSelected(ray.GetPoint(2));
				Color clr = transform.GetComponent<Image>().color;
				clr.a = 0;
				transform.GetComponent<Image>().color = clr;
				holding.GetComponent<Rigidbody>().useGravity = false;
				holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
			} else if (Physics.Raycast(ray, out hit)) {
				if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
					
					holding = hit.transform;
					Color clr = transform.GetComponent<Image>().color;
					clr.a = 0;
					transform.GetComponent<Image>().color = clr;
					holding.GetComponent<Rigidbody>().useGravity = false;
					holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
				}
			}
		}
	}

	void Holding() {
		if (holding != null && Input.GetMouseButton(0)) {
			//z stays the same here
			float zScrollDelta = 10 * Input.GetAxis("Mouse ScrollWheel");
			if (zScrollDelta != 0) {
				holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
			} else {
				holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
			}
			holding.transform.position += new Vector3(0, 0, zScrollDelta);
			float zDiff = Mathf.Abs(Camera.main.transform.position.z - holding.transform.position.z);
			Vector3 uiMouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.anchoredPosition.x, screenPos.anchoredPosition.y, zDiff) );
			Debug.Log(uiMouseWorld);
			holding.GetComponent<Rigidbody>().MovePosition(new Vector3(uiMouseWorld.x, uiMouseWorld.y, holding.transform.position.z)); 
 			holding.GetComponent<Rigidbody>().freezeRotation = true;
    		holding.Rotate(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * -100, Space.World);
    		holding.Rotate(Vector3.right * Input.GetAxis("Vertical") * Time.deltaTime * -100, Space.World);

		} else if (holding != null && Input.GetMouseButtonUp(0)) {
			Color clr = transform.GetComponent<Image>().color;
			clr.a = 1;
			transform.GetComponent<Image>().color = clr;
			holding.GetComponent<Rigidbody>().useGravity = true;
			holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			holding = null;				
		}
	}

	public Transform FindParentWithTag(Transform child, string tag) {
		while(child.parent != null) {
			if (child.parent.tag == tag ) {
				return child.parent;
			}
			child = child.parent.transform;
		}
		return null;
	}

	//two types of object selecting: 
}
