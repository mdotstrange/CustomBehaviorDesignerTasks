//by MDS
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Checks if a game object is above or below another using the threshold value. Returns success if true. Stores bool if is above.")]
    public class isGameObjectAboveOrBelow : Action
    {
        public SharedGameObject self;
        public SharedGameObject targetGO;
        public SharedFloat threshhold;
        public SharedBool isAbove;
        public SharedBool isBelow;
        public SharedFloat heightDifference;

        public override TaskStatus OnUpdate()
        {

            float yDiff = self.Value.transform.position.y - targetGO.Value.gameObject.transform.position.y;
            float AbsDiff = Mathf.Abs(yDiff);
            heightDifference.Value = AbsDiff;
         
            if(AbsDiff <= threshhold.Value)
            {
                isAbove.Value = false;
                isBelow.Value = false;         
                return TaskStatus.Failure;

            }
            else if(yDiff < 0f)
            {
                isAbove.Value = true;
                isBelow.Value = false;
                return TaskStatus.Success;
            }
          else if(yDiff > 0f)
            {
                isAbove.Value = false;
                isBelow.Value = true;
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;

        }

        public override void OnReset()
        {

            threshhold = null;
            isAbove = null;
            
        }
    }
}
