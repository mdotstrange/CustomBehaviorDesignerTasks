using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedAudioClip : SharedVariable<AudioClip>
    {
        public static implicit operator SharedAudioClip(AudioClip value) { return new SharedAudioClip { mValue = value }; }
    }
}