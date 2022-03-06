using DataComponents;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;


        Entities.ForEach((Entity player, ref PhysicsVelocity velocity, ref PhysicsMass physicsMass, ref Rotation rotation,
            in LocalToWorld localToWorld, in MovementKeys movementKeys, in MovementData movementData) =>
        {
            if (movementKeys.GoForward)
            {
                Thrust(1, ref velocity, ref physicsMass, in movementData, in localToWorld, deltaTime);
            }

            if (movementKeys.GoBackward)
            {
                Thrust(-1, ref velocity, ref physicsMass, in movementData, in localToWorld, deltaTime);
            }

            if (movementKeys.GoLeft)
            {
                Rotate(1, ref rotation, in movementData, deltaTime);
            }

            if (movementKeys.GoRight)
            {
                Rotate(-1, ref rotation, in movementData, deltaTime);
            }
        }).Run();
    }

    private static void Thrust(int direction, ref PhysicsVelocity velocity, ref PhysicsMass physicsMass, in MovementData movementData,
        in LocalToWorld localToWorld, float deltaTime)
    {
        var force = (float3) localToWorld.Forward * movementData.Speed * direction * deltaTime;


        velocity.ApplyLinearImpulse(physicsMass, force);


        velocity.Linear = Vector3.ClampMagnitude(velocity.Linear, movementData.MaxImpulse);
    }

    private static void Rotate(int direction, ref Rotation rotation, in MovementData speedData, float deltaTime)
    {
        rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(speedData.TurnSpeed * direction * deltaTime)));
    }
}