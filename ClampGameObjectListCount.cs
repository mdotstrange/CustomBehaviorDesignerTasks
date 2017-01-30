using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Clamps a game object list to the maxCount.")]
    public class ClampGameObjectListCount : Action
    {

        [RequiredField]
        [Tooltip("The SharedGameObjectList to use")]
        public SharedGameObjectList storedGameObjectList;
        public SharedInt maxCount;       

        public override TaskStatus OnUpdate()
        {

            int cutoffindex = maxCount.Value - 1;

            int itemCount = storedGameObjectList.Value.Count;

            if (itemCount >= maxCount.Value)
            {
                int countToRemove = itemCount - maxCount.Value;


                storedGameObjectList.Value.RemoveRange(cutoffindex, countToRemove);
                return TaskStatus.Success;
            } else
            {
                return TaskStatus.Success;

            }


        }

        public override void OnReset()
        {
         
            storedGameObjectList = null;    
            maxCount = null;
        }
    }
}