using UnityEngine;
using System.Collections;

public class CameraShaking : MonoBehaviour
{
    public static CameraShaking Instance;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        Instance = this;
    }

    public void Shake(float duration = 0.3f, float magnitude = 0.15f)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(DoShake(duration, magnitude));
    }

    IEnumerator DoShake(float duration, float magnitude)
    {
        originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
