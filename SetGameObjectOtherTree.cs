using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime.Tasks.Custom

{
    [TaskDescription("Sets a game object var on another tree defined by sharedGameObject")]
    public class SetGameObjectOtherTree : Action
    {
        public SharedGameObject sharedGameObject;
        public string variableName;
        public SharedGameObject targetVariable;
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

             (behaviorTree.GetVariable(variableName) as SharedGameObject).Value = targetVariable.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            behaviorTree = null;
            variableName = "";
            targetVariable = null;
        }
    }
}