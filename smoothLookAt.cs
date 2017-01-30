using UnityEngine;
using HutongGames.PlayMaker.Action;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform
{
    [TaskCategory("Basic/Transform")]
    [TaskDescription("Smoothly looks at a game object or position.")]
    public class smoothLookAt : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject basegameObject;
        public SharedGameObject targetObject;
        public SharedVector3 targetPosition;
        public SharedBool keepVertical;
        public SharedVector3 upVector;
        private SharedGameObject previousGo;
        private SharedQuaternion lastRotation;
        private SharedQuaternion desiredRotation;
        public SharedFloat speed;
        public SharedFloat finishTolerance;  
        [Tooltip(" if this is NOT checked the task will keep running and not return success.")]
        public SharedBool successOnFinish;
        private Transform targetTransform;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(basegameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                targetTransform = currentGameObject.GetComponent<Transform>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            var go = basegameObject.Value.gameObject;
            var goTarget = targetObject.Value;

            if (previousGo.Value != go)
            {
                lastRotation = go.transform.rotation;
                desiredRotation = lastRotation;
                previousGo = go;
            }


            // desired look at position

            Vector3 lookAtPos;
            if (goTarget != null)
            {
                lookAtPos = !targetPosition.IsNone ?
                    goTarget.transform.TransformPoint(targetPosition.Value) :
                    goTarget.transform.position;
            } else
            {
                lookAtPos = targetPosition.Value;
            }

            if (keepVertical.Value)
            {
                lookAtPos.y = go.transform.position.y;
            }

            // smooth look at

            var diff = lookAtPos - go.transform.position;
            if (diff != Vector3.zero && diff.sqrMagnitude > 0)
            {
                desiredRotation = Quaternion.LookRotation(diff, upVector.IsNone ? Vector3.up : upVector.Value);
            }

            lastRotation = Quaternion.Slerp(lastRotation.Value, desiredRotation.Value, speed.Value * Time.deltaTime);
            go.transform.rotation = lastRotation.Value;

            // send finish event?

            if (successOnFinish.Value == true)
            {
                var targetDir = lookAtPos - go.transform.position;
                var angle = Vector3.Angle(targetDir, go.transform.forward);

                if (Mathf.Abs(angle) <= finishTolerance.Value)
                {

                    return TaskStatus.Success;
                }

            }
            return TaskStatus.Running;

        }

        public override void OnReset()
        {
            basegameObject = null;        
        
            speed = 5;
            finishTolerance = 1;
        }

       
    }
}