using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class AutomateObjectCreate : EditorWindow {
	public string objectName;
	public GameObject source;
	public int objectCount;
	public List<Material> materials = new List<Material>();
	private List<Material> savedMaterials = new List<Material>(10);

	[MenuItem("Custom/Item Setup Automation")]
	static void Setup() {
		EditorWindow.GetWindow<AutomateObjectCreate>("Item Setup Automation");
	}

	void OnGUI() {
		GUILayout.Label("Put your .blend",EditorStyles.boldLabel);

		objectName = EditorGUILayout.TextField("Desired Name", objectName);
		source = (GameObject) EditorGUILayout.ObjectField("Model (.blend)", source, typeof(GameObject), false);
		objectCount = EditorGUILayout.IntField("Count", objectCount);
		if (source != null) {
			Material[] mats = source.GetComponent<MeshRenderer>().sharedMaterials;
			int matCount = mats.Length;
			for (int i=0; i < matCount; i++) {
				mats[i] = (Material) EditorGUILayout.ObjectField("Material "+i.ToString(), mats[i], typeof(Material), false);
				source.GetComponent<MeshRenderer>().sharedMaterials = mats;
			}

		}
		if (GUILayout.Button("Setup Prefab")) {
		}
	}
}
