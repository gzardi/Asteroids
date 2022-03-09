using Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems.DamageSystems
{
    public class DestroyOnContactSystem : JobComponentSystem
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
            var ecb = ecbSystem.CreateCommandBuffer();

            var destroyTriggerJob = new DestroyTriggerJob
            {
                Ecb = ecb,
                destroyOnContactGroup = destroyOnContactGroup
                
            };
            
            var destroyCollisionJob = new DestroyCollisionJob
            {
                Ecb = ecb,
                DestroyOnContactGroup = destroyOnContactGroup
                
            };

            destroyTriggerJob.Schedule(stepPhysicsWorld.Simulation,ref buildPhysicsWorld.PhysicsWorld, inputDeps).Complete();

            destroyCollisionJob.Schedule(stepPhysicsWorld.Simulation,ref buildPhysicsWorld.PhysicsWorld, inputDeps).Complete();

            return inputDeps;

        }
        
        private struct DestroyTriggerJob : ITriggerEventsJob
        {
            public EntityCommandBuffer Ecb;
            [ReadOnly] public ComponentDataFromEntity<DestroyOnContactTag> destroyOnContactGroup;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                if (destroyOnContactGroup.HasComponent(triggerEvent.EntityA))
                {
                    Ecb.DestroyEntity(triggerEvent.EntityA);
                }
                if (destroyOnContactGroup.HasComponent(triggerEvent.EntityB))
                {
                    Ecb.DestroyEntity(triggerEvent.EntityB);
                }
            }
        }
        
        private struct DestroyCollisionJob : ICollisionEventsJob
        {
            public EntityCommandBuffer Ecb;
            [ReadOnly] public ComponentDataFromEntity<DestroyOnContactTag> DestroyOnContactGroup;
            
            public void Execute(CollisionEvent collisionEvent)
            {
                if (DestroyOnContactGroup.HasComponent(collisionEvent.EntityA))
                {
                    Ecb.DestroyEntity(collisionEvent.EntityA);
                }
                if (DestroyOnContactGroup.HasComponent(collisionEvent.EntityB))
                {
                    Ecb.DestroyEntity(collisionEvent.EntityB);
                }
            }
        }
        
        
    }
}