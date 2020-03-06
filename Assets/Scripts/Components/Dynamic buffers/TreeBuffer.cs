using Unity.Entities;

[InternalBufferCapacity(8)]
public struct TreeBuffer : IBufferElementData
{
    public Entity Prefab;
}
