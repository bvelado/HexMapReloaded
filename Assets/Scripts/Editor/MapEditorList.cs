using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapEditorList {
    private static GUIContent moveButtonContent = new GUIContent("\u21b4", "move down");
    private static GUIContent duplicateButtonContent = new GUIContent("+", "duplicate");
    private static GUIContent deleteButtonContent = new GUIContent("-", "delete");
    private static GUIContent addButtonContent = new GUIContent("+", "add element");

    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    public static void Show(SerializedProperty list, string[] childProperties, EditorListOption options = EditorListOption.Default)
    {
        if (!list.isArray)
        {
            EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error);
            return;
        }

        bool showListLabel = (options & EditorListOption.ListLabel) != 0;
        bool showListSize = (options & EditorListOption.ListSize) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }
        if (!showListLabel || list.isExpanded)
        {
            if (showListSize)
            {
                EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            }
            ShowElements(list, childProperties, options);
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }

    private static void ShowElements(SerializedProperty list, string[] childProperties, EditorListOption options)
    {
        bool showElementLabels = (options & EditorListOption.ElementLabels) != 0;
        bool showButtons = (options & EditorListOption.Buttons) != 0;

        for (int i = 0; i < list.arraySize; i++)
        {
            if (showButtons)
            {
                EditorGUILayout.BeginHorizontal();
            }
            if (showElementLabels)
            {
                DisplayListItem(list.GetArrayElementAtIndex(i), childProperties);
            }
            else
            {
                DisplayListItem(list.GetArrayElementAtIndex(i), childProperties, false);
            }
            if (showButtons)
            {
                ShowButtons(list, i);
                EditorGUILayout.EndHorizontal();
            }
        }

        if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
        {
            list.arraySize += 1;
        }
    }

    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            list.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
        {
            list.InsertArrayElementAtIndex(index);
        }
        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }
    }

    private static void DisplayListItem(SerializedProperty item, string[] relativeProperties, bool showLabel = true)
    {
        if (relativeProperties.Length > 0)
        {
            foreach (var relativeProperty in relativeProperties)
            {
                Debug.Log("Displaying " + relativeProperty);
                if(showLabel)
                    EditorGUILayout.PropertyField(item.FindPropertyRelative(relativeProperty));
                else
                    EditorGUILayout.PropertyField(item.FindPropertyRelative(relativeProperty), GUIContent.none);
            }
        }
        else
        {
            EditorGUILayout.PropertyField(item);
        }
    }
}
