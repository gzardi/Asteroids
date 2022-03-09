using Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems.DamageSystems
{
    public class DestroyOnCollisionWithPlayerSystem : JobComponentSystem
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
            var destroyOnContactGroup = GetComponentDataFromEntity<DestroyOnContactTag>(true);
            
            var PlayerGroup = GetComponentDataFromEntity<PlayerTag>(true);
            
            var ecb = ecbSystem.CreateCommandBuffer();

            var destroyTriggerJob = new DestroyTriggerJob
            {
                Ecb = ecb,
                destroyOnContactGroup = destroyOnContactGroup,
                playerGroup = PlayerGroup
            };
            
            var destroyCollisionJob = new DestroyCollisionJob
            {
                Ecb = ecb,
                DestroyOnContactGroup = destroyOnContactGroup,
                playerGroup = PlayerGroup
                
            };

            destroyTriggerJob.Schedule(stepPhysicsWorld.Simulation,ref buildPhysicsWorld.PhysicsWorld, inputDeps).Complete();

            destroyCollisionJob.Schedule(stepPhysicsWorld.Simulation,ref buildPhysicsWorld.PhysicsWorld, inputDeps).Complete();

            return inputDeps;

        }
        
        private struct DestroyTriggerJob : ITriggerEventsJob
        {
            public EntityCommandBuffer Ecb;
            [ReadOnly] public ComponentDataFromEntity<DestroyOnContactTag> destroyOnContactGroup;
            [ReadOnly] public ComponentDataFromEntity<PlayerTag> playerGroup;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (destroyOnContactGroup.HasComponent(triggerEvent.EntityA) && playerGroup.HasComponent(triggerEvent.EntityB))
                {
                    Ecb.DestroyEntity(triggerEvent.EntityA);
                }
                if (destroyOnContactGroup.HasComponent(triggerEvent.EntityB) && playerGroup.HasComponent(triggerEvent.EntityA))
                {
                    Ecb.DestroyEntity(triggerEvent.EntityB);
                }
            }
        }
        
        private struct DestroyCollisionJob : ICollisionEventsJob
        {
            public EntityCommandBuffer Ecb;
            [ReadOnly] public ComponentDataFromEntity<DestroyOnContactTag> DestroyOnContactGroup;
            [ReadOnly] public ComponentDataFromEntity<PlayerTag> playerGroup;

            public void Execute(CollisionEvent collisionEvent)
            {
                if (DestroyOnContactGroup.HasComponent(collisionEvent.EntityA) && playerGroup.HasComponent(collisionEvent.EntityB))
                {
                    Ecb.DestroyEntity(collisionEvent.EntityA);
                }
                if (DestroyOnContactGroup.HasComponent(collisionEvent.EntityB) && playerGroup.HasComponent(collisionEvent.EntityA))
                {
                    Ecb.DestroyEntity(collisionEvent.EntityB);
                }
            }
        }
        
        
    }
}