using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIMouse : MonoBehaviour {
	public bool inMenu = true;
	public float mouseSpeed = 20.0f;
    public float previousDepth = 0.0f;
	public Transform depthOrb;
	public Transform worldSpaceCursor;
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
		IsInMenu();

		if (!inMenu) {
			Selecting(); // selecting on the actual desk, not the menu
			Holding();
		}
	}

	void Selecting() {
		if (holding == null) {
			Ray ray = Camera.main.ScreenPointToRay(screenPos.anchoredPosition3D);
			Debug.DrawRay(ray.origin, ray.direction*50);
			RaycastHit hit;
			if (GameController.Instance.selected != null && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
				HoldObject(GameController.Instance.SpawnSelected(ray.GetPoint(2) + new Vector3(0, 0, previousDepth)));
			} else if (Physics.Raycast(ray, out hit)) {
				depthOrb.position = Vector3.Scale(hit.transform.position, new Vector3(1,0,1));
				if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
					HoldObject(hit.transform);
				}
			}
		}
	}

	void Holding() {
		if (holding != null && Input.GetMouseButton(0)) {
			//z stays the same here
			//should z be moved by world pos cursor (or at least movepost?)
			float zScrollDelta = 6 * Input.GetAxis("Mouse ScrollWheel");
			if (zScrollDelta != 0) {
				holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
			} else {
				holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
			}
            previousDepth += zScrollDelta;
			holding.transform.position += new Vector3(0, 0, zScrollDelta);
			float zDiff = Mathf.Abs(Camera.main.transform.position.z - holding.transform.position.z);
			Vector3 uiMouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.anchoredPosition.x, screenPos.anchoredPosition.y, zDiff) );
			Vector3 heldObjectPosition = new Vector3(uiMouseWorld.x,uiMouseWorld.y,holding.transform.position.z);
			worldSpaceCursor.position = heldObjectPosition;	
			depthOrb.position = Vector3.Scale(heldObjectPosition, new Vector3(1,0,1));
			Rigidbody heldrb = holding.GetComponent<Rigidbody>();
			//heldrb.MovePosition(heldObjectPosition); 
			worldSpaceCursor.GetComponent<SpringJoint>().connectedBody = heldrb;
 			heldrb.freezeRotation = true;
			Quaternion deltaRotation = Quaternion.AngleAxis(Input.GetAxis("Horizontal")*5, Vector3.up) * Quaternion.AngleAxis(5*Input.GetAxis("Vertical"), Vector3.right);
			heldrb.MoveRotation(deltaRotation * heldrb.rotation);
    		//holding.Rotate(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * -100, Space.World);
    		//holding.Rotate(Vector3.right * Input.GetAxis("Vertical") * Time.deltaTime * -100, Space.World);

		} else if (holding != null && Input.GetMouseButtonUp(0)) {
			Color clr = transform.GetComponent<Image>().color;
			clr.a = 1;
			worldSpaceCursor.GetComponent<SpringJoint>().connectedBody = null;
			transform.GetComponent<Image>().color = clr;
			holding.GetComponent<Rigidbody>().useGravity = true;
			holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			holding = null;				
		}
	}

	private void HoldObject(Transform toHold) {
		holding = toHold;
		if (holding != null) {
			Color clr = transform.GetComponent<Image>().color;
			clr.a = 0;
			transform.GetComponent<Image>().color = clr;
			holding.GetComponent<Rigidbody>().useGravity = false;
			holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
		}
	}

	public void IsInMenu() {
		//probably gonna have to change this later but idc	
		inMenu = (EventSystem.current.IsPointerOverGameObject() && holding == null) || (SceneManager.GetActiveScene().name.Contains("Menu")); 
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

	public void HitScreen() {
		Ray ray = Camera.main.ScreenPointToRay(screenPos.anchoredPosition3D);
		Debug.DrawRay(ray.origin, ray.direction*50);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			GameObject col = GameObject.Find("MenuHitCollider");
			col.transform.position = ray.GetPoint(3);
			col.transform.rotation = Quaternion.LookRotation(ray.direction);
			col.GetComponent<Rigidbody>().velocity = ray.direction * 15;
		}
	}

	//two types of object selecting: 
}
