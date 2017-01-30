using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityboxCollider
{
    [TaskCategory("Basic/boxCollider")]
    [TaskDescription("Enables/Disables a Box collider. Returns Success.")]
    public class enableBoxCollider : Action
    {    
        public SharedCollider boxCollider;
        public SharedBool enabled;
       

        public override TaskStatus OnUpdate()
        {
            if(boxCollider.Value == null)
            {
                return TaskStatus.Success;
            }
            

            boxCollider.Value.enabled = enabled.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
          
            enabled = false;
        }
    }
}