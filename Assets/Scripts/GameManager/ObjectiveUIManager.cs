using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectiveUIManager : MonoBehaviour
{
    public static ObjectiveUIManager Instance;

    [Header("Panels")]
    public GameObject exitTriggerPanel;
    public GameObject interactionPanel;
    public GameObject generatorPanel;
    public GameObject reminderPanel;
    public GameObject posterPanel;

    [Header("Texts")]
    public TMP_Text exitTriggerText;
    public TMP_Text interactionText;
    public TMP_Text generatorText;
    public TMP_Text reminderText;
    public TMP_Text posterCodeText;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    [Header("Audio")]
    public AudioSource typingAudio;
    public AudioClip typingClip;
    private Coroutine typingRoutine;

    // Stores the generated poster code persistently
    private string currentSavedCode = "";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Close all panels at the start of the game
        if (exitTriggerPanel != null) exitTriggerPanel.SetActive(false);
        if (interactionPanel != null) interactionPanel.SetActive(false);
        if (generatorPanel != null) generatorPanel.SetActive(false);
        if (reminderPanel != null) reminderPanel.SetActive(false);
        if (posterPanel != null) posterPanel.SetActive(false);
    }

    #region Exit & Main Objective Panel
    public void ShowExitObjective(string message)
    {
        if (exitTriggerPanel != null) exitTriggerPanel.SetActive(true);
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);
        typingRoutine = StartCoroutine(TypeMessage(exitTriggerText, message));
    }

    public void HideExitObjective()
    {
        if (exitTriggerPanel != null) exitTriggerPanel.SetActive(false);
        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine);
            typingRoutine = null;
        }
        if (typingAudio != null && typingAudio.isPlaying)
        {
            typingAudio.Stop();
        }
    }
    #endregion

    #region Interaction (E key)
    public void ShowInteraction(string action)
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(true);
            if (interactionText != null)
                interactionText.text = $"Press <color=yellow><b>E</b></color> to {action}";
        }
    }

    public void HideInteraction()
    {
        if (interactionPanel != null) interactionPanel.SetActive(false);
    }
    #endregion

    #region Generator Info
    public void ShowGeneratorInfo(int currentFuse, int requiredFuse)
    {
        if (generatorPanel != null) generatorPanel.SetActive(true);
        UpdateFuseUI(currentFuse, requiredFuse);
    }

    public void UpdateFuseUI(int currentFuse, int requiredFuse)
    {
        if (generatorText != null)
            generatorText.text = $"<color=red>GENERATOR OFFLINE</color>\n\nThe generator requires {requiredFuse} Fuses.\n\nFuses Found: <color=yellow>{currentFuse}/{requiredFuse}</color>";
    }

    public void HideGeneratorInfo()
    {
        if (generatorPanel != null) generatorPanel.SetActive(false);
    }
    #endregion

    #region Dynamic Reminder
    public void SetReminder(string reminder)
    {
        if (reminderPanel != null) reminderPanel.SetActive(true);
        if (reminderText != null) reminderText.text = reminder;
    }

    public void HideReminder()
    {
        if (reminderPanel != null) reminderPanel.SetActive(false);
    }
    #endregion

    #region Poster Panel
    // Sets the generator-created code as soon as the game starts
    public void SetPosterCode(string code)
    {
        currentSavedCode = code;

        if (posterCodeText != null)
        {
            posterCodeText.text = currentSavedCode;
            // Force the mesh to update so it renders correctly even while the panel is inactive
            posterCodeText.ForceMeshUpdate();
        }
    }

    public void ShowPoster()
    {
        if (posterPanel != null)
        {
            posterPanel.SetActive(true);

            // Sync the displayed text with the saved code when the panel opens
            if (posterCodeText != null && !string.IsNullOrEmpty(currentSavedCode))
            {
                posterCodeText.text = currentSavedCode;
            }
        }
    }

    public void HidePoster()
    {
        if (posterPanel != null)
        {
            posterPanel.SetActive(false);
        }
    }
    #endregion

    #region Typing Effect Coroutine
    IEnumerator TypeMessage(TMP_Text textUI, string message)
    {
        if (textUI == null) yield break;

        textUI.text = "";
        if (typingAudio != null && typingClip != null)
        {
            typingAudio.clip = typingClip;
            typingAudio.loop = false;
            typingAudio.volume = 0.5f;
            typingAudio.Play();
        }

        foreach (char c in message)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (typingAudio != null && typingAudio.isPlaying)
        {
            typingAudio.Stop();
        }
        typingRoutine = null;
    }
    #endregion
}
