using UnityEngine;

namespace TimeAttack
{
	[CreateAssetMenu(fileName = "newAudioFileSO", menuName = "Audio/Audio File")]
	public class AudioFileSO : ScriptableObject
	{
		[SerializeField] AudioClip audioClip = default;
		[SerializeField] AudioSourceConfigurationSO settings = default;
		[SerializeField] bool looping = false;

		public AudioClip Clip => audioClip;
		public bool IsLooping => looping;
		public AudioSourceConfigurationSO Settings => settings;
	}
}
