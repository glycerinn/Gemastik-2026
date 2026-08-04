using TMPro;
using UnityEngine;

public class DayCounterUI : MonoBehaviour
{
    public TMP_Text dayText;

    void Update()
    {
        dayText.text = "Day " + DayManager.Instance.currentDay;
    }
}