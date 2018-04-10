using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorControl : MonoBehaviour {
    public List<Transform> objs;
    [SerializeField]
    private Transform selected = null;
    [SerializeField]
    private Transform holding = null;
    //y pos is locked
    float startZ = 0;
    bool zMove = false;
    float yPos = 0;
    float zPos = 0;
    Plane hPlane;
	// Use this for initialization
	void Start () {
        yPos = transform.position.y;
        zPos = startZ;
        hPlane = new Plane(new Vector3(-10, 10, 0), new Vector3(10, 10, 0), new Vector3(-10, -10, 0));
	}
	
	// Update is called once per frame
	void Update () {
       // bool menuIsOpen = GameObject.Find("ObjMenScrollView").GetComponent<Menuing>().isOpen;
       // //get x where z is 0
       // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       // Debug.DrawRay(ray.origin, ray.direction*20);
       // float dist = 0f;
       // float spawnY = 0f;
       // if (hPlane.Raycast(ray, out dist)) {
       //     transform.position = new Vector3(ray.GetPoint(dist).x, ray.GetPoint(dist).y, zPos);
       //     if (menuIsOpen && holding == null) {
       //         if (!EventSystem.current.IsPointerOverGameObject()) {
       //             Vector3 spawnPos = new Vector3(transform.position.x, ray.GetPoint(dist).y, zPos);

       //             if (Input.GetMouseButtonDown(0)) {
       //                 holding = GameController.Instance.SpawnSelected(spawnPos);
       //             }
       //         }
       //     } else if (!menuIsOpen && holding == null) {
       //         if (Input.GetMouseButtonDown(0)) {
       //             RaycastHit hit;
       //             if (Physics.Raycast(ray, out hit, 0)) {
       //                 Debug.Log(hit.collider.gameObject.name);
       //                 holding = (hit.collider.GetComponentInParent<Item>()).transform;
       //                 if (holding != null) {
       //                     Debug.Log(holding.name);

       //                     zPos = holding.position.z;
       //                     hPlane = new Plane(new Vector3(-10, 10, zPos), new Vector3(10, 10, zPos), new Vector3(-10, -10, zPos));
       //                 }
       //             }
       //         }
       //     }
       // }

       // //change to be based on mouse delta
       // //need to hide 
       // //allow plane shift without holding 
       // if (holding != null) {
       //     if (Input.GetKey(KeyCode.LeftShift)) {
       //         zMove = !zMove;
       //         if (!zMove) {
       //             hPlane = new Plane(new Vector3(-10, 10, zPos), new Vector3(10, 10, zPos), new Vector3(-10, -10, zPos));
       //         }
       //     }

       //     if (Input.GetMouseButton(0)) {
       //         holding.gameObject.GetComponent<Rigidbody>().useGravity = false;
       //         if (zMove) {
       //             RaycastHit hit;
       //             if (Physics.Raycast(ray, out hit, 8)) { //Exclude table items
       //                 zPos = hit.point.z;
       //                 Debug.Log(hit.transform.name);
       //                 transform.position = new Vector3(hit.point.x, transform.position.y, zPos);

       //             }
       //         } else {
       //             if (hPlane.Raycast(ray, out dist)) {
       //                 transform.position = new Vector3(ray.GetPoint(dist).x, ray.GetPoint(dist).y, zPos);
       //             }
       //         }
       //         holding.position = transform.position;

       //     }
       //     if (Input.GetMouseButtonUp(0)) {
       //         holding.gameObject.GetComponent<Rigidbody>().useGravity = true;
       //         holding.GetComponent<Rigidbody>().freezeRotation = false;
       //         
       //         holding = null;
       //     }
       //     
       //     holding.GetComponent<Rigidbody>().freezeRotation = true;
       //     holding.Rotate(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * -100, Space.World);
       //     holding.Rotate(Vector3.right * Input.GetAxis("Vertical") * Time.deltaTime * -100, Space.World);
       // }
    }

    public void SetSelected(Transform sel) {
        selected = sel;
    }
}


//