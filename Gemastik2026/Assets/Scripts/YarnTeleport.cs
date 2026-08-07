using UnityEngine;
using Yarn.Unity; // Wajib import namespace Yarn.Unity

public class YarnTeleport : MonoBehaviour
{
    [Header("Referensi Player")]
    public Transform playerTransform; // Drag GameObject Player ke sini

    void Start()
    {
        // Jika playerTransform belum diisi, cari otomatis berdasarkan Tag "Player"
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    /// <summary>
    /// Perintah khusus Yarn Spinner untuk memindahkan posisi Player.
    /// Cara panggil di Yarn: <<teleport Player NamaPoint>>
    /// </summary>
    [YarnCommand("teleport")]
    public void TeleportToPoint(string targetPointName)
    {
        // Cari objek titik tujuan di Hierarchy berdasarkan namanya
        GameObject targetPoint = GameObject.Find(targetPointName);

        if (targetPoint != null && playerTransform != null)
        {
            // Pindahkan posisi Player ke titik lokasi tujuan
            playerTransform.position = targetPoint.transform.position;
            Debug.Log($"[Yarn] Player berhasil diteleportasi ke: {targetPointName}");
        }
        else
        {
            Debug.LogError($"[Yarn Error] Gagal Teleport! Target point '{targetPointName}' atau Player tidak ditemukan.");
        }
    }
}