using DataComponents;
using Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems.DamageSystems
{
    
    [UpdateBefore(typeof(DeathCleanupSystem))]
    public class ResolveDamageSystem: JobComponentSystem
    {

        private EndSimulationEntityCommandBufferSystem ecbSystem;
        
        
        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

            Entities.WithoutBurst().WithNone<DeadTag>().ForEach((Entity entity, ref DynamicBuffer<Damage> damageBuffer, ref HealthData healthData) =>
            {
                foreach (var damage in damageBuffer)
                {
                    healthData.Value -= damage.Value;
                    if (healthData.Value <=0)
                    {
                        healthData.Value = 0;
                        ecb.AddComponent<DeadTag>(entity);
                        break;
                    }
                }
                damageBuffer.Clear();
            }).Run();
            
            return default;
        }
        
        private struct DamageCollisionJob : ITriggerEventsJob
        {
            public BufferFromEntity<Damage> damageGroup;
            [ReadOnly] public ComponentDataFromEntity<DealDamage> dealDamageGroup;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                if (dealDamageGroup.HasComponent(triggerEvent.EntityA))
                {
                    if (damageGroup.HasComponent(triggerEvent.EntityB))
                    {
                        damageGroup[triggerEvent.EntityB].Add(new Damage
                        {
                            Value = dealDamageGroup[triggerEvent.EntityA].Value
                        });
                    }
                }
                
                if (dealDamageGroup.HasComponent(triggerEvent.EntityB))
                {
                    if (damageGroup.HasComponent(triggerEvent.EntityA))
                    {
                        damageGroup[triggerEvent.EntityA].Add(new Damage
                        {
                            Value = dealDamageGroup[triggerEvent.EntityB].Value
                        });
                    }
                }
            }
        }
    }
}

