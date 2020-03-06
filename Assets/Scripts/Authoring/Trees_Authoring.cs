using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Trees_Authoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject[] TreePrefabs;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        DynamicBuffer<TreeBuffer> dynamicBuffer = dstManager.AddBuffer<TreeBuffer>(entity);

        for (int i = 0; i < TreePrefabs.Length; i++)
        {
            var prefab = conversionSystem.GetPrimaryEntity(TreePrefabs[i]);
            dynamicBuffer.Add(new TreeBuffer() {Prefab = prefab});
        }
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        for (int i = 0; i < TreePrefabs.Length; i++)
        {
            referencedPrefabs.Add( TreePrefabs[i] );
        }
    }
}
