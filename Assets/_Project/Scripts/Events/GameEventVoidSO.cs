using UnityEngine;
using UnityEngine.Events;

namespace TimeAttack
{
	[CreateAssetMenu(menuName = "Events/Void Game Event")]
	public class GameEventVoidSO : ScriptableObject
	{
		public UnityAction OnEventRaised;

		public void RaiseEvent()
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke();
        }
	}

}
