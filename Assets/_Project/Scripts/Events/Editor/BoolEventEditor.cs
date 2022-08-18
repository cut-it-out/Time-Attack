using UnityEditor;
using UnityEngine;

namespace TimeAttack
{
    [CustomEditor(typeof(GameEventBoolSO), editorForChildClasses: true)]
    public class BoolEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            GameEventBoolSO e = target as GameEventBoolSO;
            if (GUILayout.Button("Raise: true"))
                e.RaiseEvent(true);
            if (GUILayout.Button("Raise: false"))
                e.RaiseEvent(false);
        }
    }
}
