using UnityEngine;
using UnityEditor;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour),true)]
public class SettingsComponentEditor : Editor {

    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector();

        if ( Application.isPlaying )
        {
            var type = target.GetType();

            foreach ( var method in type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) )
            {
                var attributes = method.GetCustomAttributes(typeof(UpdateVariables), true);
                if ( attributes.Length > 0 )
                {
                    if ( GUILayout.Button(method.Name) )
                    {
                        ((MonoBehaviour)target).Invoke(method.Name, 0f);
                    }
                }
            }
        }
    }
}