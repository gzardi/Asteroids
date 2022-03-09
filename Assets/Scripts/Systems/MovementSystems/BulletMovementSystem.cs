using DataComponents;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    
   [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public class BulletMovementSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _ecbSystem;

        protected override void OnStartRunning()
        {
            _ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer();
            var deltaTime = Time.DeltaTime;
            
            Entities.ForEach((Entity e, ref Translation translation, ref ShotData shotData, in LocalToWorld localToWorld) =>
            {
                MoveBullet(ref translation,in shotData,in localToWorld,deltaTime);

                DecreaceLifeTime(ref shotData, deltaTime);
                
                if (IsDead(ref shotData))
                {
                    ecb.DestroyEntity(e);
                }
            }).Run();
        }

        private static bool IsDead(ref ShotData shotData)
        {
            return shotData.Lifetime <= 0f;
        }

        private static void DecreaceLifeTime(ref ShotData shotData, float deltaTime)
        {
            shotData.Lifetime -= deltaTime;
        }

        private static void MoveBullet(ref Translation translation, in ShotData shotData, in LocalToWorld localToWorld, float deltaTime)
        {
            var forwardMovement = localToWorld.Forward * deltaTime * shotData.Velocity;

            translation.Value += forwardMovement;
        }
    }
}