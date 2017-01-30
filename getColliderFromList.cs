using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Gets a Collider from a Collider list at an index.")]
    public class GetColliderFromList : Action
    {

        [RequiredField]
        [Tooltip("The SharedColliderList to set")]
        public SharedColliderList storedColliderList;
        public SharedInt index;

        public SharedCollider theCollider;

        public override void OnAwake()
        {
         
        }

        public override TaskStatus OnUpdate()
        {

            theCollider.Value = storedColliderList.Value[index.Value];

         

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            index = 0;
            storedColliderList = null;
           
        }
    }
}