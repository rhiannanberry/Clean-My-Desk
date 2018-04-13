using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menuing : MonoBehaviour {
    // Use this for initialization
    public bool isOpen = false;
    public bool isHidden = false;
	void Start () {
        if (isOpen) {
            
        GetComponent<Animator>().SetBool("isOpen", isOpen);
        }
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
        if (gameObject.name == "ObjMenScrollView") {
            GameController.Instance.itemMenuOpen = isOpen;
            if (!isOpen && GameController.Instance.phantomItem != null) {
                Destroy(GameController.Instance.phantomItem);
            }
        }
    }

    public void ToggleHide() {
        isHidden = !isHidden;
        if (isHidden) {
            GetComponent<Animator>().SetTrigger("isHidden");
        }
        //GetComponent<Animator>().SetBool("isHidden", isHidden);
    }
}
