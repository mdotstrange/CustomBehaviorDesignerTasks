using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SampleCurvePlus : Action
{
    public AnimationCurve curve;
    public SharedFloat input;
    public SharedFloat inputMax;

    float input1;
    public SharedFloat output;

	public override void OnStart()
	{
		
	}

	public override TaskStatus OnUpdate()
	{

        input1 = input.Value / inputMax.Value;

        output.Value = Mathf.Clamp(curve.Evaluate(input1), 0, 1);


        return TaskStatus.Success;
	}
}