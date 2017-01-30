using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Removes a vector3 to a vector3 list.")]
    public class RemoveVector3FromList : Action
    {
        [Tooltip("The Vector3 value")]
        public SharedVector3[] Vector;
        [RequiredField]
        [Tooltip("The SharedVector3List to set")]
        public SharedVector3List storedVector3List;

        public override void OnAwake()
        {
         
        }

        public override TaskStatus OnUpdate()
        {
            if (Vector == null || Vector.Length == 0)
            {
                return TaskStatus.Failure;
            }

            for (int i = 0; i < Vector.Length; ++i)
            {
                storedVector3List.Value.Remove(Vector[i].Value);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            Vector = null;
            storedVector3List = null;
        }
    }
}