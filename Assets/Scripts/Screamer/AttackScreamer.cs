using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class AttackScreamer : MonoBehaviour
{
    public GameObject ghostModel;
    public AudioSource screamSound;
    public AudioSource subBassSound;
    public float delay = 0f;
    public float visibleDuration = 0.4f;
    private bool triggered = false;

    void Start()
    {
        ghostModel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;
        Invoke(nameof(Execute), delay);
    }

    void Execute()
    {
        ghostModel.SetActive(true);
        screamSound.Play();
        if (subBassSound != null) subBassSound.Play();
        if (CameraShaking.Instance != null) CameraShaking.Instance.Shake(0.3f, 0.15f);
        Invoke(nameof(Hide), visibleDuration);
    }

    void Hide()
    {
        ghostModel.SetActive(false);
    }
}
