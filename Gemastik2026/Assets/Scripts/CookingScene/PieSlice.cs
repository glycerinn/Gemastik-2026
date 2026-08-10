using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PieSlice : MonoBehaviour
{
    public Image image;
    private Coroutine transitionCoroutine;

    public void SetSlice(float targetPercentage, float targetAngle)
    {
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionRoutine(targetPercentage, targetAngle));
    }

    private IEnumerator TransitionRoutine(float targetPercentage, float targetAngle)
    {
        float startPercentage = image.fillAmount;
        float startAngleZ = transform.localEulerAngles.z;

        float time = 0;
        float duration = 0.5f; // Kecepatan diagram memutar/berubah

        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = Mathf.SmoothStep(0, 1, time / duration);

            image.fillAmount = Mathf.Lerp(startPercentage, targetPercentage, progress);

            // LerpAngle memastikan putaran diagram tidak berputar terbalik
            float currentAngle = Mathf.LerpAngle(startAngleZ, targetAngle, progress);
            transform.localEulerAngles = new Vector3(0, 0, currentAngle);

            yield return null;
        }

        image.fillAmount = targetPercentage;
        transform.localEulerAngles = new Vector3(0, 0, targetAngle);
    }
}