using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSetupController : MonoBehaviour {
	public GameObject buttonPrefab;
	public GameObject defaultSetup;
    public GameObject buttonContainer;
	// Use this for initialization
	void Start () {
		foreach(Transform obj in transform.GetComponentInChildren<Transform>()) {
			//views default set to inactive so they dont show up in the game preview
			obj.gameObject.SetActive(true);
			SetupObjectButton(obj);
		}
	}
	
	private void SetupObjectButton(Transform objView) {
		ObjectView ov = objView.GetComponent<ObjectView>();

		RenderTexture rt = new RenderTexture(256, 256, 24, RenderTextureFormat.ARGB32);
		rt.Create();

		objView.Find("Camera").GetComponent<Camera>().targetTexture = rt;
		Material mt = new Material(Shader.Find("Unlit/UnlitMask"));
		mt.mainTexture = rt;

		GameObject b = Instantiate(buttonPrefab);
		b.GetComponent<ObjectButton>().SetValues(ov.prefab, ov.itemCount);

		mt.SetTexture("_AlphaTex", b.transform.GetChild(0).GetComponent<Image>().material.GetTexture("_AlphaTex"));

		b.transform.GetChild(0).GetComponent<Image>().material = mt;
		b.transform.SetParent(buttonContainer.transform, false);

		foreach(Transform untagged in defaultSetup.GetComponentInChildren<Transform>()) {
			if (untagged.name.Contains(ov.prefab.name)) {
				untagged.GetComponent<Obj>().buttonReference = b;
			}
		}

	}
}
