using Unity.Entities;
using UnityEngine;

namespace DataComponents
{
    [GenerateAuthoringComponent]
    public struct FireKey : IComponentData
    {
        public KeyCode Value;

        public bool Fire => Input.GetKeyDown(Value);
    }
}