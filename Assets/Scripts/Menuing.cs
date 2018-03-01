using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menuing : MonoBehaviour {
    // Use this for initialization
    public bool isOpen = false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleMenu() {
        isOpen = !isOpen;
        if (!isOpen) {
            GameController.Instance.selected = null;
        }
        GetComponent<Animator>().SetBool("isOpen", isOpen);
    }
}
