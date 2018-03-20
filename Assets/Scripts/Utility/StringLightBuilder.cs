using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringLightBuilder : MonoBehaviour {
	[SerializeField]
	private bool hasNextSibling = false;
	[SerializeField]
	private bool hasPreviousSibling = false;
	private Transform nextSibling;
	private Transform previousSibling;
	private LineRenderer lineRenderer;
	void Start () {
		if (gameObject.GetComponent<LineRenderer>() != null){
			lineRenderer = gameObject.GetComponent<LineRenderer>();
			lineRenderer.enabled = false;
		}
		
		SetPreviousSibling();
	}
	
	// Update is called once per frame
	void Update () {
		if (hasNextSibling) {
			Vector3[] points = {transform.position, nextSibling.transform.position};
			lineRenderer.SetPositions(points);
		}	
	}

	public void SetNextSibling() {
		if (lineRenderer != null) {
			lineRenderer.enabled = true;
		} else {
			lineRenderer = gameObject.AddComponent<LineRenderer>();
		}
		lineRenderer.startColor = Color.black;
		lineRenderer.widthMultiplier = .1f;
		lineRenderer.positionCount = 2;
		nextSibling = transform.parent.GetChild(transform.GetSiblingIndex() + 1);
		hasNextSibling = true;
	}

	private void SetPreviousSibling() {
		if (transform.GetSiblingIndex() != 0) {
			hasPreviousSibling = true;
			previousSibling = transform.parent.GetChild(transform.GetSiblingIndex() - 1);
			previousSibling.GetComponent<StringLightBuilder>().SetNextSibling();
		}
	}
}
