using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Adds a vector3 to a vector3 list.")]
    public class AddVector3ToVector3List : Action
    {
        [Tooltip("The Vector3 value")]
        public SharedVector3[] Vector;
        [RequiredField]
        [Tooltip("The SharedVector3List to set")]
        public SharedVector3List storedVector3List;

        public override void OnAwake()
        {
         //   storedVector3List.Value = new List<Vector3>();
        }

        public override TaskStatus OnUpdate()
        {
            if (Vector == null || Vector.Length == 0)
            {
                return TaskStatus.Failure;
            }

         //   storedVector3List.Value.Clear();
            for (int i = 0; i < Vector.Length; ++i)
            {
                storedVector3List.Value.Add(Vector[i].Value);
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