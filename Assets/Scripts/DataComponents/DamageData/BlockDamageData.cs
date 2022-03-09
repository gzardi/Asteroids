using Unity.Entities;

namespace DataComponents
{
    [GenerateAuthoringComponent]
    public struct BlockDamageData : IComponentData
    {
        public int Value;
    }
}