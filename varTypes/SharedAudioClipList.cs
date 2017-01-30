using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedAudioClipList : SharedVariable<List<AudioClip>>
    {
        public static implicit operator SharedAudioClipList(List<AudioClip> value) { return new SharedAudioClipList { mValue = value }; }
    }
}