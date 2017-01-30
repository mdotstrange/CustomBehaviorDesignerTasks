using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ActivateGameObjectTask : Action
{
    public SharedGameObject targetGo;
    public SharedBool activate;



    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {
        if (targetGo.Value != null)
        {
            targetGo.Value.SetActive(activate.Value);
        }



        return TaskStatus.Success;
    }

 
}