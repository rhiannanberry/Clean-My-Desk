using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimedModeUtil : MonoBehaviour {
	
	public TextMeshProUGUI txt;
	public bool execute = true;

	void Start() {
		if (execute) {
			SetText();
		}
	}
	public void SetText() {
		txt.text = "#" + SaveData.sd.timeModeLevel.ToString();
	}
}
