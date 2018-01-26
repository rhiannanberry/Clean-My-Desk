using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButtons : MonoBehaviour {
    public GameObject buttonPrefab;
    public List<ButtonSetup> buttons;
	// Use this for initialization
	void Start () {
        foreach (ButtonSetup button in buttons) {
            GameObject b = Instantiate(buttonPrefab);
            b.transform.SetParent(transform, false);
            b.transform.GetComponent<Image>().material = button.buttonMat;
            b.transform.GetComponent<SpawnItem>().SetPrefab(button.itemPrefab);
            b.transform.GetComponent<SpawnItem>().SetCount(button.count);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [System.Serializable]
    public struct ButtonSetup {
        public Material buttonMat;
        public GameObject itemPrefab;
        public int count;
    }
}
