using Tags;
using Unity.Entities;
using Unity.Jobs;

namespace Systems.DamageSystems
{
    public class DeathCleanupSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem ecbSystem;

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();
            
            Entities.WithoutBurst().WithAll<DeadTag>().ForEach((Entity entity)=>
            {
                ecb.DestroyEntity(entity);
            }).Run();
            return default;
        }
    }
}