using UnityEngine;

[ExecuteInEditMode]
public class ClippingShader : MonoBehaviour
{
    public Transform planePosition;
    public Material[] clippingMaterials;

    void Update()
    {
        for (int i = 0; i < clippingMaterials.Length; i++)
        {
            clippingMaterials[i].SetVector("_PlanePosition", planePosition.position);
            clippingMaterials[i].SetVector("_PlaneNormal", planePosition.forward);
        }
    }
}
