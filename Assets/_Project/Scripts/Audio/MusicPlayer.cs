using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeAttack
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] AudioFileSO musicTrack;
        [SerializeField] GameEventAudioSO MusicEventChannel;

        private void Start()
        {
            MusicEventChannel.RaisePlayEvent(musicTrack);
        }
    }
}
