using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputCodeManage : MonoBehaviour
{
    [Header("Kod")]
    public string correctCode = "";
    private string currentInput = "";
    public int maxDigits = 6;

    [Header("UI Referenslar")]
    public GameObject codePanel;
    public TMP_Text codeText;
    public Button[] numberButtons;
    public Button clearButton;
    public Button enterButton;
    public Button cancelButton; // YANGI: Paneldan chiqish (X yoki Back) tugmasi

    [Header("Bog'liq tizimlar")]
    public InputScreamer screamer;

    private int wrongAttempts = 0;

    private void Awake()
    {
        for (int i = 0; i < numberButtons.Length; i++)
        {
            int digit = i;
            numberButtons[i].onClick.RemoveAllListeners();
            numberButtons[i].onClick.AddListener(() => AddDigit(digit));
        }

        if (clearButton != null)
        {
            clearButton.onClick.RemoveAllListeners();
            clearButton.onClick.AddListener(ClearInput);
        }

        if (enterButton != null)
        {
            enterButton.onClick.RemoveAllListeners();
            enterButton.onClick.AddListener(SubmitCode);
        }

        // YANGI: Bekor qilish tugmasiga listener ulaymiz
        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(ClosePanel);
        }
    }

    private void Update()
    {
        // YANGI: Panel ochiq bo'lganida ESC tugmasi bosilsa panelni yopamiz
        if (codePanel != null && codePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePanel();
            }
        }
    }

    public void SetNewCode(string newCode)
    {
        correctCode = newCode;
        if (ObjectiveUIManager.Instance != null)
        {
            ObjectiveUIManager.Instance.SetPosterCode(correctCode);
        }
    }

    private void OnEnable()
    {
        ClearInput();
    }

    public void AddDigit(int digit)
    {
        if (currentInput.Length >= maxDigits) return;
        currentInput += digit.ToString();
        UpdateDisplay();
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (codeText != null)
        {
            codeText.text = currentInput;
        }
    }

    public void SubmitCode()
    {
        if (currentInput == correctCode)
        {
            OnCorrectCode();
        }
        else
        {
            OnWrongCode();
        }
    }

    void OnCorrectCode()
    {
        ClosePanel();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPanel01Activated();
        }
    }

    void OnWrongCode()
    {
        wrongAttempts++;
        ClearInput();

        if (screamer != null)
        {
            screamer.TriggerScreamer(wrongAttempts);
        }
    }

    // Panelni yopish va o'yinchiga harakatlanish imkonini qaytarish
    public void ClosePanel()
    {
        if (codePanel != null)
        {
            codePanel.SetActive(false);
        }

        ClearInput(); // Har ehtimolga qarshi yopilganda kiritilgan chala raqamlar tozalanadi

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
