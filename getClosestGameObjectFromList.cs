using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Gets the closest game object in a list.")]
    public class GetClosestGameObjectFromList : Action
    {

        [RequiredField]
        [Tooltip("The SharedVector3List to use")]
        public SharedGameObjectList storedGameObjectList;
        public SharedGameObject distanceFromGO;
        public SharedVector3 distanceFrom;
        public SharedGameObject closestGameObject;
        public SharedInt closestIndex;
        private Vector3 positionToTest;
        public SharedFloat distance;
        private SharedVector3 closestVector3;


        public override void OnAwake()
        {

        }

        public override TaskStatus OnUpdate()
        {
            //Debug.Log("DistanceFromGO value " + distanceFromGO.Value);
            if (distanceFromGO.Value != null)
            {
                positionToTest = distanceFromGO.Value.gameObject.transform.position;

            } else
            {
                positionToTest = distanceFrom.Value;

            }



            float sqrDist = Mathf.Infinity;
            int _index = 0;
            float sqrDistTest;
            //foreach (GameObject singleGO in storedGameObjectList.Value)
            for(int index = 0; index < storedGameObjectList.Value.Count; index++)
            {
                var singleGO = storedGameObjectList.Value[index];
                Vector3 singleVector = singleGO.transform.position;

                if (singleVector != null)
                {
                    sqrDistTest = (singleVector - positionToTest).sqrMagnitude;
                    if (sqrDistTest <= sqrDist)
                    {
                        sqrDist = sqrDistTest;
                        closestVector3 = singleVector;
                        closestIndex.Value = _index;
                    }
                }
                _index++;
            }
            distance.Value = Vector3.Distance(positionToTest, closestVector3.Value);

            closestGameObject.Value = storedGameObjectList.Value[closestIndex.Value];



            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storedGameObjectList = null;
            distanceFrom = Vector3.zero;
            closestGameObject = null;
            closestIndex = null;
            positionToTest = Vector3.zero;
            distanceFromGO = null;
            distance = null;


        }
    }
}