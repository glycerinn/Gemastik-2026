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

    int score;

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
        // Give every rice a random stage (Seed, Young, Nearing, or Ready)
        foreach (Rice rice in riceList)
        {
            Rice.RiceStage randomStage = (Rice.RiceStage)Random.Range(0, 4);
            rice.SetStage(randomStage);
        }

        // Only guarantee that at least one rice starts Ready
        bool hasReady = false;

        foreach (Rice rice in riceList)
        {
            if (rice.stage == Rice.RiceStage.Ready)
            {
                hasReady = true;
                break;
            }
        }

        if (!hasReady)
        {
            riceList[Random.Range(0, riceList.Count)].SetStage(Rice.RiceStage.Ready);
        }
    }

    public void OnRiceClicked(Rice clickedRice)
    {
        if(clickedRice.stage == Rice.RiceStage.Ready)
        {
            score++;
        }

        clickedRice.SetStage(Rice.RiceStage.Seed);
        foreach(Rice rice in riceList)
        {
            if(rice != clickedRice)
                rice.Grow();
        }

        EnsurePlayable();
        UpdateUI();

        if(score >= targetScore)
        {
            winPanel.SetActive(true);
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Rice : " + score + " / " + targetScore;
    }

    void EnsurePlayable()
    {
        bool hasReady = false;
        bool hasNearing = false;

        foreach(Rice rice in riceList)
        {
            if(rice.stage == Rice.RiceStage.Ready)
                hasReady = true;

            if(rice.stage == Rice.RiceStage.Nearing)
                hasNearing = true;
        }

        if(!hasReady)
        {
            Rice candidate = null;

            foreach(Rice rice in riceList)
            {
                if(rice.stage == Rice.RiceStage.Nearing)
                {
                    candidate = rice;
                    break;
                }
            }

            if(candidate == null)
            {
                foreach(Rice rice in riceList)
                {
                    if(rice.stage == Rice.RiceStage.Young)
                    {
                        candidate = rice;
                        break;
                    }
                }
            }

            if(candidate == null)
            {
                candidate = riceList[0];
            }

            candidate.SetStage(Rice.RiceStage.Ready);
        }

        hasNearing = false;

        foreach(Rice rice in riceList)
        {
            if(rice.stage == Rice.RiceStage.Nearing)
            {
                hasNearing = true;
                break;
            }
        }

        if(!hasNearing)
        {
            Rice candidate = null;

            foreach(Rice rice in riceList)
            {
                if(rice.stage == Rice.RiceStage.Young)
                {
                    candidate = rice;
                    break;
                }
            }

            if(candidate == null)
            {
                foreach(Rice rice in riceList)
                {
                    if(rice.stage == Rice.RiceStage.Seed)
                    {
                        candidate = rice;
                        break;
                    }
                }
            }

            if(candidate != null)
                candidate.SetStage(Rice.RiceStage.Nearing);
        }
    }
}