using Unity.Entities;

[GenerateAuthoringComponent]
public struct RoadSpawner : IComponentData
{
    public Entity Prefab;
    public float Length;
}
