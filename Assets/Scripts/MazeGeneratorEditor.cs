using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor{
    // Override the OnInspectorGUI with our own method
    public override void OnInspectorGUI(){
        // Get the target (the component that we want to add inspector elements to) and cast it to
        // the correct type to be stored in a variable
        MazeGenerator generator = (MazeGenerator) target;
        // Draw our usual default inspector controls before we add anything
        DrawDefaultInspector();
        // Add a "Generate" button to the inspector and call the Generate() method upon it being clicked
        if (GUILayout.Button("Generate")) generator.Generate();
    }
}