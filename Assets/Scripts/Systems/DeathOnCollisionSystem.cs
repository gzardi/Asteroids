using Tags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class DeathOnCollisionSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;


    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();

        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    [BurstCompile]
    struct DeathOnCollisionSystemJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<DeathColliderTag> deathColliderGroup;

        [ReadOnly] public ComponentDataFromEntity<PlayerTag> playerColliderGroup;

        public ComponentDataFromEntity<HealthData> healthGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;

            Entity entityB = collisionEvent.EntityB;

            bool entityAIsPlayer = playerColliderGroup.HasComponent(entityA);

            bool entityAIsDeathCollider = deathColliderGroup.HasComponent(entityA);


            bool entityBIsPlayer = playerColliderGroup.HasComponent(entityB);

            bool entityBIsDeathCollider = deathColliderGroup.HasComponent(entityB);


            if (entityAIsDeathCollider && entityAIsPlayer)
            {
                HealthData newHealthData = healthGroup[entityB];
                newHealthData.IsDead = true;
                healthGroup[entityB] = newHealthData;
            }

            if (entityBIsDeathCollider && entityAIsPlayer)
            {
                HealthData newHealthData = healthGroup[entityA];
                newHealthData.IsDead = true;
                healthGroup[entityA] = newHealthData;
            }
        }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new DeathOnCollisionSystemJob();

        job.deathColliderGroup = GetComponentDataFromEntity<DeathColliderTag>(true);

        job.playerColliderGroup = GetComponentDataFromEntity<PlayerTag>(true);

        job.healthGroup = GetComponentDataFromEntity<HealthData>();


        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);

        jobHandle.Complete();

        return jobHandle;
    }
}