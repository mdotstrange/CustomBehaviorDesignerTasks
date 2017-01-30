using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SampleCurve : Action
{
    public AnimationCurve curve;
    public SharedFloat input;
    public SharedFloat output;

	public override void OnStart()
	{
		
	}

	public override TaskStatus OnUpdate()
	{
        output.Value = Mathf.Clamp(curve.Evaluate(input.Value), 0, 1);


        return TaskStatus.Success;
	}
}