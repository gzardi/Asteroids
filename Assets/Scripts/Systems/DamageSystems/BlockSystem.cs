using DataComponents;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Systems.DamageSystems
{
    [UpdateBefore(typeof(ResolveDamageSystem))]
    public class BlockSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem EcbSystem;

        protected override void OnCreate()
        {
            EcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityCommandBuffer ecb = EcbSystem.CreateCommandBuffer();

            Entities.WithoutBurst().ForEach((Entity entity, ref BlockDamageData blockDamageData, ref DynamicBuffer<Damage> damageBuffer) =>
            {
                if (damageBuffer.Length == 0)
                {
                    return;
                }

                DynamicBuffer<int> damageValueBuffer = damageBuffer.Reinterpret<int>();

                for (int i = 0; i < damageValueBuffer.Length; i++)
                {
                    int damageToBlock = math.min(blockDamageData.Value, damageValueBuffer[i]);
                    blockDamageData.Value -= damageToBlock;
                    damageValueBuffer[i] -= damageToBlock;
                    
                    if (blockDamageData.Value == 0)
                    {
                        ecb.RemoveComponent<BlockDamageData>(entity);
                    }
                }
            }).Run();


            return default;
        }
    }
}