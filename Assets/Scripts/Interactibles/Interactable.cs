using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("UI")]
    public string interactionName = "Interact";

    public virtual void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);
    }
}