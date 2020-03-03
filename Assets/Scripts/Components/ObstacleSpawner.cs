using Unity.Entities;

[GenerateAuthoringComponent]
public struct ObstacleSpawner : IComponentData
{
    public Entity Prefab;
}
