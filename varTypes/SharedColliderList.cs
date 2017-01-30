using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedColliderList : SharedVariable<List<Collider>>
    {
        public static implicit operator SharedColliderList(List<Collider> value) { return new SharedColliderList { mValue = value }; }
    }
}