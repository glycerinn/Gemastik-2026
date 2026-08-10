using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GateQueue : MonoBehaviour
{
    public GameObject[] fishPrefab, trashPrefab;
    public Transform spawnPos;
    public int SortableCount = 10;
    public float spacing = 15f;
    private List<Sortable> Sortables_ = new List<Sortable>();
    GameObject obj;
    Sortable sortable;
    private bool Busy;
    public GameObject GameOverScreen;
    public int targetScore = 30;
    private int currentScore = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        for (int i = 0; i < SortableCount; i++)
        {
            SpawnQueue(i);
        }
    }

    void Update()
    {
        if (Sortables_.Count == 0 || Busy) return;

        Sortable currentItem = Sortables_[0];

        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 targetPos = currentItem.transform.position + Vector3.up * 6;
            if (currentItem.GetSortableType() == Sortable.SortableType.Fish)
            {
                AddScore();
            }
            else
            {
                RemoveScore();
            }

            StartCoroutine(SortAndShift(currentItem, targetPos));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 targetPos = currentItem.transform.position + Vector3.down * 5;
            if (currentItem.GetSortableType() == Sortable.SortableType.Trash)
            {
                AddScore();
            }
            else
            {
                RemoveScore();
            }
            StartCoroutine(SortAndShift(currentItem, targetPos));
        }
    }

    IEnumerator SortAndShift(Sortable sortable, Vector3 targetPos)
    {
        Busy = true;

        while (Vector3.Distance(sortable.transform.position, targetPos) > 0.01f)
        {
            sortable.transform.position = Vector3.MoveTowards(sortable.transform.position, targetPos, 20 * Time.deltaTime);
            yield return null;
        }

        Destroy(sortable.gameObject);
        Sortables_.RemoveAt(0);

        List<Vector3> targetPositions = new List<Vector3>();
        for (int i = 0; i < Sortables_.Count; i++)
        {
            targetPositions.Add(spawnPos.position + Vector3.right * (i * spacing));
        }

        bool shifting = true;
        while (shifting)
        {
            shifting = false;
            for (int i = 0; i < Sortables_.Count; i++)
            {
                Sortables_[i].transform.position = Vector3.MoveTowards(Sortables_[i].transform.position, targetPositions[i], 10 * Time.deltaTime);
                if (Vector3.Distance(Sortables_[i].transform.position, targetPositions[i]) > 0.01f)
                    shifting = true;
            }
            yield return null;
        }

        SpawnQueue(Sortables_.Count);
        Busy = false;
    }

    public void SpawnQueue(int index)
    {
        GameObject prefab;
        if (Random.value > 0.5f)
        {
            prefab = fishPrefab[Random.Range(0, fishPrefab.Length)];
        }
        else
        {
            prefab = trashPrefab[Random.Range(0, trashPrefab.Length)];
        }
        obj = Instantiate(prefab, spawnPos.position + Vector3.right * (index * spacing), Quaternion.identity);
        sortable = obj.GetComponent<Sortable>();
        Sortables_.Add(sortable);
    }

    void AddScore()
    {
        currentScore++;

        if (scoreText != null)
            scoreText.text = $"{currentScore}/{targetScore}";

        if (currentScore >= targetScore)
        {
            WinGame();
        }
    }

    void RemoveScore()
    {
        currentScore = Mathf.Max(0, currentScore - 1);

        if (scoreText != null)
            scoreText.text = $"{currentScore}/{targetScore}";
    }

    void WinGame()
    {
        Busy = true;
        GameOverScreen.SetActive(true);
    }
}
