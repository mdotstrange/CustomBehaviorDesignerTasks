// Converted Playmaker action "Move Towards"
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Move a game object to another game object's position. Return success when it reaches the finish distance.")]
    public class MoveGameObjectTowards : Action
    {     
        public SharedGameObject ObjectToMove;
        public SharedGameObject targetObject;   
        public SharedVector3 targetPosition;
        public SharedBool ignoreVertical; 
        public SharedFloat maxSpeed;   
        public SharedFloat finishDistance;
        private GameObject go;
        private GameObject goTarget;
        private Vector3 targetPos;
        private Vector3 targetPosWithVertical;
        public override TaskStatus OnUpdate()
        {
            if (!UpdateTargetPos())
            {
                return TaskStatus.Failure;
            }

            go.transform.position = Vector3.MoveTowards(go.transform.position, targetPos, maxSpeed.Value * Time.deltaTime);

            var distance = (go.transform.position - targetPos).magnitude;
            if (distance < finishDistance.Value)
            {

                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }


        public bool UpdateTargetPos()
        {
            go = ObjectToMove.Value;
            if (go == null)
            {
                return false;
            }

            goTarget = targetObject.Value;
            if (goTarget == null && targetPosition.IsNone)
            {
                return false;
            }

            if (goTarget != null)
            {
                targetPos = !targetPosition.IsNone ?
                    goTarget.transform.TransformPoint(targetPosition.Value) :
                    goTarget.transform.position;
            } else
            {
                targetPos = targetPosition.Value;
            }

            targetPosWithVertical = targetPos;

            if (ignoreVertical.Value)
            {
                targetPos.y = go.transform.position.y;
            }

            return true;
        }

        public Vector3 GetTargetPos()
        {
            return targetPos;
        }

        public Vector3 GetTargetPosWithVertical()
        {
            return targetPosWithVertical;
        } 

        public override void OnReset()
        {
            gameObject = null;
            targetObject = null;
            maxSpeed = 10f;
            finishDistance = 1f;
          

        }
    }
}