using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class RoadSpawnerSystem : JobComponentSystem
{
    [BurstCompile]
    struct SetComponentDataJob : IJobParallelFor
    {
        [ReadOnly]
        [DeallocateOnJobCompletion]
        public NativeArray<Entity>                  Instances;

        [NativeDisableParallelForRestriction]
        public ComponentDataFromEntity<Translation> Translations;

        public void Execute(int index)
        {
            for (int i = 0; i < Instances.Length; i++)
            {
                Entity instanceEntity   = Instances[i];
                float3 position         = new float3(0, 0, i * 5);

                Translations[instanceEntity] = new Translation { Value = position };
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.WithStructuralChanges().ForEach((Entity entity, in RoadSpawner roadSpawner, in LocalToWorld location) =>
        {
            inputDeps.Complete();

            EntityManager.DestroyEntity(entity);

            NativeArray<Entity> instances = new NativeArray<Entity>((int)roadSpawner.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
            EntityManager.Instantiate(roadSpawner.Prefab, instances);

            inputDeps = new SetComponentDataJob
            {
                Instances = instances,
                Translations = GetComponentDataFromEntity<Translation>()
            }.Schedule((int)roadSpawner.Length, 1, default);
        }).Run();

        return inputDeps;
    }
}
