using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Checks to see if game object list is empty.")]
    public class isGameObjectListEmpty : Action
    {

        [RequiredField]
        [Tooltip("The List to test")]
        public SharedGameObjectList storedList;


        public override void OnAwake()
        {

        }

        public override TaskStatus OnUpdate()
        {



            int count = storedList.Value.Count;

            if (count == 0)
            {
                return TaskStatus.Success;

            } else
            {
                return TaskStatus.Failure;

            }



        }

        public override void OnReset()
        {

            storedList = null;
        }
    }
}