using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Rotates towards the specified rotation. The rotation can either be specified by a transform or rotation. If the transform " +
                     "is used then the rotation will not be used.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=2")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}RotateTowardsIcon.png")]
    public class RotateTowardsObject : Action
    {

        public SharedGameObject objectToRotate;
        [Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
        [Tooltip("The agent is done rotating when the angle is less than this value")]
        public SharedFloat rotationEpsilon = 0.5f;
        [Tooltip("The maximum number of angles the agent can rotate in a single tick")]
        public SharedFloat maxLookAtRotationDelta = 1;
        [Tooltip("Should the rotation only affect the Y axis?")]
        public SharedBool onlyY;
        [Tooltip("The GameObject that the agent is rotating towards")]
        public SharedGameObject target;
        [Tooltip("If target is null then use the target rotation")]
        public SharedVector3 targetRotation;
        Transform objecTrans;

        public override TaskStatus OnUpdate()
        {
           objecTrans = objectToRotate.Value.gameObject.transform;

            var rotation = Target();
            // Return a task status of success once we are done rotating
            if (Quaternion.Angle(objecTrans.rotation, rotation) < rotationEpsilon.Value)
            {
                return TaskStatus.Success;
            }
            // We haven't reached the target yet so keep rotating towards it
            objecTrans.rotation = Quaternion.RotateTowards(objecTrans.rotation, rotation, maxLookAtRotationDelta.Value);
            return TaskStatus.Running;
        }

        // Return targetPosition if targetTransform is null
        private Quaternion Target()
        {
            if (target == null || target.Value == null)
            {
                return Quaternion.Euler(targetRotation.Value);
            }
            var position = target.Value.transform.position - objecTrans.position;
            if (onlyY.Value)
            {
                position.y = 0;
            }
            if (usePhysics2D)
            {
                var angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
                return Quaternion.AngleAxis(angle, Vector3.forward);
            }
            return Quaternion.LookRotation(position);
        }

        // Reset the public variables
        public override void OnReset()
        {
            usePhysics2D = false;
            rotationEpsilon = 0.5f;
            maxLookAtRotationDelta = 1f;
            onlyY = false;
            target = null;
            targetRotation = Vector3.zero;
        }
    }
}