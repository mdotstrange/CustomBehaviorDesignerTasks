using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityBehaviour
{
    [TaskCategory("Basic/NavMeshAgent")]
    [TaskDescription("Tests if an agent is enabled.")]
    public class isAgentEnabled : Action
    {

        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;

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

            if(navMeshAgent.enabled)
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