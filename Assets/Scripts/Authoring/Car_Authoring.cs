using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Car_Authoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float MovementSpeed = 1.0f;
    public float RotationSpeed = 1.0f;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new InputData());
        dstManager.AddComponentData(entity, new PlayerTag());
        dstManager.AddComponentData(entity, new MovementSpeed { Value = MovementSpeed });
        dstManager.AddComponentData(entity, new RotationSpeed { Value = RotationSpeed });
    }
}
