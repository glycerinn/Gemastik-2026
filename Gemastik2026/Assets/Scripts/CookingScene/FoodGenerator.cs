using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FoodGenerator : MonoBehaviour
{
    public ChoiceSlot[] choiceSlots;
    public List<FoodSO> availableFoods = new List<FoodSO>();
    public GameObject itemPrefab;

    private void Start()
    {
        GenerateAll();
    }

    public void GenerateAll()
    {
        foreach(ChoiceSlot slot in choiceSlots)
        {
            GenerateItem(slot);
        }
    }

    void GenerateItem(ChoiceSlot slot)
    {
        if(slot.hasBeenChosen)
            return;

        foreach(Transform child in slot.transform)
            Destroy(child.gameObject);

        GameObject obj = Instantiate(itemPrefab);
        obj.transform.SetParent(slot.transform, false);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        List<FoodSO> foods = IngredientManager.Instance.GetAvailableFoods();
        FoodSO selectedFood = foods[Random.Range(0, foods.Count)];

        DraggableItem drag = obj.GetComponent<DraggableItem>();
        drag.food = selectedFood;
        drag.homeSlot = slot;

        obj.GetComponent<Image>().sprite = selectedFood.icon;
    }

    public void Reroll()
    {
        foreach(ChoiceSlot slot in choiceSlots)
        {
            if(slot.hasBeenChosen)
                continue;

            GenerateItem(slot);
        }
    }

    public void ResetChoices()
    {
        foreach (ChoiceSlot slot in choiceSlots)
        {
            slot.hasBeenChosen = false;

            foreach (Transform child in slot.transform)
                Destroy(child.gameObject);
        }

        GenerateAll();
    }
}