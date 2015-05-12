#if UNITY_EDITOR 
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Manager))]
public class ManagerEditor : Editor
{
    Manager _manager;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _manager = (Manager) target;
        if (GUILayout.Button("Menu"))
        {
            _manager.activateMenu();
        }
        if (GUILayout.Button("Game"))
        {
            _manager.activateGame();
        }
        if (GUILayout.Button("Score"))
        {
            _manager.activateScore();
        }
    }
}
#endif
