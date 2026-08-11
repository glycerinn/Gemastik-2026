using UnityEngine;

[ExecuteAlways]
public class WaterReflectionManager : MonoBehaviour
{
    public Camera mainCamera;
    public Transform waterSurface; // Referensi titik batas air
    public Material waterMaterial;

    void LateUpdate()
    {
        if (mainCamera == null || waterSurface == null || waterMaterial == null) return;

        // Mencari posisi Y permukaan air di layar (berupa persentase 0.0 sampai 1.0)
        Vector3 screenPos = mainCamera.WorldToScreenPoint(waterSurface.position);
        float screenY = screenPos.y / Screen.height;

        // Mengirimkan nilai tersebut ke Shader Material
        waterMaterial.SetFloat("_WaterLineScreenY", screenY);
    }
}