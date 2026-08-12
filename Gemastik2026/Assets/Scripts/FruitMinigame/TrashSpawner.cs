using System.Collections;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public RectTransform canvasRect;

    private Coroutine spawnCoroutine;

    void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        StopSpawning();
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnTrash();

            float currentInterval = GameManager.instance != null ?
                GameManager.instance.GetCurrentSpawnInterval() : 1f;

            yield return new WaitForSeconds(currentInterval);
        }
    }

    void SpawnTrash()
    {
        if (trashPrefabs.Length == 0 || canvasRect == null) return;

        int randomIndex = Random.Range(0, trashPrefabs.Length);

        GameObject obj = Instantiate(trashPrefabs[randomIndex], canvasRect);
        obj.transform.SetAsLastSibling();

        RectTransform rect = obj.GetComponent<RectTransform>();

        float width = canvasRect.rect.width;
        float height = canvasRect.rect.height;

        float randomX = Random.Range(-width / 2f + 100f, width / 2f - 100f);
        float spawnY = height / 2f + 120f;

        rect.anchoredPosition = new Vector2(randomX, spawnY);
    }

    // Membersihkan sisa buah dengan transisi pudar (Fade Out)
    public void ClearAllTrash()
    {
        Trash[] activeTrash = canvasRect.GetComponentsInChildren<Trash>();
        foreach (Trash t in activeTrash)
        {
            t.FadeOutAndDestroy(0.25f);
        }
    }
}