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
        spline = (BezierSpline)EditorGUILayout.ObjectField("Bezier Spline", spline, typeof(BezierSpline), true);
        radius = EditorGUILayout.FloatField("Wire Radius", radius);
        circVertexCount = EditorGUILayout.IntField("Circ. Vertex Count", circVertexCount);
        circVertexCount = (circVertexCount < 3) ? 3 : circVertexCount;
        loopCount = EditorGUILayout.IntField("Loop Count", loopCount);
        loopCount = (loopCount < 1) ? 1 : loopCount;
        decoration = (Transform)EditorGUILayout.ObjectField("Decoration", decoration, typeof(Transform), true);
        hook = (Transform)EditorGUILayout.ObjectField("Hook", hook, typeof(Transform), true);
        if (spline == null) {
            GUI.enabled = false; 
        }           
        if (meshExists) {
            if (GUILayout.Button("Regenerate Mesh")) {
                //update vertices and decoration placements
                //update everything if any of the details have been changed
                splineDecorator.GenerateMesh(spline, radius, circVertexCount, loopCount, decoration, hook);
            }
        } else {
            if (GUILayout.Button("GenerateMesh")) {
                meshExists = true;
                splineDecorator.GenerateMesh(spline, radius, circVertexCount, loopCount, decoration, hook);
            }
        }
        if (spline == null) {
            GUI.enabled = true; 
            EditorGUILayout.LabelField("Cannot generate or update mesh without Bezier Spline");
        }
    }
}