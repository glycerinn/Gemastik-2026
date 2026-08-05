using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalButton : MonoBehaviour
{
    public TMP_Text titleText;

    JournalEntrySO entry;
    JournalUI ui;

    public void Initialize(JournalEntrySO newEntry, JournalUI journal)
    {
        entry = newEntry;
        ui = journal;

        titleText.text = entry.title;

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        ui.DisplayEntry(entry);
    }
}