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
        // 1. Kalkulasi posisi dari dorongan Mouse
        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;

        // 2. Kalkulasi posisi dari arus air (Gerakan mengambang otomatis)
        // Kita menggunakan Sin untuk X dan Cos untuk Y agar pergerakannya memutar halus seperti angka 8, bukan diagonal kaku
        float ayunanOtomatisX = Mathf.Sin(Time.time * kecepatanArusX) * jarakAyunanX;
        float ayunanOtomatisY = Mathf.Cos(Time.time * kecepatanArusY) * jarakAyunanY;

        // 3. Gabungkan dorongan mouse dengan ayunan otomatis
        Vector3 targetPosisi = new Vector3(
            (mouseX * -pergerakanX) + ayunanOtomatisX,
            (mouseY * -pergerakanY) + ayunanOtomatisY,
            0
        );

        // 4. Pindahkan posisi dengan mulus (Lerp)
        transform.position = Vector3.Lerp(transform.position, posisiAwal + targetPosisi, Time.deltaTime * kecepatanSmooth);
    }
}