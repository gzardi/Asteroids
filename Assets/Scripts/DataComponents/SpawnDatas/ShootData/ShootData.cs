using Unity.Entities;

namespace DataComponents
{
    [GenerateAuthoringComponent]
    public struct ShotData : IComponentData
    {
        public float Velocity;
        public float Lifetime;
    }
}