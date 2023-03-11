using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomMovement))]
public class RoomBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RoomMovement roomM = (RoomMovement)target;
        if (GUILayout.Button("Create Floor")) roomM.createFloor();


    }
}
