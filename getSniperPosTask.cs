//by MDS
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Gets closest nav edge and Sniper Pos.")]
    public class getSniperPosTask : Action
    {
        public SharedGameObject NavAgentGameObject;
        public SharedGameObject target;
        [Tooltip("If no target, v3 will be used")]
        public SharedVector3 TargetV3;
        [Tooltip("Y position differences smaller than this will be ignored")]
        public SharedFloat edgeTolerance;
        [Tooltip("Anything over this returns failure")]
        public SharedFloat distanceThreshold;
        public SharedVector3 SniperPosition;
        public SharedVector3List vertList;
        private float edgeY;
        private float agentY;
        private int closestIndex;
        private int closestIndex1;
        private NavMeshAgent _agent;
        private GameObject prevGameObject;
        private float distance;

        //List<Vector3> vertsAtEdgeLevel;

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
            NavMeshHit _NavMeshHit;

            bool _nearestEdgeFound = _agent.FindClosestEdge(out _NavMeshHit);
           
            if(_nearestEdgeFound == false)
            {
                return TaskStatus.Failure;
            }
      
            var closestEdge = new Vector3(_NavMeshHit.position.x, NavAgentGameObject.Value.gameObject.transform.position.y, _NavMeshHit.position.z);              
            distance = _NavMeshHit.distance;

            if(distance >= distanceThreshold.Value)
            {
                return TaskStatus.Failure;
            }

            List<Vector3> vertsAtEdgeLevel = new List<Vector3>();
           
            var agent = NavAgentGameObject.Value;          
            edgeY = closestEdge.y;
            agentY = agent.gameObject.transform.position.y;

            for (int index = 0; index < vertList.Value.Count; index++)
            {
                var v = vertList.Value[index];
                if (Mathf.Abs(v.y - edgeY) <= edgeTolerance.Value)
                {
                    vertsAtEdgeLevel.Add(v);
                }
            }
            float sqrDist1 = Mathf.Infinity;
            int _index1 = 0;
            float sqrDistTest1;

            if(target.Value != null)
            {
                TargetV3.Value = target.Value.gameObject.transform.position;
            }
           
            for (int i = 0; i < vertsAtEdgeLevel.Count; i++)
            {
                Vector3 singleVector1 = vertsAtEdgeLevel[i];
                //Debug.Log("Singlevecotr1 " + singleVector1);

                sqrDistTest1 = (singleVector1 - closestEdge + singleVector1 - TargetV3.Value).sqrMagnitude;
                if (sqrDistTest1 <= sqrDist1)
                {
                    sqrDist1 = sqrDistTest1;
                    //SniperPosition.Value = singleVector1;
                    SniperPosition.Value = new Vector3(singleVector1.x, agentY, singleVector1.z);
                    closestIndex1 = _index1;
                }

            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
           

        }
    }
}
