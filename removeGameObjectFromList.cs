using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Removes a GO to a GO list.")]
    public class removeGameObjectFromList : Action
    {
        [Tooltip("The GO value")]
        public SharedGameObject GO;
        [RequiredField]
        [Tooltip("The SharedVector3List to set")]
        public SharedGameObjectList storedVGOList;

        public override TaskStatus OnUpdate()
        {
           
                storedVGOList.Value.Remove(GO.Value);
           

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            GO = null;
            storedVGOList = null;
        }
    }
}