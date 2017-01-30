using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.UnityPhysics
{
    [TaskCategory("Basic/Physics")]
    [TaskDescription("Casts overlap Sphere and adds hit game objects to a game object list.")]

    public class overlapSphereToList : Action
    {
        [Tooltip("The origin of the sphere")]
        public SharedGameObject scanOrigin;
        [Tooltip("The radius of the sphere")]
        public SharedFloat scanRange;
        [Tooltip("The list to store hit game objects in.")]
        public SharedGameObjectList listToStoreHitObject;
        public LayerMask objectLayerMask = -1;
        public SharedBool ignoreTriggerColliders;





        public override TaskStatus OnUpdate()
        {
            listToStoreHitObject.Value.Clear();

            float range = scanRange.Value;

            if (ignoreTriggerColliders.Value == true)
            {
                Collider[] colliders = Physics.OverlapSphere(scanOrigin.Value.gameObject.transform.position, range, objectLayerMask, QueryTriggerInteraction.Ignore);
                if (colliders.Length == 0)
                {
                    return TaskStatus.Failure;
                }


                //foreach (Collider col in colliders)
                for(int index = 0; index < colliders.Length; index++)
                {
                    var col = colliders[index];
                    listToStoreHitObject.Value.Add(col.gameObject);
                }
            } 
            else
            {
                Collider[] colliders = Physics.OverlapSphere(scanOrigin.Value.gameObject.transform.position, range, objectLayerMask, QueryTriggerInteraction.Collide);
                if (colliders.Length == 0)
                {
                    return TaskStatus.Failure;
                }


                for (int index = 0; index < colliders.Length; index++)
                {
                    var col = colliders[index];
                    listToStoreHitObject.Value.Add(col.gameObject);
                }
            }




            return TaskStatus.Success;   

        }

        public override void OnReset()
        {
            scanOrigin = null;
            scanRange = null;
            objectLayerMask = -1;
            listToStoreHitObject = null;
            




        }
    }
}