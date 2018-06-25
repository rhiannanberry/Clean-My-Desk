using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltips : MonoBehaviour {
	private string[] tips = {
		"Throwing items off of the desk puts them back in your inventory!",
		"Open your item drawer with the i key.",
		"You can only place new objects when the item drawer is open.",
		"You can only pick up and move old objects when the item drawer is closed.",
		"Get yourself something nice. You deserve to be happy!",
		"The Main mode has item limits, while God mode does not.",
		"Press ESCAPE to open the options menu.",
		"You can control the music playlist through one of the monitors."
	};
	// Use this for initialization
	void Start () {
		int index = Random.Range(0, tips.Length);
		transform.GetComponent<TextMeshProUGUI>().text = "Tip: " + tips[index];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
