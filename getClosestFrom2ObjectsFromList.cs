using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Gets the closest game object in a list compared to two game objects.")]
    public class getClosestFrom2ObjectsFromList : Action
    {

        [RequiredField]
        [Tooltip("The SharedVector3List to use")]
        public SharedGameObjectList storedGameObjectList;
        public SharedGameObject agent_GO;
        public SharedGameObject target_GO;
        public SharedVector3 distanceFrom;
        public SharedVector3 distanceFrom1;
        public SharedGameObject closestGameObject;
        public SharedInt closestIndex;
        private Vector3 positionToTest;
        private Vector3 positionToTest1;
        public SharedFloat distance;
        private SharedVector3 closestVector3;


        public override void OnAwake()
        {

        }

        public override TaskStatus OnUpdate()
        {
            //Debug.Log("agent_GO value " + agent_GO.Value);

            if (agent_GO.Value != null)
            {
                positionToTest = agent_GO.Value.gameObject.transform.position;

            } else
            {
                positionToTest = distanceFrom.Value;

            }
            if (target_GO.Value != null)
            {
                positionToTest1 = target_GO.Value.gameObject.transform.position;

            } else
            {
                positionToTest1 = distanceFrom1.Value;

            }



            float sqrDist = Mathf.Infinity;
            int _index = 0;
            float sqrDistTest;
            // removed foreach loop
            //foreach (GameObject singleGO in storedGameObjectList.Value)
            for (int index = 0; index < storedGameObjectList.Value.Count; index++)
            {
                var i = storedGameObjectList.Value[index];

                Vector3 singleVector = i.transform.position;
       
                if (singleVector != null)
                {
                    sqrDistTest = (singleVector - positionToTest + singleVector - positionToTest1).sqrMagnitude;
                    if (sqrDistTest <= sqrDist)
                    {
                        sqrDist = sqrDistTest;
                        closestVector3 = singleVector;
                        closestIndex.Value = _index;
                    }
                }
                _index++;
            }
            distance.Value = Vector3.Distance(closestVector3.Value, agent_GO.Value.gameObject.transform.position);

            closestGameObject.Value = storedGameObjectList.Value[closestIndex.Value];



            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storedGameObjectList = null;
            distanceFrom = Vector3.zero;
            closestGameObject = null;
            closestGameObject = null;
            closestIndex = null;
            positionToTest = Vector3.zero;
            positionToTest1 = Vector3.zero;
            agent_GO = null;
            target_GO = null;
            distance = null;


        }
    }
}