using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime.Tasks.Custom

{
    [TaskDescription("Gets a bool var on another tree defined by sharedGameObject")]
    public class getBoolOnOtherTree : Action
    {
        public SharedGameObject sharedGameObject;
        public string variableName;
        public SharedBool targetVariable;
        private BehaviorTree behaviorTree;

        public override void OnStart()
        {
            behaviorTree = sharedGameObject.Value.GetComponent<BehaviorTree>();
        }

        public override TaskStatus OnUpdate()
        {
            if (behaviorTree == null)
            {
                return TaskStatus.Failure;
            }       

            targetVariable.Value = (behaviorTree.GetVariable(variableName) as SharedBool).Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            behaviorTree = null;
            variableName = "";
            targetVariable = false;
        }
    }
}