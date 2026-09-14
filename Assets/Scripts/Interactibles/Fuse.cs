using UnityEngine;

public class Fuse : Interactable
{
    private void Start()
    {
        interactionName = "Pick up Fuse";
    }

    public override void Interact()
    {
        GameManager.Instance.FindFuse();
        ObjectiveUIManager.Instance.HideInteraction();
        Destroy(gameObject);
    }
}