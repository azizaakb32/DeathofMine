using UnityEngine;

public class PanelOne : Interactable
{
    public GameObject puzzleCanvas; // SlidePuzzle canvas shu yerga biriktiriladi

    private void Start()
    {
        interactionName = "Activate Panel 01";
    }

    public override void Interact()
    {
        if (GameManager.Instance.currentStep == GameManager.GameStep.GeneratorActive)
        {
            puzzleCanvas.SetActive(true);
            ObjectiveUIManager.Instance.HideInteraction();
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
