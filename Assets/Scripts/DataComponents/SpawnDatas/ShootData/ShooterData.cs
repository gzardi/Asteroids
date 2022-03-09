using Unity.Entities;
using Unity.Mathematics;

namespace DataComponents
{
    [GenerateAuthoringComponent]
    public struct SpawnShotData : IComponentData
    {
        public Entity ShotPrefab;
        public float3 OffSet;
    }
}