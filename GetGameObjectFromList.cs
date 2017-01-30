using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetGameObjectFromList : Action
{
    public SharedGameObjectList theList;
    public SharedInt index;
    public SharedGameObject returnedObject;


	public override void OnStart()
	{
		
	}

	public override TaskStatus OnUpdate()
	{
        if(theList.Value.Count != 0)
        {
            returnedObject.Value = theList.Value[index.Value];
            return TaskStatus.Success;

        }
        else
        {
            return TaskStatus.Success;
        }



	
	}
    public override void OnReset()
    {
        theList = null;
        index = null;
        returnedObject = null;
    }
}