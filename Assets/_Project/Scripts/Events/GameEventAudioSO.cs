using UnityEngine;
using UnityEngine.Events;

namespace TimeAttack
{
    [CreateAssetMenu(menuName = "Events/Audio Game Event")]
    public class GameEventAudioSO : ScriptableObject
    {
        public event UnityAction<AudioFileSO> OnPlayEventRaised;
        public event UnityAction OnStopEventRaised;

        public void RaisePlayEvent(AudioFileSO value)
        {
            if (OnPlayEventRaised != null)
                OnPlayEventRaised.Invoke(value);
        }

        public void RaiseStopEvent()
        {
            if (OnStopEventRaised != null)
                OnStopEventRaised.Invoke();
        }
    }
}
