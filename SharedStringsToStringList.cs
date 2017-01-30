using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Sets the SharedStringList values from the Strings. Returns Success.")]
	public class SharedStringsToStringList : Action
	{
		[Tooltip("The Strings value")]
		public SharedString[] strings;
		[RequiredField]
		[Tooltip("The SharedStringList to set")]
		public SharedStringList storedStringList;
		
		public override void OnAwake()
		{
			storedStringList.Value = new List<string>();
		}
		
		public override TaskStatus OnUpdate()
		{
			if (strings == null || strings.Length == 0) {
				return TaskStatus.Failure;
			}
			
			storedStringList.Value.Clear();
			for (int i = 0; i < strings.Length; ++i) {
				storedStringList.Value.Add(strings[i].Value);
			}
			
			return TaskStatus.Success;
		}
		
		public override void OnReset()
		{
			strings = null;
			storedStringList = null;
		}
	}
}