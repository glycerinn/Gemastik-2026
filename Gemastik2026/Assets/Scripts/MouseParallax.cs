using UnityEngine;

public class MouseParallax : MonoBehaviour
{
    [Header("Pengaturan Parallax Mouse")]
    public float pergerakanX = 0.5f; // Seberapa jauh bisa bergeser ke kiri/kanan oleh mouse
    public float pergerakanY = 0.2f; // Seberapa jauh bisa bergeser ke atas/bawah oleh mouse
    public float kecepatanSmooth = 5f; // Kecepatan menghaluskan gerakan

    [Header("Pengaturan Gerakan Otomatis (Idle)")]
    public float kecepatanArusX = 1f;       // Kecepatan ayunan kiri-kanan otomatis
    public float kecepatanArusY = 0.7f;     // Kecepatan ayunan atas-bawah otomatis (sengaja dibedakan agar polanya acak/organik)
    public float jarakAyunanX = 0.1f;       // Seberapa jauh berayun kiri-kanan
    public float jarakAyunanY = 0.05f;      // Seberapa jauh berayun atas-bawah

    private Vector3 posisiAwal;

    void Start()
    {
        posisiAwal = transform.position;
    }

    void Update()
    {
        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;

        float ayunanOtomatisX = Mathf.Sin(Time.time * kecepatanArusX) * jarakAyunanX;
        float ayunanOtomatisY = Mathf.Cos(Time.time * kecepatanArusY) * jarakAyunanY;

        Vector3 targetPosisi = new Vector3(
            (mouseX * -pergerakanX) + ayunanOtomatisX,
            (mouseY * -pergerakanY) + ayunanOtomatisY,
            0
        );

        transform.position = Vector3.Lerp(transform.position, posisiAwal + targetPosisi, Time.deltaTime * kecepatanSmooth);
    }
}