using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorControl : MonoBehaviour {
    public List<Transform> objs;
    private Transform selected = null;
    //y pos is locked
    float startZ = 0;
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
        
       
        //get x where z is 0
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction*20);
        float dist = 0f;
        float spawnY = 0f;
        if (hPlane.Raycast(ray, out dist)) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                spawnY = ray.GetPoint(dist).y;
                transform.position = new Vector3(ray.GetPoint(dist).x, yPos, zPos);
                //transform.position = ray.GetPoint(dist);
                if (Input.GetMouseButton(0) && selected != null) {
                    //TODO: tie selected count to how many you can spawn
                    Instantiate(selected, new Vector3(transform.position.x, spawnY, zPos), Random.rotation);
                }
            }

        }
        
	}

    public void SetSelected(Transform sel) {
        selected = sel;
    }
}
