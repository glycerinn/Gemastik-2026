using UnityEngine;

[ExecuteAlways]
public class WaterReflectionManager : MonoBehaviour
{
    public Camera mainCamera;
    public Transform waterSurface; // Referensi titik batas air
    public Material waterMaterial;

    void LateUpdate()
    {
        // BARIS PENGAMAN: Jika ada kotak yang belum diisi di Inspector, hentikan skrip diam-diam
        if (mainCamera == null || waterSurface == null || waterMaterial == null)
        {
            return;
        }

        // Baris ke-15 Anda yang error ada di bawah sini. 
        // Sekarang baris ini aman dieksekusi.
        Vector3 screenPos = mainCamera.WorldToScreenPoint(waterSurface.position);
        float screenY = screenPos.y / Screen.height;
        waterMaterial.SetFloat("_WaterLineScreenY", screenY);
    }
}