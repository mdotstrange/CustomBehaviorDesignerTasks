using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityNavMeshAgent
{
    [TaskCategory("Basic/NavMeshAgent")]
    [TaskDescription("Gets the closest navmesh edge. Returns success if edge is found, failure if no edge is found.")]
    public class GetClosestNavEdge : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject NavAgentGameObject;

        [Tooltip("True if a nearest edge is found.")]
        public SharedBool nearestEdgeFound;

        [Tooltip("Position of hit")]
        public SharedVector3 position;

        [Tooltip("Normal at the point of hit")]
        public SharedVector3 normal;

        [Tooltip("Distance to the point of hit")]
        public SharedFloat distance;

        [Tooltip("Mask specifying NavMeshLayers at point of hit.")]
        public SharedInt mask;

        [Tooltip("Flag when hit")]
        public SharedBool hit;

        // cache the navmeshagent component
        private NavMeshAgent _agent;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(NavAgentGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                _agent = currentGameObject.GetComponent<NavMeshAgent>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_agent == null)
            {
                Debug.LogWarning("NavMeshAgent is null");
                return TaskStatus.Failure;
            }

            //Debug.Log(_agent + " agent");

            NavMeshHit _NavMeshHit;

            bool _nearestEdgeFound = _agent.FindClosestEdge(out _NavMeshHit);
            nearestEdgeFound.Value = _nearestEdgeFound;

            position.Value = new Vector3(_NavMeshHit.position.x,NavAgentGameObject.Value.gameObject.transform.position.y, _NavMeshHit.position.z);
            //Debug.Log("Edge position " + position.Value);
            normal.Value = _NavMeshHit.normal;
            distance.Value = _NavMeshHit.distance;
            mask.Value = _NavMeshHit.mask;
            hit.Value = _NavMeshHit.hit;

            if(nearestEdgeFound.Value == true)
            {
                //Debug.Log("SUCCESS edge found");
                return TaskStatus.Success;
            }
            else
            {
                //Debug.Log("FAILURE NO edge found");
                return TaskStatus.Failure;

            }

         



        }

        public override void OnReset()
        {
            NavAgentGameObject = null;
           
        }
    }
}