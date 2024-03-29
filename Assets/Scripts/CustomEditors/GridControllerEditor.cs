using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(GridControls))]
public class GridControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GridControls controls = (GridControls)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Grid"))
        {
            controls.GenerateGrid();
            
        }
    }
}
