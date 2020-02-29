using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PropsMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float DeltaTime = Time.DeltaTime;

        return Entities.WithNone<PlayerTag>()
                        .ForEach((ref Translation translation, in MovementSpeed movementSpeed) =>
                        {
                            translation.Value -= math.forward(quaternion.identity) * movementSpeed.Value * DeltaTime;
                        })
                        .Schedule(inputDeps);
    }
}
