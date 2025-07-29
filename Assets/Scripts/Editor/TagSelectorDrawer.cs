using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            string[] tags = InternalEditorUtility.tags;

            int index = System.Array.IndexOf(tags, property.stringValue);
            if (index < 0) index = 0;

            EditorGUI.BeginProperty(position, label, property);
            index = EditorGUI.Popup(position, label.text, index, tags);
            property.stringValue = tags[index];
            EditorGUI.EndProperty();
        }
        else if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
        {
            EditorGUI.LabelField(position, label.text, "Use TagSelector with string fields only");
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use TagSelector with string fields only");
        }
    }
}
