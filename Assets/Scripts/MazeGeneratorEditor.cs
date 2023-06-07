using UnityEditor;
[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
    }
}