using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public RectTransform canvasRect;
    public float spawnInterval = 1f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnTrash), 0f, spawnInterval);
    }

    void SpawnTrash()
    {
        if (trashPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, trashPrefabs.Length);

        GameObject obj = Instantiate(trashPrefabs[randomIndex], canvasRect);
        obj.transform.SetAsLastSibling();

        RectTransform rect = obj.GetComponent<RectTransform>();

        float width = canvasRect.rect.width;
        float height = canvasRect.rect.height;

        float randomX = Random.Range(-width / 2f, width / 2f);
        float spawnY = height / 2f + 100f;

        rect.anchoredPosition = new Vector2(randomX, spawnY);
    }
}