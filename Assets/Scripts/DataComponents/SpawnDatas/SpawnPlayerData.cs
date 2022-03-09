using Unity.Entities;
using Unity.Mathematics;

namespace DataComponents
{
    [GenerateAuthoringComponent]
    public struct SpawnPlayerData : IComponentData
    {
        public Entity ShotPrefab;
    }
}