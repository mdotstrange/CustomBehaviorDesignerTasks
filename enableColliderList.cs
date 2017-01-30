using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityboxCollider
{
    [TaskCategory("Basic/boxCollider")]
    [TaskDescription("Enables/Disables a Box collider list.")]
    public class enableColliderList : Action
    {
        public SharedColliderList colliderList;
        public SharedBool enable;


        public override TaskStatus OnUpdate()
        {

            if(colliderList.Value == null)
            {
                return TaskStatus.Success;
            }    
            if(colliderList.Value.Count == 0)
            {
                return TaskStatus.Success;
            }

            for (int index = 0; index < colliderList.Value.Count; index++)
            {
                var coll = colliderList.Value[index];
                coll.enabled = enable.Value;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {

            enable = false;
        }
    }
}