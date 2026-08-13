using UnityEngine;

[CreateAssetMenu(menuName = "Nutrition/Food")]
public class FoodSO : ScriptableObject
{
    public string foodName;
    public Sprite icon;
    public Sprite platedIcon;

    public FoodCategory category;
}