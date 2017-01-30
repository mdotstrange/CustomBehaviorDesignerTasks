using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Checks to see if list contains game object")]
    public class gameObjectListContains : Action
    {

        [RequiredField]
        [Tooltip("The SharedGameObjectList to check")]
        public SharedGameObjectList storedGameObjectList;
        public SharedGameObject doIExist;


        public override void OnAwake()
        {

        }

        public override TaskStatus OnUpdate()
        {
            if(storedGameObjectList.Value.Contains(doIExist.Value))
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

            storedGameObjectList = null;
        }
    }
}