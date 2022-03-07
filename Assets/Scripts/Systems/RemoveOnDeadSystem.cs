using Tags;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class RemoveOnDeadSystem: SystemBase
    {
        private EndSimulationEntityCommandBufferSystem CommandBufferSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            
            CommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer entityCommandBuffer = CommandBufferSystem.CreateCommandBuffer();

            Entities.WithAny<PlayerTag,AsteroidTag>().ForEach((Entity entity, in HealthData healthData) =>
            {

                if (healthData.IsDead)
                {
                    
                    entityCommandBuffer.DestroyEntity(entity);
                }

            }).Schedule();
            
            CommandBufferSystem.AddJobHandleForProducer(this.Dependency);

        }
    }
}