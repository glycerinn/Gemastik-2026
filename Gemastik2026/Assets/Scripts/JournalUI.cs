using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
    public List<JournalEntrySO> entries;

    public Transform buttonParent;
    public GameObject buttonPrefab;

    [Header("Right Page")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image picture;

    void Start()
    {
        foreach (JournalEntrySO entry in entries)
        {
            GameObject obj =
                Instantiate(buttonPrefab, buttonParent);

            JournalButton button =
                obj.GetComponent<JournalButton>();

            button.Initialize(entry, this);
        }

        if (entries.Count > 0)
            DisplayEntry(entries[0]);
    }

    public void DisplayEntry(JournalEntrySO entry)
    {
        titleText.text = entry.title;
        descriptionText.text = entry.description;
        picture.sprite = entry.image;
    }
}