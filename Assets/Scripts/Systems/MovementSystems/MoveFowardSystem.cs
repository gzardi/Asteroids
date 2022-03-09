using Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


namespace Systems
{
    
    public class MoveFowardSystem: SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;


            Entities
                .WithAll<AsteroidTag>()
                .WithNone<PlayerTag>()
                .ForEach((ref Translation translation , in AsteroidMovementData movementData, in Rotation rotation) =>
            {
                float3 forwardDirection = math.forward(rotation.Value);
                translation.Value += forwardDirection * movementData.Speed * deltaTime;


            }).ScheduleParallel();
            
        }
    }
}