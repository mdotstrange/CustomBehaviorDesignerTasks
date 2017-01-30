using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FloatAddTask : Action
{
    public SharedFloat float01;
    public SharedFloat float02;


    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {
        float01.Value = float01.Value + float02.Value;


        return TaskStatus.Success;
    }
}