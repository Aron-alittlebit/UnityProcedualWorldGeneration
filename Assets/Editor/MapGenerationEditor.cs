using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mg = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mg.AutoUpdate)
            {
                mg.GenerateMap();
            }
        }
        if (GUILayout.Button("Generate"))
        {
            mg.GenerateMap();
        }
    }
}
