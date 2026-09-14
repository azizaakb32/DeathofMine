using UnityEngine;

public class EmergencyExitDoor : Interactable
{
    private void Start()
    {
        interactionName = "Open Emergency Exit";
    }

    public override void Interact()
    {
        if (GameManager.Instance.currentStep == GameManager.GameStep.HasKeycard)
        {
            GameManager.Instance.OnEscape();
        }
        else
        {
            Debug.Log("Eshik qulflangan. Keycard kerak!");
        }
    }
}