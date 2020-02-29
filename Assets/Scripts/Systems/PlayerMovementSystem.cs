using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class PlayerMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float m_DeltaTime = Time.DeltaTime;

        var jobHandle = Entities.ForEach((ref Translation translation, ref Rotation rotation, in InputData input, in PlayerTag playerTag, in MovementSpeed movementSpeed, in RotationSpeed rotationSpeed) =>
        {
            translation.Value += input.Value * movementSpeed.Value * m_DeltaTime;
            rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), input.Value.x * rotationSpeed.Value * m_DeltaTime));

            // The rotation restitution is the twice the rotation speed
            rotation.Value = math.lerp(rotation.Value.value, quaternion.identity.value, (rotationSpeed.Value * 2) * m_DeltaTime);
        }).Schedule(inputDeps);

        return jobHandle;
    }
}
