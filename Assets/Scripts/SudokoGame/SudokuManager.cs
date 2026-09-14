using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SudokuManager : MonoBehaviour
{
    public GameObject puzzleCanvas;
    public TMP_InputField[] cells = new TMP_InputField[16]; // 4x4 = 16 katak
    public TMP_Text feedbackText;

    [Header("Qiyinlik")]
    public int cellsToRemove = 4; // Nechta katak bo'sh qoldirilsin (16 tadan)

    private int[] puzzle = new int[16];
    private int[] solution = new int[16];

    // Bazaviy (o'zgarmas) to'g'ri 4x4 sudoku yechimi — aralashtirish uchun asos
    private readonly int[] baseSolution = new int[16]
    {
        1, 2, 3, 4,
        3, 4, 1, 2,
        2, 1, 4, 3,
        4, 3, 2, 1
    };

    private void OnEnable()
    {
        GenerateNewPuzzle();
        SetupBoard();
    }

    // ------------------- Tasodifiy 4x4 sudoku generatsiya qilish -------------------
    private void GenerateNewPuzzle()
    {
        int[] grid = (int[])baseSolution.Clone();

        // 1) Qatorlarni "band" ichida aralashtirish (har band = 2 qator, jami 2 ta band)
        grid = ShuffleRowsWithinBands(grid);

        // 2) Ustunlarni "band" ichida aralashtirish (har band = 2 ustun, jami 2 ta band)
        grid = ShuffleColsWithinBands(grid);

        // 3) Qator-bandlarni o'zaro almashtirish
        grid = ShuffleRowBands(grid);

        // 4) Ustun-bandlarni o'zaro almashtirish
        grid = ShuffleColBands(grid);

        // 5) Raqamlarni qayta belgilash (1->4, 2->1 kabi tasodifiy almashtirish, 1 dan 4 gacha)
        grid = RemapNumbers(grid);

        solution = grid;

        // 6) Puzzle hosil qilish: tasodifiy kataklarni bo'sh (0) qilib qo'yish
        puzzle = (int[])solution.Clone();
        List<int> indexes = new List<int>();
        for (int i = 0; i < 16; i++) indexes.Add(i);

        for (int i = 0; i < cellsToRemove && indexes.Count > 0; i++)
        {
            int randPos = Random.Range(0, indexes.Count);
            puzzle[indexes[randPos]] = 0;
            indexes.RemoveAt(randPos);
        }
    }

    private int[] ShuffleRowsWithinBands(int[] grid)
    {
        int[] result = (int[])grid.Clone();
        for (int band = 0; band < 2; band++) // 2 ta band, har biri 2 qatordan
        {
            int r0 = band * 2;
            int r1 = band * 2 + 1;
            if (Random.Range(0, 2) == 0)
            {
                SwapRows(result, r0, r1);
            }
        }
        return result;
    }

    private int[] ShuffleColsWithinBands(int[] grid)
    {
        int[] result = (int[])grid.Clone();
        for (int band = 0; band < 2; band++) // 2 ta band, har biri 2 ustundan
        {
            int c0 = band * 2;
            int c1 = band * 2 + 1;
            if (Random.Range(0, 2) == 0)
            {
                SwapColsByColIndex(result, c0, c1);
            }
        }
        return result;
    }

    private int[] ShuffleRowBands(int[] grid)
    {
        if (Random.Range(0, 2) == 0) return grid;

        int[] result = (int[])grid.Clone();
        // 2 ta bandni (har biri 2 qator) o'zaro almashtirish
        for (int rr = 0; rr < 2; rr++)
        {
            int r0 = rr;
            int r1 = 2 + rr;
            SwapRows(result, r0, r1);
        }
        return result;
    }

    private int[] ShuffleColBands(int[] grid)
    {
        if (Random.Range(0, 2) == 0) return grid;

        int[] result = (int[])grid.Clone();
        for (int cc = 0; cc < 2; cc++)
        {
            int c0 = cc;
            int c1 = 2 + cc;
            SwapColsByColIndex(result, c0, c1);
        }
        return result;
    }

    private int[] RemapNumbers(int[] grid)
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4 };
        Shuffle(numbers);

        int[] result = new int[16];
        for (int i = 0; i < 16; i++)
        {
            result[i] = numbers[grid[i] - 1];
        }
        return result;
    }

    private void SwapRows(int[] grid, int r0, int r1)
    {
        for (int c = 0; c < 4; c++)
        {
            int temp = grid[r0 * 4 + c];
            grid[r0 * 4 + c] = grid[r1 * 4 + c];
            grid[r1 * 4 + c] = temp;
        }
    }

    private void SwapColsByColIndex(int[] grid, int c0, int c1)
    {
        for (int r = 0; r < 4; r++)
        {
            int temp = grid[r * 4 + c0];
            grid[r * 4 + c0] = grid[r * 4 + c1];
            grid[r * 4 + c1] = temp;
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    // ------------------------------------------------------------------------------

    private void SetupBoard()
    {
        for (int i = 0; i < 16; i++)
        {
            var cell = cells[i];
            cell.text = "";
            cell.characterLimit = 1;
            cell.contentType = TMP_InputField.ContentType.IntegerNumber;

            if (puzzle[i] != 0)
            {
                cell.text = puzzle[i].ToString();
                cell.interactable = false;
            }
            else
            {
                cell.text = "";
                cell.interactable = true;
            }
        }

        if (feedbackText != null)
            feedbackText.text = "";
    }

    public void OnCheckButtonPressed()
    {
        for (int i = 0; i < 16; i++)
        {
            if (!int.TryParse(cells[i].text, out int value) || value != solution[i])
            {
                feedbackText.text = "Incorrect. Try again.";
                return;
            }
        }

        feedbackText.text = "Access Granted!";
        StartCoroutine(CompletePuzzle());
    }

    private IEnumerator CompletePuzzle()
    {
        yield return new WaitForSecondsRealtime(1f);

        puzzleCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.Instance.OnPanel02Activated();
    }
}