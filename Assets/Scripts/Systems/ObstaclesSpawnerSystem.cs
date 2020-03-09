using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

public class ObstaclesSpawnerSystem : JobComponentSystem
{
    float m_timeToInstance = 0;
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        Random rnd = new Random();
        rnd.InitState();
        m_timeToInstance = rnd.NextFloat(1, 3);

        commandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // Non-deterministic Random Number Generation
        float dt = Time.DeltaTime;
        Random rnd = new Random();
        rnd.InitState();
        
        uint systemVersion = LastSystemVersion;

        if (m_timeToInstance > 0)
        {
            m_timeToInstance -= Time.DeltaTime;
            return inputDeps;
        }
        else
            m_timeToInstance = rnd.NextFloat(10, 15); // Instantiation rate

        EntityCommandBuffer.Concurrent commandBufferCreate = commandBufferSystem.CreateCommandBuffer().ToConcurrent();

        double elapsedTime = Time.ElapsedTime * 10000;

        JobHandle spawnJobHandle = Entities.ForEach
        (
            (int entityInQueryIndex, in ObstacleSpawner spawn, in LocalToWorld center) =>
            {
                // Using Random in a Job
                // Calculate a seed per entity. Note that the seed must not be 0.
                var seed = (uint)((systemVersion + 1) * (entityInQueryIndex + 1) * 0x9F6ABC1 ); 
                Random rndPos = new Random(seed);

                int roadWidth = 5;      // TODO: Here should be calculed the 'width' of the road
                int roadEntities = 10;  // TODO: Here should be accesed the quantity of Entities instantiated (this information is storaged on the RoadSpawner Length)

                Entity spawnedEntity = commandBufferCreate.Instantiate(entityInQueryIndex, spawn.Prefab);
                int randomRoad = rndPos.NextInt(-1, 2);
                Translation translation = new Translation() { Value = new int3(randomRoad * 5, 0, 0) + new int3(0, 0, (roadEntities - 1) * roadWidth) };
                
                //MovementSpeed movementSpeed = new MovementSpeed() { Value = 1 }; // TODO: This value should change according to the game difficulty

                commandBufferCreate.SetComponent(entityInQueryIndex, spawnedEntity, translation);
                //commandBufferCreate.SetComponent(entityInQueryIndex, spawnedEntity, movementSpeed);
            }
        )
        .WithName("ObstaclesSpawnerSystem")
        .Schedule(inputDeps);

        commandBufferSystem.AddJobHandleForProducer(spawnJobHandle);

        inputDeps = JobHandle.CombineDependencies(spawnJobHandle, inputDeps);

        return inputDeps;
    }
}
