using UnityEngine;

[ExecuteInEditMode]
public class ClippingShaderMultiplePlanes : MonoBehaviour
{
    public Transform InitialPlanePosition;
    public Transform FinalPlanePosition;
    public Material clippingMaterial;

    void Update()
    {
        clippingMaterial.SetVector("_InitialPlanePosition", InitialPlanePosition.position);
        clippingMaterial.SetVector("_InitialPlaneNormal", InitialPlanePosition.forward);

        clippingMaterial.SetVector("_FinalPlanePosition", FinalPlanePosition.position);
        clippingMaterial.SetVector("_FinalPlaneNormal", FinalPlanePosition.forward);
    }
}