using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnitySphereCollider
{
    [TaskCategory("Basic/SphereCollider")]
    [TaskDescription("Enables/Disables a SphereCollider. Returns Success.")]
    public class enableSphereCollider : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The radius of the SphereCollider")]
        public SharedBool enabled;

        private SphereCollider sphereCollider;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                sphereCollider = currentGameObject.GetComponent<SphereCollider>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (sphereCollider == null)
            {
                Debug.LogWarning("SphereCollider is null");
                return TaskStatus.Failure;
            }

            sphereCollider.enabled = enabled.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            enabled = false;
        }
    }
}