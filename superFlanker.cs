using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class superFlanker : Action
{
    public SharedGameObject target;
    public SharedVector3 targetV3;
    public enum locale { Front, Back, Right, Left, Up, Down }
    public locale theLocation;
    public SharedBool random;
    public SharedFloat distanceMultiplier;
    public SharedVector3 location;
    public bool everyFrame;
    private int eValue;

    public override void OnReset()
    {
        target = null;
        targetV3 = null;
        location = null;

    }


    public override TaskStatus OnUpdate()
	{
        if (target.Value != null)
        {
            Transform targetTrans = target.Value.gameObject.transform;
            Vector3 targetPos = target.Value.gameObject.transform.position;

            if (random.Value == true)
            {
                eValue = Random.Range(0, 6);
            } else
            {
                eValue = (int)theLocation;
            }

            switch (eValue)
            {
                case 0:
                    //front
                    location.Value = targetPos + targetTrans.forward * distanceMultiplier.Value;
                    break;
                case 1:
                    //back
                    location.Value = targetPos + -targetTrans.forward * distanceMultiplier.Value;
                    break;
                case 2:
                    //right
                    location.Value = targetPos + targetTrans.right * distanceMultiplier.Value;
                    break;
                case 3:
                    //left
                    location.Value = targetPos + -targetTrans.right * distanceMultiplier.Value;
                    break;
                case 4:
                    //up
                    location.Value = targetPos + targetTrans.up * distanceMultiplier.Value;
                    break;
                case 5:
                    //down
                    location.Value = targetPos + -targetTrans.up * distanceMultiplier.Value;
                    break;
            }
            return TaskStatus.Success;
        }
        else
        {
            if (random.Value == true)
            {
                eValue = Random.Range(0, 6);
            } else
            {
                eValue = (int)theLocation;
            }

            switch (eValue)
            {
                case 0:
                    //front
                    location.Value = targetV3.Value + Vector3.forward * distanceMultiplier.Value;
                    break;
                case 1:
                    //back
                    location.Value = targetV3.Value + Vector3.back * distanceMultiplier.Value;
                    break;
                case 2:
                    //right
                    location.Value = targetV3.Value + Vector3.right * distanceMultiplier.Value;
                    break;
                case 3:
                    //left
                    location.Value = targetV3.Value + Vector3.left * distanceMultiplier.Value;
                    break;
                case 4:
                    //up
                    location.Value = targetV3.Value + Vector3.up * distanceMultiplier.Value;
                    break;
                case 5:
                    //down
                    location.Value = targetV3.Value + Vector3.down * distanceMultiplier.Value;
                    break;
            }
            return TaskStatus.Success;

        }

        
	}
}