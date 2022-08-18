using UnityEditor;
using UnityEngine;

namespace TimeAttack
{
    [CustomEditor(typeof(GameEventVoidSO), editorForChildClasses: true)]
    public class VoidEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            GameEventVoidSO e = target as GameEventVoidSO;
            if (GUILayout.Button("Raise"))
                e.RaiseEvent();
        }
    }
}
