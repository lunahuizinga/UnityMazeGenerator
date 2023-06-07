using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor{
    public override void OnInspectorGUI(){
        MazeGenerator generator = (MazeGenerator) target;
        DrawDefaultInspector();
        if (GUILayout.Button("Generate")) generator.Generate();
    }
}