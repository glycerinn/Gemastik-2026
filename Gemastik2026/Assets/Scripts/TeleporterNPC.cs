using UnityEngine;
using Yarn.Unity;

public class TeleporterNPC : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueRunner dialogueRunner;
    public string dialogueNode;

    [Header("Player & Interaction")]
    public Transform player;
    public float interactDistance = 2f;

    [Header("Highlight")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;

        // Mencari player otomatis jika kolom Player di Inspector dibiarkan kosong
        if (player == null)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null) player = pObj.transform;
        }
    }

    private void Update()
    {
        // Jangan interaksi jika percakapan Yarn sedang berjalan atau player tidak ada
        if (dialogueRunner == null || dialogueRunner.IsDialogueRunning || player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Efek warna highlight saat player mendekat
        if (spriteRenderer != null)
        {
            spriteRenderer.color = distance <= interactDistance ? highlightColor : normalColor;
        }

        // Tekan 'E' untuk memulai dialog
        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            Talk();
        }
    }

    public void Talk()
    {
        dialogueRunner.StartDialogue(dialogueNode);
    }

    // ---------------------------------------------------------
    // YARN COMMAND KHUSUS TELEPORTASI
    // Menggunakan 'static' agar command ini bersifat global
    // dan bisa dipakai berulang kali tanpa bentrok antar NPC
    // ---------------------------------------------------------
    [YarnCommand("teleport_player")]
    public static void TeleportPlayer(string targetPointName)
    {
        GameObject targetPoint = GameObject.Find(targetPointName);
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (targetPoint != null && playerObj != null)
        {
            if (FadeManager.Instance != null)
            {
                // Jika log ini muncul, berarti FadeManager terbaca dengan baik
                Debug.Log("FADEMANAGER DITEMUKAN! Memulai coroutine animasi fade...");
                FadeManager.Instance.TeleportWithFade(playerObj.transform, targetPoint.transform.position);
            }
            else
            {
                // Jika log ini muncul, ini ALASAN MENGAPA player langsung teleport!
                Debug.LogError("FADEMANAGER NULL! Player dipindah paksa secara instan.");
                playerObj.transform.position = targetPoint.transform.position;
            }
        }
    }
}