using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SplineDecorator))]
public class SplineDecoratorInspector : Editor {
    private SplineDecorator splineDecorator;
    private BezierSpline spline;
    private bool meshExists = false;

    private float radius;
    private int circVertexCount;
    private int loopCount;

    private Transform decoration;
    private Transform hook;

    public override void OnInspectorGUI() {
        EditorStyles.label.wordWrap = true;
        splineDecorator = target as SplineDecorator;
        splineDecorator.spline = (BezierSpline)EditorGUILayout.ObjectField("Bezier Spline", splineDecorator.spline, typeof(BezierSpline), true);
        splineDecorator.radius = EditorGUILayout.FloatField("Wire Radius", splineDecorator.radius);
        splineDecorator.cVertCount = EditorGUILayout.IntField("Circ. Vertex Count", splineDecorator.cVertCount);
        splineDecorator.cVertCount = (splineDecorator.cVertCount < 3) ? 3 : splineDecorator.cVertCount;
        splineDecorator.loopCount = EditorGUILayout.IntField("Loop Count", splineDecorator.loopCount);
        splineDecorator.loopCount = (splineDecorator.loopCount < 1) ? 1 : splineDecorator.loopCount;
        splineDecorator.decorationFrequency = EditorGUILayout.IntField("Decoration Freq.", splineDecorator.decorationFrequency);
        splineDecorator.decoration = (Transform)EditorGUILayout.ObjectField("Decoration", splineDecorator.decoration, typeof(Transform), true);
        splineDecorator.hook = (Transform)EditorGUILayout.ObjectField("Hook", splineDecorator.hook, typeof(Transform), true);
        if (splineDecorator.spline == null) {
            GUI.enabled = false; 
        }           
        if (meshExists) {
            if (GUILayout.Button("Regenerate Mesh")) {
                //update vertices and decoration placements
                //update everything if any of the details have been changed
                splineDecorator.GenerateMesh();
            }
        } else {
            if (GUILayout.Button("GenerateMesh")) {
                meshExists = true;
                splineDecorator.GenerateMesh();
            }
        }
        if (splineDecorator.spline == null) {
            GUI.enabled = true; 
            EditorGUILayout.LabelField("Cannot generate or update mesh without Bezier Spline");
        }
    }

    private void OnSceneGUI() {

    }
}