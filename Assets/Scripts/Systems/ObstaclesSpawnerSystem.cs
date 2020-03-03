using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

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
        float dt = Time.DeltaTime;
        Random rnd = new Random();
        rnd.InitState((uint)(dt * 100000));

        if (m_timeToInstance > 0)
        {
            m_timeToInstance -= Time.DeltaTime;
            return inputDeps;
        }
        else
            m_timeToInstance = rnd.NextFloat(5, 10); // Instantiation rate

        EntityCommandBuffer.Concurrent commandBufferCreate = commandBufferSystem.CreateCommandBuffer().ToConcurrent();

        JobHandle spawnJobHandle = Entities.ForEach
        (
            (int entityInQueryIndex, in ObstacleSpawner spawn, in LocalToWorld center) =>
            {
                int roadWidth = 5;      // TODO: Here should be calculed the 'width' of the road
                int roadEntities = 10;  // TODO: Here should be accesed the quantity of Entities instantiated (this information is storaged on the RoadSpawner Length)

                Entity spawnedEntity = commandBufferCreate.Instantiate(entityInQueryIndex, spawn.Prefab);
                Translation translation = new Translation() { Value = rnd.NextInt3(new int3(-5, 0, 0), new int3(5, 0, 0)) + new int3(0, 0, (roadEntities - 1) * roadWidth) };

                MovementSpeed movementSpeed = new MovementSpeed() { Value = 1 }; // TODO: This value should change according to the game difficulty

                commandBufferCreate.SetComponent(entityInQueryIndex, spawnedEntity, translation);
                commandBufferCreate.SetComponent(entityInQueryIndex, spawnedEntity, movementSpeed);
            }
        )
        .WithName("ObstaclesSpawnerSystem")
        .Schedule(inputDeps);

        commandBufferSystem.AddJobHandleForProducer(spawnJobHandle);

        inputDeps = JobHandle.CombineDependencies(spawnJobHandle, inputDeps);

        return inputDeps;
    }
}
