using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameStep
    {
        FindExit,
        BlockedExitSeen,
        NeedFuses,
        GeneratorActive,
        Panel01Active,
        Panel02Active,
        HasKeycard,
        Escaped
    }

    public GameStep currentStep = GameStep.FindExit;

    [Header("Game Balances")]
    public int totalFusesRequired = 3;
    public int currentFusesFound = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void StartGameObjectives()
    {
        ObjectiveUIManager.Instance.SetReminder("Objective: Find a way out of the mine.");
    }

    public void OnReachBlockedExit()
    {
        if (currentStep == GameStep.FindExit)
        {
            currentStep = GameStep.BlockedExitSeen;

            string mainTaskText = "EXIT BLOCKED\n\n" +
                                  "The exit is blocked by fallen rocks.\n" +
                                  "To escape the mine, you must open the emergency exit.\n\n" +
                                  "Tasks:\n" +
                                  "1. Start the Generator\n" +
                                  "2. Activate Panel 01\n" +
                                  "3. Activate Panel 02\n" +
                                  "4. Find the Keycard\n" +
                                  "5. Open the Exit Door and Escape";

            ObjectiveUIManager.Instance.ShowExitObjective(mainTaskText);
            ObjectiveUIManager.Instance.SetReminder("Objective: Start the Generator.");
        }
    }

    public void OnInteractWithGenerator()
    {
        // CASE 1: Player saw the blocked exit and is interacting with the generator for the first time
        if (currentStep == GameStep.BlockedExitSeen)
        {
            currentStep = GameStep.NeedFuses;
            ObjectiveUIManager.Instance.ShowGeneratorInfo(currentFusesFound, totalFusesRequired);
            return; // Stop here, don't fall through to the next condition
        }

        // CASE 2: Player already knows the objective and is interacting with the generator again
        if (currentStep == GameStep.NeedFuses)
        {
            if (currentFusesFound >= totalFusesRequired)
            {
                currentStep = GameStep.GeneratorActive;
                ObjectiveUIManager.Instance.HideGeneratorInfo();
                ObjectiveUIManager.Instance.SetReminder("Objective: Activate Panel 01.");
            }
            else
            {
                ObjectiveUIManager.Instance.ShowGeneratorInfo(currentFusesFound, totalFusesRequired);
            }
        }
    }

    public void FindFuse()
    {
        currentFusesFound++;

        ObjectiveUIManager.Instance.UpdateFuseUI(currentFusesFound, totalFusesRequired);

        if (currentStep == GameStep.NeedFuses)
        {
            ObjectiveUIManager.Instance.SetReminder($"Objective: Start the Generator ({currentFusesFound}/{totalFusesRequired} Fuses).");
        }
    }

    public void OnPanel01Activated()
    {
        if (currentStep == GameStep.GeneratorActive)
        {
            currentStep = GameStep.Panel01Active;
            ObjectiveUIManager.Instance.SetReminder("Objective: Activate Panel 02 (Door opened).");
        }
    }

    public void OnPanel02Activated()
    {
        if (currentStep == GameStep.Panel01Active)
        {
            currentStep = GameStep.Panel02Active;
            ObjectiveUIManager.Instance.SetReminder("Objective: Find the Keycard.");
        }
    }

    public void OnKeycardCollected()
    {
        if (currentStep == GameStep.Panel02Active)
        {
            currentStep = GameStep.HasKeycard;
            ObjectiveUIManager.Instance.SetReminder("Objective: Escape through the Emergency Exit!");
        }
    }

    public void OnEscape()
    {
        if (currentStep == GameStep.HasKeycard)
        {
            currentStep = GameStep.Escaped;

            if (ObjectiveUIManager.Instance != null)
            {
                ObjectiveUIManager.Instance.HideReminder();
                ObjectiveUIManager.Instance.HideInteraction();
            }

            MenuManager menuManager = FindFirstObjectByType<MenuManager>();
            if (menuManager != null)
            {
                menuManager.ShowEscaped();
            }
        }
    }
}