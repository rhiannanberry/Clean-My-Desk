using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSetupController : MonoBehaviour {
	public GameObject buttonPrefab;

    public GameObject buttonContainer;
	// Use this for initialization
	void Start () {
		foreach(Transform obj in transform.GetComponentInChildren<Transform>()) {
			SetupObjectButton(obj);
		}
	}
	
	private void SetupObjectButton(Transform objView) {
		ObjectView ov = objView.GetComponent<ObjectView>();

		RenderTexture rt = new RenderTexture(256, 256, 24, RenderTextureFormat.ARGB32);
		rt.Create();

		objView.Find("Camera").GetComponent<Camera>().targetTexture = rt;
		Material mt = new Material(Shader.Find("Unlit/Texture"));
		mt.mainTexture = rt;

		GameObject b = Instantiate(buttonPrefab);
		b.GetComponent<ObjectButton>().SetValues(ov.prefab, ov.itemCount);
		b.GetComponent<Image>().material = mt;
		b.transform.SetParent(buttonContainer.transform, false);

	}
}
