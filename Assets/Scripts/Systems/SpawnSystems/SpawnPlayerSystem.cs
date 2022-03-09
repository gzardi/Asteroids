using DataComponents;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [AlwaysSynchronizeSystem]
    public class SpawnPlayerSystem : SystemBase
    {
        private BeginInitializationEntityCommandBufferSystem _ecbSystem;

        protected override void OnStartRunning()
        {
            _ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer();


            Entities
                .ForEach((in SpawnPlayerData playerData, in MovementKeys keys) =>
                {
                    if (keys.SpawnPlayer)
                    {
                        SpawnNewShot(in playerData, ecb);
                    }
                }).Run();
        }

        private static void SpawnNewShot(in SpawnPlayerData playerData, EntityCommandBuffer ecb)
        {
            var Player = ecb.Instantiate(playerData.ShotPrefab);

        }
    }
}