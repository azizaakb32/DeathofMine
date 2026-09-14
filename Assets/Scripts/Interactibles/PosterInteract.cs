using UnityEngine;
public class PosterInteract : Interactable
{
    [Header("Poster sozlamalari")]
    public float viewAngleThreshold = 60f; // shu burchakdan oshsa - yopiladi
    private Camera playerCamera;
    private bool isPosterOpen = false;

    private void Start()
    {
        interactionName = "Afishani ko'rish";
        playerCamera = Camera.main;
    }

    public override void Interact()
    {
        ObjectiveUIManager.Instance.HideInteraction();
        ObjectiveUIManager.Instance.ShowPoster();
        isPosterOpen = true;
    }

    private void Update()
    {
        if (!isPosterOpen) return;

        Vector3 dirToPoster = (transform.position - playerCamera.transform.position).normalized;
        float angle = Vector3.Angle(playerCamera.transform.forward, dirToPoster);

        if (angle > viewAngleThreshold)
        {
            ClosePoster();
        }
    }

    private void ClosePoster()
    {
        isPosterOpen = false;
        ObjectiveUIManager.Instance.HidePoster();
    }
}
