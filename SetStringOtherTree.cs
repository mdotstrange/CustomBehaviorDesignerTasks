using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime.Tasks.Custom

{
    [TaskDescription("Sets a string variable on another tree thats on the SharedGameObject")]
    public class SetStringOtherTree : Action
    {
        public SharedGameObject sharedGameObject;
        public string variableName;
        public SharedString targetVariable;
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

             (behaviorTree.GetVariable(variableName) as SharedString).Value = targetVariable.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            behaviorTree = null;
            variableName = "";
            targetVariable = "";
        }
    }
}