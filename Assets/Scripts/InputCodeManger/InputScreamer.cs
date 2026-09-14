using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InputScreamer : MonoBehaviour
{
    [Header("Refs")]
    public AudioSource screamSound;
    public GameObject screamerImageUI;

    [Header("Sozlamalar")]
    public float screamerDuration = 0.4f;

    [Header("3+ xato bo'lganda ishga tushadigan voqea")]
    public UnityEvent onCriticalFailure; // Inspector'dan istalgan funksiyani biriktirasiz

    public void TriggerScreamer(int attemptCount)
    {
        Debug.Log("TriggerScreamer chaqirildi, urinish: " + attemptCount);
        StopAllCoroutines();
        StartCoroutine(ScreamerSequence(attemptCount));
    }

    IEnumerator ScreamerSequence(int attemptCount)
    {
        if (screamerImageUI != null) screamerImageUI.SetActive(true);
        if (screamSound != null) screamSound.Play();
        yield return new WaitForSecondsRealtime(screamerDuration);
        if (screamerImageUI != null) screamerImageUI.SetActive(false);

        if (attemptCount >= 3)
        {
            Debug.Log("3+ marta xato! Qo'shimcha xavf boshlanmoqda.");
            onCriticalFailure?.Invoke();
        }
    }
}
