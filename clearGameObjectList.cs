using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Clears a game object list.")]
    public class clearGameObjectList : Action
    {

        [RequiredField]
        [Tooltip("The SharedGameObjectList to clear")]
        public SharedGameObjectList storedGameObjectList;


        public override void OnAwake()
        {
          
        }

        public override TaskStatus OnUpdate()
        {



            storedGameObjectList.Value.Clear();


            return TaskStatus.Success;
        }

        public override void OnReset()
        {

            storedGameObjectList = null;
        }
    }
}