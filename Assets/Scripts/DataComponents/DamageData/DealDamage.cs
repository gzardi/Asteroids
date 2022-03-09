using Unity.Entities;

namespace DataComponents
{
    [GenerateAuthoringComponent]
    public struct DealDamage : IComponentData
    {
        public int Value;
    }
}