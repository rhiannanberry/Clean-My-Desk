using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIMouse : MonoBehaviour {
	public bool inMenu = true;
	public float mouseSpeed = 20.0f;
    public float previousDepth = -2.0f;
	public Transform depthOrb;
	public Transform worldSpaceCursor;
	public Transform holding = null;
	private RectTransform screenPos;

	private bool timeMode = false;
    private Vector3 lastCursorPosition;
    private Vector3 cursorMovement;
    private const float RELEASE_FORCE = 0.2f;

    // Use this for initialization
    void Start () {
		Cursor.visible = false;
		screenPos = transform.GetComponent<RectTransform>();
		screenPos.anchoredPosition = new Vector2(Screen.width/2, (Screen.height/2));
		if (SceneManager.GetActiveScene().name == "TimeMode") {
			timeMode = true;
			GetComponent<Image>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		Cursor.lockState = CursorLockMode.Locked;
		Vector3 deltaMovement = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * mouseSpeed;
		screenPos.anchoredPosition3D += deltaMovement;
		screenPos.anchoredPosition = Input.mousePosition;
		screenPos.anchoredPosition = new Vector2(Mathf.Clamp(screenPos.anchoredPosition.x, 0, Screen.width), Mathf.Clamp(screenPos.anchoredPosition.y, 0, Screen.height)); 
		IsInMenu();
		if (timeMode) {
			if (GameController.Instance.paused) {
				GetComponent<Image>().enabled = true;
			} else {

				GetComponent<Image>().enabled = false;
			}
		} else if (!inMenu) {
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
            heldrb.useGravity = false;
			heldrb.MovePosition(heldObjectPosition);
            //worldSpaceCursor.GetComponent<SpringJoint>().connectedBody = heldrb;
            heldrb.freezeRotation = true;

			Quaternion deltaRotation = Quaternion.AngleAxis(Input.GetAxis("Horizontal")*5, Vector3.up) * Quaternion.AngleAxis(5*Input.GetAxis("Vertical"), Vector3.right);
			heldrb.MoveRotation(deltaRotation * heldrb.rotation);

            if(lastCursorPosition != null)
            {
                cursorMovement = worldSpaceCursor.transform.position - lastCursorPosition;
                lastCursorPosition = worldSpaceCursor.transform.position;
            }
		} else if (Input.GetMouseButtonUp(0) || !MouseScreenCheck()) {

            Color clr = transform.GetComponent<Image>().color;
			clr.a = 1;
			transform.GetComponent<Image>().color = clr;
			if (holding != null) {
                Vector3 throwVector = (holding.position + cursorMovement) - holding.position;
                float speed = (throwVector.magnitude * RELEASE_FORCE) / Time.deltaTime;
                Vector3 throwVelocity = speed * throwVector.normalized;
                holding.GetComponent<Rigidbody>().velocity = throwVelocity;
  	        	holding.GetComponent<Rigidbody>().useGravity = true;
				holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;			
			}
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
            if (holding.gameObject.name.Equals("MedicineBottle"))
                SaveData.MedsTaken++;//This is super fragile I know, i'm just not sure what a better approach would be? check against its model?
		}
	}

	public void IsInMenu() {
		//probably gonna have to change this later but idc	
		inMenu = (EventSystem.current.IsPointerOverGameObject() && (holding == null || timeMode)) || (SceneManager.GetActiveScene().name.Contains("Menu")); 
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
			col.transform.position = ray.GetPoint(0);
			col.transform.rotation = Quaternion.LookRotation(ray.direction);
			col.GetComponent<Rigidbody>().velocity = ray.direction * 15;
		}
	}

	//two types of object selecting:

 public bool MouseScreenCheck(){
        #if UNITY_EDITOR
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Handles.GetMainGameViewSize().x - 1 || Input.mousePosition.y >= Handles.GetMainGameViewSize().y - 1){
        return false;
        }
        #else
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
        return false;
        }
        #endif
        else {
            return true;
        }
 }
}
