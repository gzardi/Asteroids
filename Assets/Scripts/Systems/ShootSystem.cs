using DataComponents;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [AlwaysSynchronizeSystem]
    public class ShootSystem : SystemBase
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
                .ForEach((in SpawnShotData spawnShotData, in Translation translation, in Rotation rotation, in FireKey fireKey) =>
                {
                    if (fireKey.Fire)
                    {
                        SpawnNewShot(in spawnShotData, in translation, in rotation, ecb);
                    }
                }).Run();
        }

        private static void SpawnNewShot(in SpawnShotData spawnShotData, in Translation translation, in Rotation rotation, EntityCommandBuffer ecb)
        {
            var bullet = ecb.Instantiate(spawnShotData.ShotPrefab);

            var bulletTranslation = new Translation {Value = translation.Value + math.mul(rotation.Value, spawnShotData.OffSet).xyz};

            ecb.AddComponent(bullet, bulletTranslation);

            var bulletTRotation = new Rotation {Value = rotation.Value};

            ecb.AddComponent(bullet, bulletTRotation);
        }
    }
}