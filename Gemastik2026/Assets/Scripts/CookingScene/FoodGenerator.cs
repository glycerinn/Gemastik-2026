using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FoodGenerator : MonoBehaviour
{
    public ChoiceSlot[] choiceSlots;
    public List<FoodSO> availableFoods = new List<FoodSO>();
    public GameObject itemPrefab;

    [Header("Animasi Transisi")]
    public float transitionDuration = 0.25f;

    // Flag untuk mengunci tombol (cooldown)
    private bool isRerolling = false;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = AudioManager.instance;
    }

    private void Start()
    {
        GenerateAll();
    }

    public void GenerateAll()
    {
        StartCoroutine(GenerateAllRoutine());
    }

    private IEnumerator GenerateAllRoutine()
    {
        foreach (ChoiceSlot slot in choiceSlots)
        {
            if (!slot.hasBeenChosen)
            {
                StartCoroutine(SpawnAndFadeIn(slot));
            }
        }
        yield return null;
    }

    private IEnumerator SpawnAndFadeIn(ChoiceSlot slot)
    {
        foreach (Transform child in slot.transform) Destroy(child.gameObject);

        GameObject obj = Instantiate(itemPrefab);
        obj.transform.SetParent(slot.transform, false);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.zero;

        List<FoodSO> foods = IngredientManager.Instance.GetAvailableFoods();
        FoodSO selectedFood = foods[Random.Range(0, foods.Count)];

        DraggableItem drag = obj.GetComponent<DraggableItem>();
        drag.food = selectedFood;
        drag.homeSlot = slot;
        obj.GetComponent<Image>().sprite = selectedFood.icon;

        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null) cg = obj.AddComponent<CanvasGroup>();
        cg.alpha = 0f;

        float time = 0;
        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            float progress = time / transitionDuration;
            rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress);
            cg.alpha = Mathf.Lerp(0f, 1f, progress);
            yield return null;
        }

        rt.localScale = Vector3.one;
        cg.alpha = 1f;
    }

    public void Reroll()
    {
        audioManager.playRestockSFX();
        // Jika sedang reroll, abaikan klik (Cooldown system)
        if (isRerolling) return;
        StartCoroutine(RerollRoutine());
    }

    private IEnumerator RerollRoutine()
    {
        isRerolling = true; // Kunci tombol

        // 1. Animasi menghilang untuk makanan lama (jika masih ada di slot)
        foreach (ChoiceSlot slot in choiceSlots)
        {
            // PENGUBAHAN: Hapus "if (slot.hasBeenChosen) continue;" 
            // agar slot yang sudah diambil tetap ikut proses reset.

            foreach (Transform child in slot.transform)
            {
                StartCoroutine(FadeOutAndDestroy(child.gameObject));
            }
        }

        // TUNGGU: Animasi pudar selesai + Jeda 1 detik tambahan
        yield return new WaitForSeconds(transitionDuration + 0.5f);

        // 2. Munculkan makanan baru di SEMUA slot
        foreach (ChoiceSlot slot in choiceSlots)
        {
            // PENGUBAHAN: Reset status slot agar game tahu slot ini sudah diisi ulang
            slot.hasBeenChosen = false;
            StartCoroutine(SpawnAndFadeIn(slot));
        }

        isRerolling = false; // Buka kunci tombol
    }

    public void ResetChoices()
    {
        StartCoroutine(ResetChoicesRoutine());
    }

    private IEnumerator ResetChoicesRoutine()
    {
        foreach (ChoiceSlot slot in choiceSlots)
        {
            slot.hasBeenChosen = false;
            foreach (Transform child in slot.transform)
            {
                StartCoroutine(FadeOutAndDestroy(child.gameObject));
            }
        }
        yield return new WaitForSeconds(transitionDuration);
        StartCoroutine(GenerateAllRoutine());
    }

    private IEnumerator FadeOutAndDestroy(GameObject obj)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null) cg = obj.AddComponent<CanvasGroup>();

        cg.blocksRaycasts = false;

        float time = 0;
        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            float progress = time / transitionDuration;

            if (rt != null) rt.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, progress);
            if (cg != null) cg.alpha = Mathf.Lerp(1f, 0f, progress);

            yield return null;
        }
        Destroy(obj);
    }
}