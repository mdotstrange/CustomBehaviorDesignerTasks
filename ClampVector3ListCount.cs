using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Clamps a Vector3 list to the maxCount.")]
    public class ClampVector3ListCount : Action
    {

        [RequiredField]
        [Tooltip("The SharedVector3List to use")]
        public SharedVector3List storedVector3List;
        public SharedInt maxCount;

        public override TaskStatus OnUpdate()
        {
            int cutoffindex = maxCount.Value - 1;

            int itemCount = storedVector3List.Value.Count;

            if(itemCount >= maxCount.Value)
            {
                int countToRemove = itemCount - maxCount.Value;


                storedVector3List.Value.RemoveRange(cutoffindex, countToRemove);
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Success;

            }



           
        }

        public override void OnReset()
        {

            storedVector3List = null;
        
            maxCount = null;
        }
    }
}