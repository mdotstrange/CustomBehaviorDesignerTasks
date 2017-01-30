using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Checks to see if vector3 list is empty.")]
    public class IsVector3ListEmpty : Action
    {

        [RequiredField]
        [Tooltip("The SharedVector3List to test")]
        public SharedVector3List storedVector3List;


        public override void OnAwake()
        {
            
        }

        public override TaskStatus OnUpdate()
        {



           int count = storedVector3List.Value.Count;

            if(count == 0)
            {
                return TaskStatus.Success;

            }
            else
            {
                return TaskStatus.Failure;

            }


            
        }

        public override void OnReset()
        {
            
            storedVector3List = null;
        }
    }
}