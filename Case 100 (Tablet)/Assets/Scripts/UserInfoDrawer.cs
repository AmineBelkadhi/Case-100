using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScoreInfo))]
public class UserInfoDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        
        return EditorGUIUtility.singleLineHeight * 4 + 24;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.LabelField(position, label);


        var mintimeRect = new Rect(position.x, position.y + 18, position.width, 16);
        var maxtimeRect = new Rect(position.x, position.y + 36, position.width, 16);
        var idRect = new Rect(position.x, position.y + 54, position.width, 16);
        var scoreRect = new Rect(position.x, position.y + 72, position.width, 16);


        EditorGUI.indentLevel++;

        EditorGUI.PropertyField(mintimeRect, property.FindPropertyRelative("minTime"));
        EditorGUI.PropertyField(maxtimeRect, property.FindPropertyRelative("maxTime"));
        EditorGUI.PropertyField(scoreRect, property.FindPropertyRelative("score"));
        EditorGUI.PropertyField(idRect, property.FindPropertyRelative("Id"));
        

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }
}
