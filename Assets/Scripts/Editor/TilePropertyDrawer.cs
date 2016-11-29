using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Tile))]
public class TilePropertyDrawer : PropertyDrawer
{
    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //{
    //    return label != GUIContent.none && Screen.width < 333 ? (16f + 18f) : 16f;
    //}

    //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //{
    //    int oldIndentLevel = EditorGUI.indentLevel;
    //    label = EditorGUI.BeginProperty(position, label, property);
    //    Rect contentPosition = EditorGUI.PrefixLabel(position, label);
    //    if (position.height > 16f)
    //    {
    //        position.height = 16f;
    //        EditorGUI.indentLevel += 1;
    //        contentPosition = EditorGUI.IndentedRect(position);
    //        contentPosition.y += 18f;
    //    }
    //    contentPosition.width *= 0.75f;
    //    EditorGUI.indentLevel = 0;
    //    EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("TileSO"), GUIContent.none);
    //    contentPosition.x += contentPosition.width;
    //    contentPosition.width /= 3f;
    //    EditorGUIUtility.labelWidth = 14f;
    //    EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("MapPosition"));
    //    EditorGUI.EndProperty();
    //    EditorGUI.indentLevel = oldIndentLevel;
    //}

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int oldIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel += 1;
        label = EditorGUI.BeginProperty(position, label, property);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(property.FindPropertyRelative("TileSO"), GUIContent.none);
        EditorGUILayout.PropertyField(property.FindPropertyRelative("MapPosition"));
        EditorGUILayout.EndVertical();
        
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oldIndentLevel;
    }
}
