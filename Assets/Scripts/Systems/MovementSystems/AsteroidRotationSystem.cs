using Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


namespace Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public class AsteroidRotationSystem: SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;


            Entities
                .WithAll<RotationTag>()
                .WithNone<PlayerTag>()
                .ForEach((ref Rotation rotation , in AsteroidMovementData movementData) =>
            {
                quaternion normRotation = math.normalize(rotation.Value);
                quaternion angleToRotate = quaternion.AxisAngle(math.up(), movementData.TurnSpeed * deltaTime);

                rotation.Value = math.mul(normRotation, angleToRotate);


            }).ScheduleParallel();
            
        }
    }
}
