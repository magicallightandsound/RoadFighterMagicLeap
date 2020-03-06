using System;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class TreesSpawnerSystem : JobComponentSystem
{
    private EntityQuery m_entityQuery;
    
    protected override void OnCreate()
    {
        m_entityQuery = GetEntityQuery(typeof(TreesSpawner));
    }
    
    [BurstCompile]
    struct PlaceTreesJob : IJobParallelFor
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
                float3 position         = new float3(8.25f, 0, i * 2.5f);

                Translations[instanceEntity] = new Translation { Value = position };
            }
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.WithStructuralChanges().ForEach((Entity entity, in TreesSpawner treeSpawner, in LocalToWorld location) =>
        {
            inputDeps.Complete();
            var treeBuffer = EntityManager.GetBuffer<TreeBuffer>(entity);

            int roadLength = 10;
            int treesPerRoadFragment = 3;
            
            /*NativeArray<Entity> instances = new NativeArray<Entity>( roadLength * treesPerRoadFragment, Allocator.TempJob, NativeArrayOptions.ClearMemory);
            EntityManager.Instantiate(treeBuffer[0].Prefab, instances);
            
            inputDeps = new PlaceTreesJob
            {
                Instances = instances,
                Translations = GetComponentDataFromEntity<Translation>()
            }.Schedule( roadLength * treesPerRoadFragment, 1, default);
            */
            EntityManager.DestroyEntity(entity);
        }).Run();

        return inputDeps;
    }
}