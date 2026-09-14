using UnityEngine;

public class PosterCodeGenerate : MonoBehaviour
{
    [Header("Linked Scripts")]
    public InputCodeManage inputCodeManager;

    private void Start()
    {
        // Generate a new code as soon as the game starts
        GenerateNewCode();
    }

    public void GenerateNewCode()
    {
        if (inputCodeManager == null)
        {
            return;
        }

        // Generate a random code
        string newCode = GenerateRandomCode(inputCodeManager.maxDigits);

        // Send the new code to InputCodeManager
        inputCodeManager.SetNewCode(newCode);
    }

    private string GenerateRandomCode(int length)
    {
        System.Text.StringBuilder codeBuilder = new System.Text.StringBuilder();
        for (int i = 0; i < length; i++)
        {
            codeBuilder.Append(Random.Range(0, 10));
        }
        return codeBuilder.ToString();
    }
}
