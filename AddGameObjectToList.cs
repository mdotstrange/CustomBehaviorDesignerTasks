using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Adds a GameObject to a list")]
    public class AddGameObjectToList : Action
    {
        [Tooltip("The Game Object value")]
        public SharedGameObject[] gameObjectToAdd;
        [RequiredField]
        [Tooltip("The SharedGameObjectList to set")]
        public SharedGameObjectList storedGameObjectList;

        public override void OnAwake()
        {
           
        }

        public override TaskStatus OnUpdate()
        {
            if (gameObjectToAdd == null || gameObjectToAdd.Length == 0)
            {
                return TaskStatus.Failure;
            }

            for (int i = 0; i < gameObjectToAdd.Length; ++i)
            {
                storedGameObjectList.Value.Add(gameObjectToAdd[i].Value);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            gameObjectToAdd = null;
            storedGameObjectList = null;
        }
    }
}