using UnityEditor;
using UnityEngine;

namespace TimeAttack
{
    [CustomEditor(typeof(GameEventFloatSO), editorForChildClasses: true)]
    public class FloatEventEditor : Editor
    {
        float testValue = 0f;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;
                        
            GameEventFloatSO e = target as GameEventFloatSO;
            testValue = EditorGUILayout.FloatField(testValue);

            if (GUILayout.Button("Raise event"))
            {                
                e.RaiseEvent(testValue);
            }
        }
    }
}
