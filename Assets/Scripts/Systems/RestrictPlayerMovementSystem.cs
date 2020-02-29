using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(PlayerMovementSystem))]
public class RestrictPlayerMovementSystem : JobComponentSystem
{
    [BurstCompile]
    struct RestrictPlayerMovementJob : IJobForEach<PlayerTag, Translation>
    {
        public float Min_X;
        public float Max_X;

        public void Execute([ReadOnly] ref PlayerTag tag, ref Translation translation)
        {
            translation.Value = new float3
                                (
                                    math.clamp(translation.Value.x, Min_X, Max_X),
                                    translation.Value.y,
                                    translation.Value.z
                                );
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RestrictPlayerMovementJob()
                  {
                        Min_X = -5,
                        Max_X = 5
                  };
        return job.Schedule(this, inputDeps);
    }
}
