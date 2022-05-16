using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    Object sceneObject;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Set the scene here instead of setting the name. The name will get set automatically", MessageType.None);

        sceneObject = EditorGUILayout.ObjectField(sceneObject, typeof(SceneAsset), true);
        if(sceneObject)
        {
            serializedObject.FindProperty("scene").stringValue = sceneObject.name;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
