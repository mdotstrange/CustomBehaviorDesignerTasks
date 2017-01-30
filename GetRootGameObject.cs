using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Gets the root game object of a game object var.")]
    public class GetRootGameObject : Action
    {
        [Tooltip("The GameObject to get the root of.")]
        public SharedGameObject baseGameObject;
        [Tooltip("The root game object")]    
        public SharedGameObject rootGameObject;

        public override TaskStatus OnUpdate()
        {

            rootGameObject.Value = baseGameObject.Value.gameObject.transform.root.gameObject;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            baseGameObject = null;
            rootGameObject = null;
        }
    }
}