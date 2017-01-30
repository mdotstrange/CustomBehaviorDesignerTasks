using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FloatRemapTask : Action
{
   
    public SharedFloat theFloat;
    public SharedFloat baseStart;    
    public SharedFloat baseEnd;    
    public SharedFloat targetStart;
    public SharedFloat targetEnd;
    public SharedFloat storeResult;


    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {

        storeResult.Value = map(theFloat.Value, baseStart.Value, baseEnd.Value, targetStart.Value, targetEnd.Value);



        return TaskStatus.Success;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

}