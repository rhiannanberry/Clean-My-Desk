using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuBGAnimator : MonoBehaviour {

	public List<GameObject> items;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range(1,50) == 10) {
			Debug.Log("spawned");
			GameObject newItem = Instantiate(items[Random.Range(0,items.Count)]);
			newItem.transform.SetParent(transform);
			newItem.transform.localPosition = new Vector3(70, Random.Range(-20.0f,20.0f), Random.Range(5.0f, 8.0f));
			newItem.GetComponent<Rigidbody>().useGravity = false;
			newItem.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-10.0f, -3.0f), 0, 0);
		}	
	}
}
