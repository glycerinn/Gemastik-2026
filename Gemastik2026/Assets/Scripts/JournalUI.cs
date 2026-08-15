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
    public RectTransform descriptionScrollView;

    [Header("Description Positions")]
    public float positionWithImage = -60f;
    public float positionWithoutImage = 40f;

    void Start()
    {
        foreach (JournalEntrySO entry in entries)
        {
            GameObject obj = Instantiate(buttonPrefab, buttonParent);

            JournalButton button = obj.GetComponent<JournalButton>();

            button.Initialize(entry, this);
        }

        if (entries.Count > 0)
            DisplayEntry(entries[0]);
    }

    public void DisplayEntry(JournalEntrySO entry)
    {
        titleText.text = entry.title;
        descriptionText.text = entry.description;

        if (entry.image != null)
        {
            // Show image
            picture.gameObject.SetActive(true);
            picture.sprite = entry.image;

            // Keep description lower
            SetDescriptionPosition(positionWithImage);
        }
        else
        {
            // Hide image completely
            picture.gameObject.SetActive(false);

            // Move description upward
            SetDescriptionPosition(positionWithoutImage);
        }
    }

    private void SetDescriptionPosition(float y)
    {
        Vector2 position = descriptionScrollView.anchoredPosition;
        position.y = y;
        descriptionScrollView.anchoredPosition = position;
    }
}