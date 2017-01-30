using UnityEngine;
using XftWeapon;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    [TaskCategory("XWeaponTrail")]
    [TaskDescription("Enables/Disables XWeaponTrail effect")]
    public class EnableXTrailTask : Action
    {
        private GameObject trailOwner;
        public SharedCollider trailOwnerColl;
        public SharedBool enable;
        public SharedBool stopSmoothly;
        public SharedFloat fadeTime;
        XWeaponTrail trail;

        public override TaskStatus OnUpdate()
        {
           if(trailOwnerColl.Value.gameObject.transform.childCount == 0)
            {
                return TaskStatus.Success;
            }

            if (trailOwner == null)
            {
                //Debug.Log("No owner");
                trailOwner = trailOwnerColl.Value.gameObject.transform.GetChild(0).gameObject;
            
            }
            //Debug.Log(" trail owner is " + trailOwner.Value);
            if (trailOwner.gameObject.GetComponent<XWeaponTrail>() == null)
            {
                //Debug.Log("No script");
               return TaskStatus.Success;
            }

           

            trail = trailOwner.gameObject.GetComponent<XWeaponTrail>();

            if (enable.Value == true)
            {
                //Debug.Log("Trail activate");
                trail.Activate();
                return TaskStatus.Success;
            } 
            else
            {
                if (stopSmoothly.Value == true)
                {
                    //Debug.Log("Stop smooth");
                    trail.StopSmoothly(fadeTime.Value);
                    return TaskStatus.Success;
                } 
                else
                {
                    //Debug.Log("Deactivate");
                    trail.Deactivate();
                    return TaskStatus.Success;
                }
            }

           

        }

        public override void OnReset()
        {
            trailOwner = null;
            enable = null;
            stopSmoothly = null;
            fadeTime = null;
        }
    }
}