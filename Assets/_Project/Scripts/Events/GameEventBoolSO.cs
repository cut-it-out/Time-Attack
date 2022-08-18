using UnityEngine;
using UnityEngine.Events;

namespace TimeAttack
{
	[CreateAssetMenu(menuName = "Events/Bool Game Event")]
	public class GameEventBoolSO : ScriptableObject
	{
		public event UnityAction<bool> OnEventRaised;

		public void RaiseEvent(bool value)
		{
			if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
			Debug.Log($"{name} raised with value {value}");
		}
	}

}
