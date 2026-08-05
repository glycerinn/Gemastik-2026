using UnityEngine;

[CreateAssetMenu(menuName = "Journal/Entry")]
public class JournalEntrySO : ScriptableObject
{
    public string title;

    [TextArea(5,10)]
    public string description;

    public Sprite image;
}