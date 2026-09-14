using UnityEngine;

public class GhostScreamer : MonoBehaviour
{
    public GameObject ghostModel;
    public AudioSource whisperSound; // ixtiyoriy, past ovoz
    public float visibleDuration = 1.2f;
    public float delay = 0f;
    private bool triggered = false;

    void Start()
    {
        ghostModel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;
        Invoke(nameof(ShowGhost), delay);
    }

    void ShowGhost()
    {
        ghostModel.SetActive(true);
        if (whisperSound != null) whisperSound.Play();
        Invoke(nameof(HideGhost), visibleDuration);
    }

    void HideGhost()
    {
        ghostModel.SetActive(false);
    }
}
