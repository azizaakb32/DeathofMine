using UnityEngine;

public class PanelTwo : Interactable
{
    public GameObject puzzleCanvas; // SudokuCanvas (fon + grid) shu yerga biriktiriladi

    private void Start()
    {
        interactionName = "Activate Panel 02";
    }

    public override void Interact()
    {
        if (GameManager.Instance.currentStep == GameManager.GameStep.Panel01Active)
        {
            puzzleCanvas.SetActive(true);
            ObjectiveUIManager.Instance.HideInteraction();
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
