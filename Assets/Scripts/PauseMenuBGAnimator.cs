using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuBGAnimator : MonoBehaviour {

	public List<GameObject> items;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range(1,30) == 10) {
			Debug.Log("spawned");
			GameObject newItem = Instantiate(items[Random.Range(0,items.Count)]);
			newItem.transform.SetParent(transform);
			newItem.transform.localPosition = new Vector3(10, Random.Range(-2f,1.5f), Random.Range(3.0f, 6.0f));
			newItem.GetComponent<Rigidbody>().useGravity = false;
			newItem.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-4.0f, -1.0f), 0, 0);
			newItem.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
			newItem.GetComponent<Rigidbody>().detectCollisions = false;
		}	
	}
}
