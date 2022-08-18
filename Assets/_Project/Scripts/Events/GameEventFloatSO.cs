using UnityEngine;
using UnityEngine.Events;

namespace TimeAttack
{
    [CreateAssetMenu(menuName = "Events/Float Game Event")]
    public class GameEventFloatSO : ScriptableObject
    {
        public event UnityAction<float> OnEventRaised;

        public void RaiseEvent(float value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            Debug.Log($"{name} raised with value {value}");
        }
    }
}
