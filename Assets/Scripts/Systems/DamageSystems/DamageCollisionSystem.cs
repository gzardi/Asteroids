
using DataComponents;
using Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;


namespace Systems.DamageSystems
{
    [UpdateBefore(typeof(ResolveDamageSystem))]
    public class DamageCollisionSystem : JobComponentSystem
    {
        private BuildPhysicsWorld buildPhysicsWorld;
        private StepPhysicsWorld stepPhysicsWorld;

        protected override void OnCreate()
        {
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var damageCollisionJob = new DamageCollisionJob
            {
                DamageGroup = GetBufferFromEntity<Damage>(),
                DeadGroup = GetComponentDataFromEntity<DeadTag>(true),
                DealDamageGroup = GetComponentDataFromEntity<DealDamage>(true)
            };
            
            damageCollisionJob.Schedule(stepPhysicsWorld.Simulation,ref buildPhysicsWorld.PhysicsWorld , inputDeps).Complete();

            return inputDeps;
        }
        
        private struct DamageCollisionJob: ITriggerEventsJob
        {
            public BufferFromEntity<Damage> DamageGroup;
            [ReadOnly] public ComponentDataFromEntity<DeadTag> DeadGroup;
            
            [ReadOnly] public ComponentDataFromEntity<DealDamage> DealDamageGroup;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                if (DealDamageGroup.HasComponent(triggerEvent.EntityA))
                {
                    if (!DeadGroup.HasComponent(triggerEvent.EntityB))
                    {
                        if (DamageGroup.HasComponent(triggerEvent.EntityB))
                        {
                            DamageGroup[triggerEvent.EntityB].Add(new Damage
                            {
                                Value = DealDamageGroup[triggerEvent.EntityA].Value
                            });
                        }
                    }
                }
                
                if (DealDamageGroup.HasComponent(triggerEvent.EntityB))
                {
                    if (!DeadGroup.HasComponent(triggerEvent.EntityB))
                    {
                        if (DamageGroup.HasComponent(triggerEvent.EntityA))
                        {
                            DamageGroup[triggerEvent.EntityA].Add(new Damage
                            {
                                Value = DealDamageGroup[triggerEvent.EntityB].Value
                            });
                        }
                    }
                }
            }
        }
    }
}