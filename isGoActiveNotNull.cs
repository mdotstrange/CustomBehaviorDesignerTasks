using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Returns success if the target is still active and not null.")]
    public class isGoActiveNotNull : Action
    {

        public SharedGameObject targetGameObject;

        public override TaskStatus OnUpdate()
        {
            if(targetGameObject.Value == null)
            {
                return TaskStatus.Failure;
            }


            if (targetGameObject.Value.gameObject.activeInHierarchy == true && targetGameObject.Value != null)
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
            targetGameObject = null;
        }
    }
}