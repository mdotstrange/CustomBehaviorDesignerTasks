using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityBehaviour
{
    [TaskCategory("Basic/NavMeshAgent")]
    [TaskDescription("Enables/Disables a navmesh agent component.")]
    public class enableAgent : Action
    {

        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        
        public SharedBool enable;
        private NavMeshAgent navMeshAgent;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                navMeshAgent = currentGameObject.GetComponent<NavMeshAgent>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {

            if (navMeshAgent == null)
            {
                Debug.LogWarning("NavMeshAgent is null");
                return TaskStatus.Failure;
            }

            navMeshAgent.enabled = enable.Value;

            return TaskStatus.Success;
        }

        

          

        public override void OnReset()
        {
            targetGameObject = null;
            enable = null;
           
        }
    }
}