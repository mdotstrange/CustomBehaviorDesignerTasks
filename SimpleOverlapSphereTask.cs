using UnityEngine;
using HutongGames.PlayMaker.Actions;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPhysics
{
    [TaskCategory("Basic/Physics")]
    [TaskDescription("Uses an overlap sphere to check for collision, failure if no collision otherwise success-only returns first object hit.")]
    public class SimpleOverlapSphereTask : Action
    {
        [Tooltip("Sphere origin.")]
        public SharedGameObject scanOrigin;

        [Tooltip("Sphere origin v3.")]
        public SharedVector3 scanOriginV3;

        [Tooltip("The radius of the shpere.")]
        public SharedFloat scanRange;
     
        [Tooltip("Set to true to ignore colliders set to trigger.")]
        public SharedBool ignoreTriggerColliders;
    

        [Tooltip("Pick only from these layers.")]
        public LayerMask layerMask;
        public SharedGameObject hitObject;
      


        public override TaskStatus OnUpdate()
        {
           

            if (scanOrigin.Value != null)
            {
                scanOriginV3.Value = scanOrigin.Value.transform.position;
            }


            if (ignoreTriggerColliders.Value == true)
            {
                Collider[] colliders = Physics.OverlapSphere(scanOriginV3.Value, scanRange.Value, layerMask, QueryTriggerInteraction.Ignore);
                if (colliders.Length == 0)
                {
                    return TaskStatus.Failure;
                } else
                {
                    hitObject.Value = colliders[0].gameObject;
                    return TaskStatus.Success;
                }

            } else
            {
                Collider[] colliders = Physics.OverlapSphere(scanOriginV3.Value, scanRange.Value, layerMask, QueryTriggerInteraction.Collide);
                if (colliders.Length == 0)
                {
                    return TaskStatus.Failure;
                } 
                else
                {
                    var list = new List<Collider>(colliders);

                    for (int index = 0; index < list.Count; index++)
                    {
                        var i = list[index].gameObject;
                        if (i == scanOrigin.Value)
                        {
                            list.RemoveAt(index);
                        }

                    }

                    hitObject.Value = list[0].gameObject;
                    return TaskStatus.Success;
                }

            }



        }

        public override void OnReset()
        {
            scanOriginV3 = Vector3.zero;
            hitObject = null;
            scanOrigin = null;

        }


    }
}