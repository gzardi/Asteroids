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


        Entities.ForEach((Entity player, ref PhysicsVelocity vel, ref PhysicsMass physicsMass, ref Rotation rotation,
            in LocalToWorld localToWorld, in MovementData speedData) =>
        {
            if (Input.GetKey(speedData.ForwardKey))
            {
                var force = (float3) localToWorld.Forward * speedData.Speed * deltaTime;

                float x = Mathf.Clamp(force.x, -speedData.MaxForce, speedData.MaxForce);

                float z = Mathf.Clamp(force.z, -speedData.MaxForce, speedData.MaxForce);

                var impulse = new Vector3(x, force.y, z);

                vel.ApplyLinearImpulse(physicsMass, impulse);
            }

            if (Input.GetKey(speedData.BackwardKey))
            {
                var force = (float3) localToWorld.Forward * speedData.Speed * deltaTime ;

                float x = Mathf.Clamp(force.x, -speedData.MaxForce, speedData.MaxForce);

                float z = Mathf.Clamp(force.z, -speedData.MaxForce, speedData.MaxForce);

                var impulse = new Vector3(x, force.y, z);

                vel.ApplyLinearImpulse(physicsMass, -impulse);
            }

            if (Input.GetKey(speedData.LeftKey))
            {
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(speedData.TurnSpeed * deltaTime)));
            }

            if (Input.GetKey(speedData.RightKey))
            {
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(-speedData.TurnSpeed * deltaTime)));
            }
        }).Run();
    }
}