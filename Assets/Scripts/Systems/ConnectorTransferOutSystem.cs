using System;
using Unity.Entities;
using UnityEngine;

namespace SupplyChain
{
    [UpdateBefore(typeof(ProcessSystem))]
    public class ConnectorTransferOutSystem : ComponentSystem
    {
        public struct SourceData
        {
            public readonly int Length;
            public EntityArray Entity;
            public GameObjectArray GameObject;
            public ComponentArray<ProcessComponent> Process;
            public ComponentArray<ConnectorOutComponent> ConnectorOut;
        }

        public struct TargetData
        {
            public readonly int Length;
            public ComponentArray<ProcessComponent> Process;
            public ComponentArray<ConnectorInComponent> ConnectorIn;
        }

        [Inject] private SourceData sourceData;
        [Inject] private TargetData targetData;

        protected override void OnUpdate()
        {
            for (int i = 0; i < sourceData.Length; i++)
            {
                if (sourceData.Process[i].State != ProcessComponent.ProcessState.Complete)
                {
                    continue;
                }

                int otherConnectorGuid = sourceData.ConnectorOut[i].OtherConnectorGuid;
                if (otherConnectorGuid <= 0)
                {
                    UnityEngine.Object.Destroy(sourceData.GameObject[i].GetComponent<ConnectorOutComponent>());
                    continue;
                }
                int targetIndex = GetTargetIndex(targetData, otherConnectorGuid);
                if (targetIndex < 0)
                {
                    sourceData.ConnectorOut[i].OtherConnectorGuid = 0;
                    UnityEngine.Object.Destroy(sourceData.GameObject[i].GetComponent<ConnectorOutComponent>());
                    continue;
                }

                bool allAdded = false;
                Item.Stack[] outBuffer = sourceData.Process[i].ItemBufferOut;
                Item.Stack[] itemBuffer = Inventory.AddAll(targetData.Process[targetIndex].ItemBufferIn, outBuffer, out allAdded);
                if (!allAdded)
                {
                    continue; // items blocked
                }
                sourceData.Process[i].ItemBufferOut = new Item.Stack[sourceData.Process[i].ItemBufferOut.Length];
                targetData.Process[targetIndex].ItemBufferIn = itemBuffer;
                //Debug.Log(fromData.GameObject[i].name + " transfered out " + string.Join(", ", outBuffer) + " to " + toData.ConnectorIn[j].gameObject.name);
            }
        }

        private int GetTargetIndex(TargetData targetData, int guid)
        {
            for (int j = 0; j < targetData.Length; j++)
            {
                int thisGuid = targetData.ConnectorIn[j].Guid;
                if (thisGuid == guid)
                {
                    return j;
                }
            }
            return -1;
        }
    }
}
