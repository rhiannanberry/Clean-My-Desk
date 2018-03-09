using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menuing : MonoBehaviour {
    // Use this for initialization
    public bool isOpen = false;
    public bool isHidden = false;
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

    public void ToggleHide() {
        isHidden = !isHidden;
        if (isHidden) {
            GetComponent<Animator>().SetTrigger("isHidden");
        }
        //GetComponent<Animator>().SetBool("isHidden", isHidden);
    }
}
