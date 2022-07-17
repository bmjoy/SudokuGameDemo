using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PlayerData : MonoBehaviour
{
    Color32 WrongColor = new Color32(0, 0, 0, 0);
    public static PlayerData instance;
    private void Awake()
    {
        instance = this;
    }
    public void SaveData()
    {
        PlayerPrefsExtra.SetColor("correctNumberInCell" , Settings.instance.correctNumberInCell);
        PlayerPrefsExtra.SetColor("wrongNumberInCell", Settings.instance.wrongNumberInCell);
        PlayerPrefsExtra.SetColor("sameNumberHighlight", Settings.instance.sameNumberHighlight);
        PlayerPrefsExtra.SetColor("ReadonlyFrameColor", Settings.instance.ReadonlyFrameColor);
        PlayerPrefsExtra.SetColor("ImportantMessageColor", Settings.instance.ImportantMessageColor);
        PlayerPrefsExtra.SetColor("NotimportantMessageColor", Settings.instance.NotimportantMessageColor);

        // orginalne wypelnione sudoku
        List<int> OriginalSolution = Sudoku.instance.ConvertBoardToListOfInt(Sudoku.instance.originalSolution, Sudoku.instance.OriginalSolutionList);

        // aktualny stan sudoku wlaczajac to to co zrobil gracz
        List<int> CurrentBoard = Sudoku.instance.ConvertListOf_IF_ToIntList(Sudoku.instance.textList, Sudoku.instance.CurrentBoard);

        // plansza po pierwotnym wyzerowaniu
        List<int> OriginalEmptiedBoard = Sudoku.instance.ConvertBoardToListOfInt(Sudoku.instance.SudokuBoard, Sudoku.instance.BackUpList);

        PlayerPrefsExtra.SetList("ReadOnlyCells", Sudoku.instance.ReadOnlyList);

        PlayerPrefsExtra.SetList("CurrentBoard", CurrentBoard);
        PlayerPrefsExtra.SetList("OriginalSolution", OriginalSolution);
        PlayerPrefsExtra.SetList("OriginalEmptiedBoard", OriginalEmptiedBoard);

        PlayerPrefs.SetFloat("TimeToCountDown", Settings.instance.TimeToCountDown);
        PlayerPrefs.SetFloat("TimeToCountUp", Settings.instance.TimeToCountUp);

        Application.Quit();
    }

    public void LoadData()
    {
       

        Color32 corrNumInCell = PlayerPrefsExtra.GetColor("correctNumberInCell");
        Color32 wrongNumInCell = PlayerPrefsExtra.GetColor("wrongNumberInCell");
        Color32 sameNumHL = PlayerPrefsExtra.GetColor("sameNumberHighlight");
        Color32 readOnlyFC = PlayerPrefsExtra.GetColor("ReadonlyFrameColor");
        Color32 ImpMessage = PlayerPrefsExtra.GetColor("ImportantMessageColor");
        Color32 notImpMessage = PlayerPrefsExtra.GetColor("NotimportantMessageColor");

        if (!TMPro_ExtensionMethods.Compare(corrNumInCell, WrongColor))
        {
            Settings.instance.correctNumberInCell = corrNumInCell;
        }

        if (!TMPro_ExtensionMethods.Compare(wrongNumInCell, WrongColor))
        {
            Settings.instance.wrongNumberInCell = wrongNumInCell;
        }

        if (!TMPro_ExtensionMethods.Compare(sameNumHL, WrongColor))
        {
            Settings.instance.sameNumberHighlight = sameNumHL;
        }

        if (!TMPro_ExtensionMethods.Compare(readOnlyFC, WrongColor))
        {
            Settings.instance.ReadonlyFrameColor = readOnlyFC;
        }

        if (!TMPro_ExtensionMethods.Compare(ImpMessage, WrongColor))
        {
            Settings.instance.ImportantMessageColor = ImpMessage;
        }

        if (!TMPro_ExtensionMethods.Compare(notImpMessage, WrongColor))
        {
            Settings.instance.NotimportantMessageColor = notImpMessage;
        }


        float timeDown = PlayerPrefs.GetFloat("TimeToCountDown");
        float timeUp = PlayerPrefs.GetFloat("TimeToCountUp");

        if (timeDown > 0)
        {
            Settings.instance.TimeToCountDown = timeDown;
        }
        if (timeUp > 0)
        {
            Settings.instance.TimeToCountUp = timeUp;
        }

    

        Sudoku.instance.ReadOnlyList = PlayerPrefsExtra.GetList<TMP_InputField>("ReadOnlyCells").ToList();

        int[,] board_emptied = Sudoku.instance.ConvertListToIntArray(PlayerPrefsExtra.GetList<int>("OriginalEmptiedBoard"), Sudoku.instance.SudokuBoard);

        int[,] original_solution = Sudoku.instance.ConvertListToIntArray(PlayerPrefsExtra.GetList<int>("OriginalSolution"), Sudoku.instance.originalSolution);

        int[,] current_board = Sudoku.instance.ConvertListToIntArray(PlayerPrefsExtra.GetList<int>("CurrentBoard"), Sudoku.instance.currentBoard_array);

        Array.Copy(board_emptied, Sudoku.instance.SudokuBoard, board_emptied.Length);

        Array.Copy(original_solution, Sudoku.instance.originalSolution, original_solution.Length);

        Array.Copy(current_board, Sudoku.instance.backupBoard, current_board.Length);

      
    }

  
}
