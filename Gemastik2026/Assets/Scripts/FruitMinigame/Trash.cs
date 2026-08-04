using UnityEngine;
using UnityEngine.EventSystems;

public class Trash : MonoBehaviour, IPointerClickHandler
{
    float fallSpeed;
    public float fallSpeedMax;
    public float fallSpeedMin;

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();

        fallSpeed = Random.Range(fallSpeedMin, fallSpeedMax);

        float size = Random.Range(140f, 220f);
        rect.sizeDelta = new Vector2(size, size);

        float scale = Random.Range(0.9f, 1.1f);
        rect.localScale = new Vector3(scale, scale, 1f);
    }

    void Update()
    {
        rect.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;

        if (rect.anchoredPosition.y < -1080 / 2f - 100f)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.TrashCollected();
        Destroy(gameObject);
    }
}