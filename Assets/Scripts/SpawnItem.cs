using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnItem : MonoBehaviour {
    private Item item;
    private int index;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //UI control for buttons somewhere in here
    public void SetSelected() {
        GameController.Instance.selected = index;
    }

    public void SetDefaults(Item i, int index) {
        this.index = index;
        item = i;
        GetComponent<Image>().material = i.UIMat;
    }
}
