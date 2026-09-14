using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScreamer : MonoBehaviour
{
    public AudioSource audioSource;
    public float delay = 0f;
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;
        Invoke(nameof(PlaySound), delay);
    }

    void PlaySound()
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f); // har safar biroz farqli eshitilsin
        audioSource.Play();
    }
}
