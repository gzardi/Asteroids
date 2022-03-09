using Unity.Entities;
using UnityEngine;

namespace DataComponents
{
    
    [GenerateAuthoringComponent]
    public struct MovementKeys : IComponentData
    {
        public KeyCode Forward;
        
        public KeyCode Backward;
        
        public KeyCode Left;
        
        public KeyCode Right;

        public KeyCode SpawnKey;


        public bool GoForward => Input.GetKey(Forward);

        public bool GoBackward => Input.GetKey(Backward);

        public bool GoLeft => Input.GetKey(Left);

        public bool GoRight => Input.GetKey(Right);
        public bool SpawnPlayer => Input.GetKeyDown(SpawnKey);
    }
}