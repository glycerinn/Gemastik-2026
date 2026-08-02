using UnityEngine;
using UnityEngine.UI;

public class PieSlice : MonoBehaviour
{
    public Image image;

    public void SetSlice(float percentage, float startAngle)
    {
        image.fillAmount = percentage;

        transform.localEulerAngles = new Vector3(0, 0, startAngle);
    }
}