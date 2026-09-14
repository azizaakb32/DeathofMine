using UnityEngine;

public class Generator : Interactable
{
    private void Start()
    {
        // Ekranning pastida chiquvchi matn
        interactionName = "Start Generator";
    }

    public override void Interact()
    {
        // E tugmasi bosilganda GameManager-ga xabar beramiz
        GameManager.Instance.OnInteractWithGenerator();
    }
}