using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Removes a vector3 to a vector3 list at an index.")]
    public class RemoveVector3IndexFromList : Action
    {
       
        [RequiredField]
        [Tooltip("The SharedVector3List to set")]
        public SharedVector3List storedVector3List;
        public SharedInt index;

        public override void OnAwake()
        {
          
        }

        public override TaskStatus OnUpdate()
        {
           

           
                storedVector3List.Value.RemoveAt(index.Value);


            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            index = 0;
            storedVector3List = null;
        }
    }
}