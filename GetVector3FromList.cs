using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Removes a vector3 to a vector3 list at an index.")]
    public class GetVector3FromList : Action
    {

        [RequiredField]
        [Tooltip("The SharedVector3List to set")]
        public SharedVector3List storedVector3List;
        public SharedInt index;
        public SharedVector3 thePosition;
        private Vector3 singleVector;

        public override void OnAwake()
        {
            //   storedVector3List.Value = new List<Vector3>();
        }

        public override TaskStatus OnUpdate()
        {

            singleVector = storedVector3List.Value[index.Value];

            thePosition.Value = singleVector;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            index = 0;
            storedVector3List = null;
            thePosition = new Vector3(0, 0, 0);
            singleVector = new Vector3(0, 0, 0);
        }
    }
}