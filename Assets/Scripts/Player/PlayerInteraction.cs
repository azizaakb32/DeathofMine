using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;
    private Interactable currentInteractable;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    ObjectiveUIManager.Instance.ShowInteraction(currentInteractable.interactionName);
                }

                // MANA SHU YERGA O'ZGARTIRISH KIRITILDI:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();
                    currentInteractable = null; // Fuse kabi Destroy bo'ladigan obyektlar uchun tozalash
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable = null;
                ObjectiveUIManager.Instance.HideInteraction();
                ObjectiveUIManager.Instance.HideGeneratorInfo();
            }
        }
    }
}