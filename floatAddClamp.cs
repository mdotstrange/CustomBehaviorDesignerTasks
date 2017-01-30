using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Adds to a float and clamps it")]
    public class floatAddClamp : Action
    {
    
        public SharedFloat valueToAdd;
    
        public SharedFloat floatValue;

        public SharedFloat clampHi;
        public SharedFloat clampLo;

        public override TaskStatus OnUpdate()
        {
            floatValue.Value = Mathf.Clamp(floatValue.Value + valueToAdd.Value, clampLo.Value, clampHi.Value);


            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            floatValue.Value = 0;
            valueToAdd.Value = 0;
        }
    }
}