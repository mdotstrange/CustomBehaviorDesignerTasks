
//Important code is from http://www.habrador.com/tutorials/linear-algebra/1-behind-or-in-front/
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Determines whether a game object is in front of or behind another. Always returns success. Stores results in bools")]
    public class IsGameObjectFrontOrBehind : Action
    {
        public SharedGameObject baseGameObject;
        public SharedGameObject targetGameObject;
        public SharedBool inFront;
        public SharedBool Behind;
       

        public override TaskStatus OnUpdate()
        {

            Vector3 youForward = baseGameObject.Value.gameObject.transform.forward;
            Vector3 youToEnemy = targetGameObject.Value.gameObject.transform.position - baseGameObject.Value.gameObject.transform.position;
            float dotProduct = DotProduct(youForward, youToEnemy);

            if (dotProduct >= 0f)
            {
                Behind.Value = false;
                inFront = true;
                return TaskStatus.Success;

            } else
            {
                Behind.Value = true;
                inFront = false;
                return TaskStatus.Success;
            }


          
        }
        float DotProduct(Vector3 vec1, Vector3 vec2)
        {
            float dotProduct = vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z;
            return dotProduct;
        }

        public override void OnReset()
        {


            
        }
    }
}