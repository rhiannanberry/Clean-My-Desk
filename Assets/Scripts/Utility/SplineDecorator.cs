using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplineDecorator : MonoBehaviour {

	public BezierSpline spline;


	private float radius;
	private int loopCount, cVertCount;
	private Transform decoration, hook;


	public GameObject debugTextPrefab;

	public int frequency;

	public float wireRadius = .1f;

	public int radiusPointNum = 4;

	public bool lookForward;

	public Transform endPoint;

	public Transform[] items;

	private Queue<float> freePointTs;

	private void Awake () {
		//size is radiusPointNum * frequency
		freePointTs = new Queue<float>();
		Vector3[] verts = new Vector3[radiusPointNum * frequency];
		Vector3[] norms = new Vector3[radiusPointNum * frequency];
		Vector2[] uvs = new Vector2[radiusPointNum * frequency];
		List<int> tris = new List<int>();
		DiscreteDecoration(); 
		
		float splineDelta = 1.0f/(frequency-1); //also for uv delta
		float uvDelta = 1.0f/(radiusPointNum);
		for (int x = 0; x < frequency; x++) {
			float t = splineDelta * x;
			t = FudgeT(t);
			List<Vector3> loop = GenerateLoop(spline.GetPoint(t) - transform.position, spline.GetDirection(t));

			int mult = x * loop.Count;
			for (int i  = 0; i < loop.Count; i++) {
				//GameObject txtObj = Instantiate(debugTextPrefab, loop[i] + transform.position, Quaternion.identity);
				//txtObj.GetComponent<TextMesh>().text = (mult + i).ToString();
				verts[mult + i] = loop[i];
				norms[mult + i] = loop[i];
				uvs[mult + i] = new Vector2(x * splineDelta, (i == 0 ? 0 : (i*1.0f)/(loop.Count - 1)));
			}

			int triCount = tris.ToArray().Length / 3;
			int[] triArr = tris.ToArray();

			Mesh mesh = new Mesh();
			mesh.vertices = verts;
			mesh.uv = uvs;
			mesh.triangles = GenerateTris(verts);
			mesh.RecalculateBounds();
			gameObject.GetComponent<MeshFilter>().mesh = mesh;

		}
	}

	void Update() {

	}

	public void GenerateMesh(BezierSpline spline, float radius, int cVertCount, int loopCount, Transform decoration, Transform hook) {
		this.spline = spline;
		this.radius = radius;
		this.cVertCount = cVertCount;
		this.loopCount = loopCount;
		this.decoration = decoration;
		this.hook = hook;
		GenerateVertices();
	}

	private void GenerateVertices() {
		List<int> freePoints = GetFreeControlPoints();
		int cpLen = spline.ControlPointCount;
		bool startHanging, endHanging;
		startHanging = freePoints[0] != 0;
		endHanging = freePoints[freePoints.Count-1] != spline.ControlPointCount-1;
		if (startHanging) freePoints.Insert(0, 0);
		if (endHanging) freePoints.Add(spline.ControlPointCount-1);

		for (int i = 0; i < freePoints.Count-1; i++) {
			float startT, endT, tRange, tDelta;
			startT = 1.0f*(freePoints[i])/(cpLen-1);
			endT = 1.0f*(freePoints[i+1])/(cpLen-1);
			tRange = endT - startT;
			tDelta = tRange / (loopCount-1);

			for(int j = 0; j < loopCount; j++) {
				List<Vector3> loop = GenerateLoop(spline.GetPoint(t) - transform.position, spline.GetDirection(t));


				if (i != freePoints.Count-2 && j == loopCount - 2) {
					j++;
				}
			}

		}
	}
		
		int[] GenerateTris(Vector3[] verts) {
			List<int> tris = new List<int>();
			int cnt = 0;
			while (cnt < (verts.Length - radiusPointNum)) { //12 - 8 = 4
		//	Debug.Log(cnt);						//0 1 2 3  4 5 6 7  8 9 10 11	
			int i = cnt % radiusPointNum;		//0 1 2 3, 0 1 2 3, 0 1 2  3 
			int current = cnt - i;				 	//0 0 0 0, 4 4 4 4, 8 8 8  8 
			int next = current + radiusPointNum; 	//4 4 4 4, 8 8 8 8, 12__ 

			int pa1 = current + i;						// 0 1 2 3  4 5  6  7	8  9  10 11
			int pa2 = current + (i + 1)%radiusPointNum;	// 1 2 3 0	5 6  7  4	9  10 11 8
			int pa3 = next +  (i + 1)%radiusPointNum;  	// 5 6 7 4	9 10 11 8	13 14 15 12

			int pb1 = current + i;						// 0 1 2 3	4 5  6  7	8  9  10 11
			int pb2 = next + i;							// 4 5 6 7  8 9  10 11	12 13 14 15
			int pb3 = next + ((i + 1)%radiusPointNum);	// 5 6 7 4	9 10 11 8	13 14 15 12

			tris.Add(pa1);
			tris.Add(pa2);
			tris.Add(pa3);


			tris.Add(pb3);
			tris.Add(pb2);
			tris.Add(pb1);

			cnt++; 
		}
		return tris.ToArray();
	}

	List<Vector3> GenerateLoop(Vector3 position, Vector3 direction) {
		List<Vector3> loop = new List<Vector3>();
		Quaternion rotation  = Quaternion.LookRotation(direction);
		float theta = 0f;
		float thetaScale = 1.0f / radiusPointNum;
		for (int i = 0; i < radiusPointNum; i++) {
			theta += (2.0f * Mathf.PI * thetaScale);
			Vector3 radiusPoint = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * wireRadius;
			loop.Add(position + rotation * radiusPoint);
		}
		return loop;
	}

	private float FudgeT(float t) {
		if (freePointTs.Peek() <= t) {
			t = freePointTs.Dequeue();
		}
		return t;

	}

	private void DiscreteDecoration() {
		
		/*/
		int stepSize = 

		if (spline.Loop || stepSize == 1) {
			stepSize = 1f / stepSize;
		}
		else {
			stepSize = 1f / (stepSize - 1);
		}
		for (int p = 0, f = 0; f < frequency; f++) {
			for (int i = 0; i < items.Length; i++, p++) {
				Transform item;
					item = Instantiate(items[i]) as Transform;

				Vector3 position = spline.GetPoint(p * stepSize);
				item.transform.localPosition = position;
				if (lookForward) {
					item.transform.LookAt(position + spline.GetDirection(p * stepSize));
				}
				item.transform.parent = transform;
			}
		}

		*/
		int ctrlPtCnt = spline.ControlPointCount;//
		for (int i = 0; i < ctrlPtCnt; i++) {
			if (spline.GetControlPointMode(i) == BezierControlPointMode.Free) {
				if (i != 0) {
					i++; //gets to middle free ctrl pt
				}
				Transform item = Instantiate(endPoint) as Transform;
				freePointTs.Enqueue(i/((ctrlPtCnt-1)*1.0f));
		Debug.Log(ctrlPtCnt);
				Vector3 position = spline.GetPoint(i/((ctrlPtCnt-1) * 1.0f));
				item.transform.localPosition = position;
				item.transform.parent = transform;
				i++; //skips right hand free ctrl pt
			}
		}
	}

	private List<int> GetFreeControlPoints() {
		List<int> freePoints = new List<int>();
		int ctrlPtCnt = spline.ControlPointCount;
		for(int i = 0; i < ctrlPtCnt; i++) {
			if (spline.GetControlPointMode(i) == BezierControlPointMode.Free) {
				if (i != 0) {
					i++;
				}
				freePoints.Add(i);
				i++;
			}
		}
		return freePoints;
	}
}