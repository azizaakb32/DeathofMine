using UnityEngine;

public class KeycardItem : Interactable
{
    public GameObject enemyObject; // Inspector'dan dushman obyektini bog'lang

    private void Start()
    {
        interactionName = "Pick up Keycard";
    }

    public override void Interact()
    {
        if (GameManager.Instance.currentStep == GameManager.GameStep.Panel02Active)
        {
            GameManager.Instance.OnKeycardCollected();
            ObjectiveUIManager.Instance.HideInteraction();

            if (enemyObject != null)
                enemyObject.SetActive(true); // dushmanni ishga tushirish

            Destroy(gameObject); // Olinganidan keyin xonadan yo'qoladi
        }
    }
}
