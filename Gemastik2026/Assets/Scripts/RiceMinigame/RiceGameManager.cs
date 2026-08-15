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
    public int targetScore = 50;

    [Header("Polishing Settings")]
    public ParticleSystem harvestParticlePrefab;

    private int score = 0;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.instance;
        Instance = this;
    }

    private void Start()
    {
        InitializeField();
        UpdateUI();
    }

    void InitializeField()
    {
        int readyCount = 0;

        foreach (Rice rice in riceList)
        {
            if (rice != null)
            {
                // Mengatur agar tepat 3 padi yang siap panen di awal
                if (readyCount < 3)
                {
                    rice.SetStage(Rice.RiceStage.Ready, animate: false);
                    readyCount++;
                }
                else
                {
                    // Sisanya diberikan status Acak (Benih atau Padi Muda)
                    Rice.RiceStage randomStage = (Rice.RiceStage)Random.Range(0, 2);
                    rice.SetStage(randomStage, animate: false);
                }
            }
        }

        EnsureAtLeastOneReady();
    }

    public void OnRiceClicked(Rice clickedRice)
    {
        if (clickedRice == null || clickedRice.isAnimating) return;

        if (clickedRice.stage == Rice.RiceStage.Ready)
        {
            audioManager.playTrashHarvestTakeSFX();
            SpawnHarvestParticle(clickedRice.transform.position, clickedRice.transform);

            StartCoroutine(clickedRice.HarvestRoutine(() =>
            {
                score++;
                UpdateUI();

                // Tumbuhkan padi lain!
                GrowOtherRices(clickedRice);
                EnsureAtLeastOneReady(); // Jaga-jaga anti softlock

                if (score >= targetScore)
                {
                    if (winPanel != null) winPanel.SetActive(true);
                    Time.timeScale = 0;
                }
            }));
        }
    }

    void GrowOtherRices(Rice harvestedRice)
    {
        // PERTUMBUHAN CEPAT: 
        // Setiap kali kita panen 1 padi, SEMUA padi lain yang belum matang akan bertumbuh 1 tahap!
        foreach (Rice r in riceList)
        {
            if (r != null && r != harvestedRice && r.stage != Rice.RiceStage.Ready)
            {
                r.Grow();
            }
        }
    }

    void EnsureAtLeastOneReady()
    {
        bool hasReady = false;
        foreach (Rice r in riceList)
        {
            if (r != null && r.stage == Rice.RiceStage.Ready)
            {
                hasReady = true;
                break;
            }
        }

        if (!hasReady)
        {
            // Cari padi paling matang untuk dipaksa jadi Ready
            Rice chosenRice = null;
            foreach (Rice r in riceList)
            {
                if (r == null) continue;
                if (r.stage == Rice.RiceStage.Nearing) { chosenRice = r; break; }
                if (r.stage == Rice.RiceStage.Young) { chosenRice = r; break; }
                if (r.stage == Rice.RiceStage.Seed) { chosenRice = r; break; }
            }
            if (chosenRice != null) chosenRice.SetStage(Rice.RiceStage.Ready, animate: true);
        }
    }

    void SpawnHarvestParticle(Vector3 position, Transform parentObject)
    {
        if (harvestParticlePrefab != null)
        {
            ParticleSystem effect = Instantiate(harvestParticlePrefab, position, Quaternion.identity, parentObject);
            effect.transform.localPosition = new Vector3(0, 0, -50f);
            effect.Play();
            Destroy(effect.gameObject, 1.5f);
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score + " / " + targetScore;
        }
    }
}