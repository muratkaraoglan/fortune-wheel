#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using FortuneWheel.Scripts.Item;
using FortuneWheel.Scripts.Stat;

namespace FortuneWheel.Scripts.Utils
{
 [CustomPropertyDrawer(typeof(StatEffect))]
public class StatEffectDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        if (property.managedReferenceValue == null)
        {
            if (GUI.Button(position, $"Add {label.text}"))
            {
                ShowTypeMenu(property);
            }
        }
        else
        {
            Rect fieldRect = new Rect(position.x, position.y, position.width - 60, position.height);
            Rect removeRect = new Rect(position.x + position.width - 55, position.y, 55, position.height);

            EditorGUI.PropertyField(fieldRect, property, label, true);

            if (GUI.Button(removeRect, "X"))
            {
                property.managedReferenceValue = null;
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
            return EditorGUIUtility.singleLineHeight;

        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    private void ShowTypeMenu(SerializedProperty property)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(StatEffect).IsAssignableFrom(p) && !p.IsAbstract);

        GenericMenu menu = new GenericMenu();
        foreach (var type in types)
        {
            var capturedType = type;
            menu.AddItem(new GUIContent(type.Name), false, () =>
            {
                property.managedReferenceValue = Activator.CreateInstance(capturedType);
                property.serializedObject.ApplyModifiedProperties();
            });
        }
        menu.ShowAsContext();
    }
}
#endif
}