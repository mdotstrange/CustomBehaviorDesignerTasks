using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPhysics
{
    [TaskCategory("Basic/Physics")]
    [TaskDescription("Enables/Disables colliders in a collider List")]

    public class enableColliders : Action
    {
        [Tooltip("The collider list")]
        public SharedColliderList colliderList;
        public SharedBool enable;

        public override TaskStatus OnUpdate()
        {
            foreach(Collider element in colliderList.Value )
            {
                element.enabled = enable.Value;

            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            enable = false;         
        }
    }
}