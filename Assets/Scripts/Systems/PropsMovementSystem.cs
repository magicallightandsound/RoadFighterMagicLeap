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

                            int roadWidth = 5;      // TODO: Here should be calculed the 'width' of the road
                            int roadEntities = 10;  // TODO: Here should be accesed the quantity of Entities instantiated (this information is storaged on the RoadSpawner Length)

                            if (translation.Value.z <= -roadWidth)
                                translation.Value.z = (roadEntities - 1) * roadWidth;
                        })
                        .Schedule(inputDeps);
    }
}
