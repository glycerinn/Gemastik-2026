using UnityEngine;

public class LayerSwitcher2D : MonoBehaviour
{
    [Header("Nama Layer")]
    public string layerGunung = "PlayerGunung";
    public string layerDesa = "Player";

    [Header("Tag Player")]
    public string playerTag = "Player";

    // Status untuk melacak posisi jalur (False = di Desa/Bawah, True = di Gunung/Atas)
    private bool isGunung = false;

    // Terpanggil otomatis saat Player menyentuh kotak Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Balik status setiap kali trigger disentuh (Toggle)
            isGunung = !isGunung;

            string targetLayerName = isGunung ? layerGunung : layerDesa;
            int targetLayerIndex = LayerMask.NameToLayer(targetLayerName);

            if (targetLayerIndex != -1)
            {
                other.gameObject.layer = targetLayerIndex;

                // Jika player memiliki anak objek (child), ubah juga layernya agar ikut
                foreach (Transform child in other.transform)
                {
                    child.gameObject.layer = targetLayerIndex;
                }

                Debug.Log("Status jalur berubah. Layer sekarang: " + targetLayerName);
            }
        }
    }
}