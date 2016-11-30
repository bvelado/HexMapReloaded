using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Tile))]
public class TilePropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return label != GUIContent.none && Screen.width < 333 ? (16f + 18f) : 16f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);

        position.width = position.width / 2f;

        EditorGUI.PropertyField(position ,property.FindPropertyRelative("TileSO"), GUIContent.none);

        position.x = position.width + position.x;

        EditorGUI.PropertyField(position, property.FindPropertyRelative("MapPosition"), GUIContent.none);
        
        EditorGUI.EndProperty();
    }
}
