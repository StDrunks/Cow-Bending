using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetAspectRatio))]
public class SetAspectRatioEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SetAspectRatio setAspectRatio = (SetAspectRatio)target;

        if (GUILayout.Button("Set Ratio"))
        {
            setAspectRatio.SetRatio();
        }
    }
}