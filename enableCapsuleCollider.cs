using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityboxCollider
{
    [TaskCategory("Basic/Collider")]
    [TaskDescription("Enables/Disables a capsule collider. Returns Success.")]
    public class enableCapsuleCollider : Action
    {
        public SharedCollider capsuleCollider;
        public SharedBool enabled;


        public override TaskStatus OnUpdate()
        {


            capsuleCollider.Value.enabled = enabled.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {

            enabled = false;
        }
    }
}