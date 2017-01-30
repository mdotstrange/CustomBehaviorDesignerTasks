using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Checks to see if list contains V3")]
    public class Vector3ListContain : Action
    {

        [RequiredField]
   
        public SharedVector3List storedList;
        public SharedVector3 doIExist;


        public override void OnAwake()
        {

        }

        public override TaskStatus OnUpdate()
        {
            if (storedList.Value.Contains(doIExist.Value))
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

            storedList = null;
        }
    }
}