using DataComponents;
using Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;


namespace Systems.DamageSystems
{
    [UpdateBefore(typeof(BlockSystem))]
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
            var damageCollisionJob = new DamagePlayerOnCollisionJob
            {
                DamageGroup = GetBufferFromEntity<Damage>(),
                DeadGroup = GetComponentDataFromEntity<DeadTag>(true),
                DealDamageGroup = GetComponentDataFromEntity<DealDamage>(true),
                PlayerGroup = GetComponentDataFromEntity<PlayerTag>(true)
            };

            var damageEnemyOnCollisionJob = new DamageEnemyOnCollisionJob()
            {
                DamageGroup = GetBufferFromEntity<Damage>(),
                DeadGroup = GetComponentDataFromEntity<DeadTag>(true),
                DealDamageGroup = GetComponentDataFromEntity<DealDamage>(true),
                EnemyGroup = GetComponentDataFromEntity<EnemyTag>(true)
            };

            damageCollisionJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps).Complete();

            damageEnemyOnCollisionJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps).Complete();

            return inputDeps;
        }

        private struct DamagePlayerOnCollisionJob : ITriggerEventsJob
        {
            public BufferFromEntity<Damage> DamageGroup;
            [ReadOnly] public ComponentDataFromEntity<DeadTag> DeadGroup;

            [ReadOnly] public ComponentDataFromEntity<DealDamage> DealDamageGroup;

            [ReadOnly] public ComponentDataFromEntity<PlayerTag> PlayerGroup;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (PlayerGroup.HasComponent(triggerEvent.EntityB))
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
                }

                if (PlayerGroup.HasComponent(triggerEvent.EntityA))
                {
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


        private struct DamageEnemyOnCollisionJob : ITriggerEventsJob
        {
            public BufferFromEntity<Damage> DamageGroup;
            [ReadOnly] public ComponentDataFromEntity<DeadTag> DeadGroup;

            [ReadOnly] public ComponentDataFromEntity<DealDamage> DealDamageGroup;

            [ReadOnly] public ComponentDataFromEntity<EnemyTag> EnemyGroup;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (EnemyGroup.HasComponent(triggerEvent.EntityB))
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
                }

                if (EnemyGroup.HasComponent(triggerEvent.EntityA))
                {
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
}