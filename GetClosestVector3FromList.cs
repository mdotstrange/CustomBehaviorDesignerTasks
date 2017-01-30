using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Gets the closest vector3 from another vector3 in a list.")]
    public class GetClosestVector3FromList : Action
    {

        [RequiredField]
        [Tooltip("The SharedVector3List to use")]
        public SharedVector3List storedVector3List;
        public SharedGameObject distanceFromGO;
        public SharedVector3 distanceFrom;
        public SharedVector3 closestVector3;
        public SharedInt closestIndex;
        private Vector3 positionToTest;
        public SharedFloat distance;


        public override void OnAwake()
        {

        }

        public override TaskStatus OnUpdate()
        {
            //Debug.Log("DistanceFromGO value " + distanceFromGO.Value);
            if (distanceFromGO.Value != null)
            {
                positionToTest = distanceFromGO.Value.gameObject.transform.position;

            } 
            else
            {
                positionToTest = distanceFrom.Value;

            }

           

            float sqrDist = Mathf.Infinity;
            int _index = 0;
            float sqrDistTest;
            //foreach (Vector3 singleVector in storedVector3List.Value)
            for(int index = 0; index < storedVector3List.Value.Count; index++)
            {
                var singleVector = storedVector3List.Value[index];

                if (singleVector != null)
                {
                    sqrDistTest = (singleVector - positionToTest).sqrMagnitude;
                    if (sqrDistTest <= sqrDist)
                    {
                        sqrDist = sqrDistTest;
                        closestVector3.Value = singleVector;
                        closestIndex.Value = _index;
                    }
                }
                _index++;
            }
            distance.Value = Vector3.Distance(positionToTest, closestVector3.Value);



            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storedVector3List = null;
            distanceFrom = new Vector3(0, 0, 0);
            closestVector3 = new Vector3(0, 0, 0);
            closestIndex = null;
            positionToTest = new Vector3(0, 0, 0);
            distanceFromGO = null;
            distance = null;


        }
    }
}