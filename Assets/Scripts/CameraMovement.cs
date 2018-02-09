using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    Vector3 center;
    Vector3 startRotation; //-x rotation is up
    Vector3 mousePos;

    public float verticalRange = 3;
    public float horizontalRange = 10;
    GameObject mouse;
    private float horizontalRatio, verticalRatio;
	// Use this for initialization
	void Start () {
        startRotation = transform.eulerAngles; //only change in x and y
        mouse = GameObject.Find("Mouse");
    }
	
	// Update is called once per frame
	void Update () {
        mousePos = mouse.GetComponent<RectTransform>().anchoredPosition3D;

        float deltaHorizontal = (mousePos.x * horizontalRange) / Screen.width - horizontalRange / 2;
        float deltaVertical = (-1 * mousePos.y * verticalRange) / Screen.height + verticalRange / 2;
        transform.eulerAngles = new Vector3(deltaVertical + startRotation.x, deltaHorizontal + startRotation.y, startRotation.z);

	}
}
