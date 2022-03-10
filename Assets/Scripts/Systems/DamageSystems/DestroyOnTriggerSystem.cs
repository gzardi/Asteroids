using Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems.DamageSystems
{
    public class DestroyOnTriggerSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem ecbSystem;
        private BuildPhysicsWorld buildPhysicsWorld;
        private StepPhysicsWorld stepPhysicsWorld;

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var destroyAsteroidOnContactWithPlayerGroup = GetComponentDataFromEntity<AsteroidTag>(true);
            
            var PlayerGroup = GetComponentDataFromEntity<PlayerTag>(true);
            
            var ecb = ecbSystem.CreateCommandBuffer();

            var destroyTriggerJob = new DestroyOnTriggerWithPlayerJob
            {
                Ecb = ecb,
                destroyAsteroidOnContactWithPlayerGroup = destroyAsteroidOnContactWithPlayerGroup,
                playerGroup = PlayerGroup
            };
            

            destroyTriggerJob.Schedule(stepPhysicsWorld.Simulation,ref buildPhysicsWorld.PhysicsWorld, inputDeps).Complete();

            return inputDeps;

        }
        
        private struct DestroyOnTriggerWithPlayerJob : ITriggerEventsJob
        {
            public EntityCommandBuffer Ecb;
            [ReadOnly] public ComponentDataFromEntity<AsteroidTag> destroyAsteroidOnContactWithPlayerGroup;
            [ReadOnly] public ComponentDataFromEntity<PlayerTag> playerGroup;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (destroyAsteroidOnContactWithPlayerGroup.HasComponent(triggerEvent.EntityA) && playerGroup.HasComponent(triggerEvent.EntityB))
                {
                    Ecb.DestroyEntity(triggerEvent.EntityA);
                }
                if (destroyAsteroidOnContactWithPlayerGroup.HasComponent(triggerEvent.EntityB) && playerGroup.HasComponent(triggerEvent.EntityA))
                {
                    Ecb.DestroyEntity(triggerEvent.EntityB);
                }
            }
        }
        
        
    }
}