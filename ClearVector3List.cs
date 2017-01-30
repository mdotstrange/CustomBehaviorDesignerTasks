using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Removes a vector3 to a vector3 list at an index.")]
    public class ClearVector3List : Action
    {

        [RequiredField]
        [Tooltip("The SharedVector3List to set")]
        public SharedVector3List storedVector3List;
      

        public override void OnAwake()
        {
            //   storedVector3List.Value = new List<Vector3>();
        }

        public override TaskStatus OnUpdate()
        {



            storedVector3List.Value.Clear();


            return TaskStatus.Success;
        }

        public override void OnReset()
        {
          
            storedVector3List = null;
        }
    }
}