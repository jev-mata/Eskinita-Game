using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PowerUps))]
public class PowerUpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var powerUpTypeProp = serializedObject.FindProperty("powerUpType");
        EditorGUILayout.PropertyField(powerUpTypeProp);

        switch ((PowerUpType)powerUpTypeProp.enumValueIndex)
        {
            case PowerUpType.Life:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lifeAmount"));
                break;
            case PowerUpType.Skill:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skillDuration"));
                break;
        }
 
        serializedObject.ApplyModifiedProperties();
    }
}
