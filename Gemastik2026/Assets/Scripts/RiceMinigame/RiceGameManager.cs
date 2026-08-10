using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RiceGameManager : MonoBehaviour
{
    public static RiceGameManager Instance;
    public List<Rice> riceList;
    public TMP_Text scoreText;
    public GameObject winPanel;
    public int targetScore = 30;

    [Header("Polishing Settings")]
    public ParticleSystem harvestParticlePrefab;
    public Color particleColor = new Color(1f, 0.9f, 0.2f);

    int score;
    private bool isInputLocked = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeField();
        UpdateUI();
    }

    void InitializeField()
    {
        foreach (Rice rice in riceList)
        {
            if (rice != null)
            {
                Rice.RiceStage randomStage = (Rice.RiceStage)Random.Range(0, 4);
                rice.SetStage(randomStage);
            }
        }

        CheckAndForceReadyRice();
    }

    public void OnRiceClicked(Rice clickedRice)
    {
        if (clickedRice == null) return;
        if (isInputLocked) return;

        if (clickedRice.stage == Rice.RiceStage.Ready)
        {
            score++;
            UpdateUI();

            isInputLocked = true;

            // Memanggil partikel dan menjadikannya anak (child) dari padi yang diklik
            SpawnHarvestParticle(clickedRice.transform.position, clickedRice.transform);

            clickedRice.PlayJuicyHarvest(() =>
            {
                if (score >= targetScore)
                {
                    if (winPanel != null)
                        winPanel.SetActive(true);
                }
                else
                {
                    GrowRandomRice();
                }

                isInputLocked = false;
            });
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Panen: {score} / {targetScore}";
    }

    void GrowRandomRice()
    {
        foreach (Rice rice in riceList)
        {
            if (rice != null) rice.Grow();
        }
        CheckAndForceReadyRice();
    }

    // --- PERBAIKAN LOGIKA SOFTLOCK ---
    void CheckAndForceReadyRice()
    {
        bool hasReady = false;
        foreach (Rice rice in riceList)
        {
            if (rice != null && rice.stage == Rice.RiceStage.Ready) { hasReady = true; break; }
        }

        // Jika BENAR-BENAR tidak ada padi yang Ready, kita harus paksa 1 padi menjadi Ready
        if (!hasReady)
        {
            List<Rice> nearingList = new List<Rice>();
            List<Rice> youngList = new List<Rice>();
            List<Rice> seedList = new List<Rice>();

            foreach (Rice rice in riceList)
            {
                if (rice == null) continue;
                if (rice.stage == Rice.RiceStage.Nearing) nearingList.Add(rice);
                else if (rice.stage == Rice.RiceStage.Young) youngList.Add(rice);
                else if (rice.stage == Rice.RiceStage.Seed) seedList.Add(rice);
            }

            Rice chosenRice = null;

            // Prioritaskan padi yang sudah hampir matang (Nearing)
            if (nearingList.Count > 0) chosenRice = nearingList[Random.Range(0, nearingList.Count)];
            else if (youngList.Count > 0) chosenRice = youngList[Random.Range(0, youngList.Count)];
            else if (seedList.Count > 0) chosenRice = seedList[Random.Range(0, seedList.Count)];

            // Paksa padi yang terpilih langsung menjadi Ready agar game bisa dilanjutkan!
            if (chosenRice != null)
            {
                chosenRice.SetStage(Rice.RiceStage.Ready);
            }
        }
    }

    // --- PERBAIKAN MUNCULNYA PARTIKEL ---
    void SpawnHarvestParticle(Vector3 position, Transform parentObject)
    {
        if (harvestParticlePrefab != null)
        {
            // Spawn partikel dan jadikan anak dari padi agar terbawa oleh skala Canvas
            ParticleSystem effect = Instantiate(harvestParticlePrefab, position, Quaternion.identity, parentObject);

            // Dorong sedikit partikelnya ke arah kamera agar tidak tertimpa gambar UI
            effect.transform.localPosition = new Vector3(0, 0, -50f);

            var mainModule = effect.main;
            mainModule.startColor = particleColor;

            effect.Play();
        }
    }
}