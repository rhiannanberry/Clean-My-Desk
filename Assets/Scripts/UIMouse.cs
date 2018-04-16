﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class UIMouse : MonoBehaviour {
	public bool inMenu = true;
	public float mouseSpeed = 20.0f;

    public float previousDepth = -2.0f;
	public Transform worldSpaceCursor;
	public Transform holding = null;
	private GameObject holder, holdingLocalPositionProxy;
	private bool timeMode = false;
    private Vector3 lastCursorPosition;
    private Vector3 cursorMovement;
    private const float RELEASE_FORCE = 0.2f;
	private Transform prevHitItem;

	private GameController gC;
	public Transform ZPosCross, XPosCross;

    // Use this for initialization
    void Start () {
        gC = GameController.Instance;

        holder = new GameObject();
		holdingLocalPositionProxy = new GameObject();
		holdingLocalPositionProxy.transform.SetParent(holder.transform);
		Cursor.visible = false;
		gC.screenPos = transform.GetComponent<RectTransform>();
		gC.screenPos.anchoredPosition = new Vector2(Screen.width/2, (Screen.height/2));
		if (SceneManager.GetActiveScene().name == "TimeMode") {
			timeMode = true;
			GetComponent<Image>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		Cursor.lockState = CursorLockMode.Locked;
		Vector3 deltaMovement = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * mouseSpeed;
		gC.screenPos.anchoredPosition3D += deltaMovement;
		gC.screenPos.anchoredPosition = Input.mousePosition;
		gC.screenPos.anchoredPosition = new Vector2(Mathf.Clamp(gC.screenPos.anchoredPosition.x, 0, Screen.width), Mathf.Clamp(gC.screenPos.anchoredPosition.y, 0, Screen.height)); 
		IsInMenu();
		if (timeMode) {
			if (gC.paused) {
				GetComponent<Image>().enabled = true;
			} else {

				GetComponent<Image>().enabled = false;
			}
		} else if (!inMenu) {
			NotHoldingUpdate(); // selecting on the actual desk, not the menu
			Holding();
			UpdateDeskCrosshair();
		}
	}

	void NotHoldingUpdate() {
		if (prevHitItem != null) {
			prevHitItem.GetComponent<Obj>().ObjectHover(false);
		}
		//REMINDER TO RHIANNAN OF THE FUTURE:
		//UPDATE UNITY TO C# V6 SO YOU CAN USE NULL CONDITIONAL OPERATORS

		//if you have the item menu open and you've selected something and you're not holding something
		if (gC.selected != null && holding == null) {

			Vector3 newPosition = GetMouseWorldPosition(getZDelta()); // based on prevZ and mouse center


			UpdatePhantomSelected();
			UpdatePhantomItem(newPosition);
			PlaceItem(newPosition);
		}
		HighlightHoverAndPickupItem();		
	}

	void Holding() {
		if (holding != null && Input.GetMouseButton(0)) {
			if (gC.phantomItem != null) {
				gC.phantomItem.transform.localScale = new Vector3(0,0,0);
			}
			float zDelta = getZDelta();

			Vector3 newPosition = Vector3.zero;

			//SETTING UP PLANE AT CORRECT/expected Z
			float itemExpectedZ = holder.transform.position.z + zDelta;
			itemExpectedZ = itemExpectedZ > 5f ? 5f : itemExpectedZ;
			Debug.Log("Expected Z: " + itemExpectedZ);
			Plane itemPosZPlane = new Plane(Vector3.back, new Vector3(0, 0, itemExpectedZ));
			
			//SETTING UP MOUSE RAY
			Ray mouseRay = Camera.main.ScreenPointToRay(gC.screenPos.anchoredPosition);
			float hitDistance = 0;
			if (itemPosZPlane.Raycast(mouseRay, out hitDistance)) {
				newPosition += mouseRay.GetPoint(hitDistance);
			}

			worldSpaceCursor.position = newPosition;	
			Rigidbody heldrb = holding.GetComponent<Rigidbody>();
            heldrb.useGravity = false;
			holder.transform.position = newPosition;
            heldrb.freezeRotation = true;
			//holding.localPosition = itemPositionOffset;
			heldrb.velocity = Vector3.zero;
			heldrb.MovePosition(holdingLocalPositionProxy.transform.position);
			UpdateRigidbodyRotation(holding.gameObject);

            if(lastCursorPosition != null)
            {
                cursorMovement = worldSpaceCursor.transform.position - lastCursorPosition;
                lastCursorPosition = worldSpaceCursor.transform.position;
            }
		} else if (!Input.GetMouseButton(0) || !MouseScreenCheck()) {//change to !getmouse(0) && holding != null

            Color clr = transform.GetComponent<Image>().color;
			clr.a = 1;
			transform.GetComponent<Image>().color = clr;
			if (holding != null) {
				holding.SetParent(GameObject.Find("DefaultSetup").transform);
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
		HoldObject(toHold, toHold.transform.position);
	}

	private void HoldObject(Transform toHold, Vector3 grabPosition) {
		holder.transform.position = grabPosition;
		toHold.SetParent(holder.transform);
		holdingLocalPositionProxy.transform.localPosition = toHold.localPosition;
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

	private float getZDelta() {
		float zScrollDelta = 6 * Input.GetAxis("Mouse ScrollWheel");
		if (zScrollDelta != 0 && holding != null) {
			holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		} else if (holding != null){
			holding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
		}
		return zScrollDelta;
	}

	private Vector3 GetMouseWorldPosition(float zVal) {
		//Setting spawn location
		Vector3 newPosition = Vector3.zero;
		if (gC.phantomItem != null) {
			previousDepth = zVal + gC.phantomItem.transform.position.z;
		}
		Mathf.Clamp(previousDepth, -4.5f, 5f);
		//SETTING UP PLANE AT CORRECT/expected Z
		Plane itemPosZPlane = new Plane(Vector3.back, new Vector3(0, 0, previousDepth));
		
		//SETTING UP MOUSE RAY
		Ray mouseRay = Camera.main.ScreenPointToRay(gC.screenPos.anchoredPosition);
		float hitDistance = 0;

		if (itemPosZPlane.Raycast(mouseRay, out hitDistance)) {
			newPosition += mouseRay.GetPoint(hitDistance);
		}

		return newPosition;
	}

	private void UpdatePhantomSelected() {
		if ((gC.selected != gC.phantomSelected) || gC.phantomItem == null) {
			//Update those values to match
			gC.phantomSelected = gC.selected;
			//If not null, destroy previous phantomItem
			if (gC.phantomItem != null) Destroy(gC.phantomItem);

			//If phantomSelected not null, instantiate new phantomItem
			if (gC.phantomSelected != null) {
				gC.setPhantom = true;
				gC.phantomItem = gC.phantomSelected.GetComponent<ObjectButton>().InstantiateObject();
				gC.phantomItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
			}
			//remove colliders and obj script
			//set position, but maybe do that outside of this if/at the end of this function
			//will also need to figure in rotation
		}
	}

	//Add rotation to this later
	private void UpdatePhantomItem(Vector3 newPosition) {
		if (gC.phantomItem != null) {
			UpdateRigidbodyRotation(gC.phantomItem);
			gC.phantomItem.transform.position = newPosition;
			if (holding == null) {
				gC.phantomItem.transform.localScale = Vector3.one;
			}
		}
	}

	private void PlaceItem(Vector3 newPosition) {
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
			//Destroy phantomItem and hold an object
			Transform newItem = gC.SpawnSelected(gC.phantomItem.transform.position);
			Debug.Log(newItem);
			if (newItem != null) {
				string sfx = (Random.value > 0.5) ? "gasp" : "scream" ;
				gC.audioManager.PlaySound(sfx);
				newItem.rotation = gC.phantomItem.transform.rotation;
				newItem.GetComponent<Obj>().ObjectHover(true);
				prevHitItem = newItem;
				HoldObject(newItem);
			}
		}
	}

	private void HighlightHoverAndPickupItem() {
		if ((gC.selected == null && !gC.itemMenuOpen) && holding == null) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(gC.screenPos.anchoredPosition3D);
			Debug.DrawRay(ray.origin, ray.direction*50);

			if (Physics.Raycast(ray, out hit)) {//hovering when not in item menu
				if (hit.transform.GetComponent<Obj>() != null) {
					hit.transform.GetComponent<Obj>().ObjectHover(true);
					prevHitItem = hit.transform;
				}
				if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
					HoldObject(hit.transform, hit.point);
				}
			}
		}
	}

	private void UpdateRigidbodyRotation(GameObject obj) {
		Rigidbody rb = obj.GetComponent<Rigidbody>();
		rb.freezeRotation = false;
		Quaternion deltaRotation = Quaternion.AngleAxis(Input.GetAxis("Horizontal")*5, Vector3.up) * Quaternion.AngleAxis(5*Input.GetAxis("Vertical"), Vector3.right);
		rb.MoveRotation(deltaRotation * rb.rotation);
	}

	private void UpdateDeskCrosshair() {

		if (holding != null || (gC.phantomItem != null && gC.itemMenuOpen)) {
			XPosCross.gameObject.SetActive(true);	
			ZPosCross.gameObject.SetActive(true);
			Vector3 pos = (holding != null) ? holding.position : gC.phantomItem.transform.position;
			XPosCross.position = new Vector3(Mathf.Clamp(pos.x, -7.75f, 7.75f),0,0);
			ZPosCross.position = new Vector3(0,0, Mathf.Clamp(pos.z, -3.5f, 3.5f));
		} else {
			ZPosCross.gameObject.SetActive(false);	
			XPosCross.gameObject.SetActive(false);	
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
		Ray ray = Camera.main.ScreenPointToRay(gC.screenPos.anchoredPosition3D);
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
