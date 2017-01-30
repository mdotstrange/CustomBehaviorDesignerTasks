using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime.Tasks.Custom

{
    [TaskDescription("Gets a game object var on another tree defined by sharedGameObject")]
    public class GetGameObjectOtherTree : Action
    {
        public SharedGameObject sharedGameObject;
        public SharedInt group;
        public string variableName;
        public SharedGameObject targetVariable;
        private Behavior behavior;

        public override void OnStart()
        {
            var behaviorTrees = GetDefaultGameObject(sharedGameObject.Value).GetComponents<Behavior>();
            if (behaviorTrees.Length == 1)
            {
                behavior = behaviorTrees[0];
            } else if (behaviorTrees.Length > 1)
            {
                for (int i = 0; i < behaviorTrees.Length; ++i)
                {
                    if (behaviorTrees[i].Group == group.Value)
                    {
                        behavior = behaviorTrees[i];
                        break;
                    }
                }
            }
            // If the group can't be found then use the first behavior tree
            if (behavior == null)
            {
                behavior = behaviorTrees[0];
            }


        }

        public override TaskStatus OnUpdate()
        {
            if (behavior == null)
            {
                return TaskStatus.Failure;
            }
          

            targetVariable.Value = (behavior.GetVariable(variableName) as SharedGameObject).Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
       
            variableName = "";
            targetVariable = null;
        }
    }
}