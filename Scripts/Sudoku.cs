using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class Sudoku : MonoBehaviour
{
    public static Sudoku instance;

    public int[,] SudokuBoard = new int[9, 9];
    public int[,] backupBoard = new int[9, 9];
    public int[,] originalSolution = new int[9, 9];
    public int[,] currentBoard_array = new int[9,9];

    public List<int> CurrentBoard;
    public List<int> OriginalSolutionList;
    public List<int> BackUpList;

    public  List<TMP_InputField> textList = new List<TMP_InputField>();

    public TMP_InputField Difficulty;

    public List<int> allowedNumbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    public List<int> allowedNumbersReversed = new List<int>() { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
    public List<int> allowedNumbersRandomized = new List<int>() { 9, 8, 7, 6, 5, 4, 3, 2, 1 };

    public int current_x;
    public int current_y;

    public int currentCell = -1;

    public bool HighlightNumbersCurrentlyAllowed = true;

    public List<TMP_InputField> ReadOnlyList = new List<TMP_InputField>();
    public string currentPositionValue;

    private SudokuSolver solver = new SudokuSolver();


    private void Awake()
    {
        instance = this;
        Shuffle.ShuffleList(allowedNumbersRandomized);
    }

    private void Start()
    {
        try
        {
            PlayerData.instance.LoadData();
            MessageManager.instance.PrintMessage("Loading Complete", MessageType.important);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
            MessageManager.instance.PrintMessage($"Error while loading : {e}", MessageType.important);

        }

        LoadListFromArray(currentBoard_array, textList);

        SetReadOnly();
        SwitchFrame();
    }
    private void Update()
    {
        HandleInput();
        if (currentCell != -1)
        {
            textList[currentCell].Select();
        }

    }
    public void SaveListToArray(int[,] board, List<TMP_InputField> textList)
    {
        int k = 0;
        int number;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (textList[k].text == "")
                {
                    number = 0;
                }
                else
                {
                    number = int.Parse(textList[k].text);             
                }
                board[i, j] = number;
                k++;
            }
        }
    }
    public void LoadListFromArray(int[,] board, List<TMP_InputField> textList)
    {
        int k = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                textList[k].text = board[i, j].ToString();
                k++;
            }
        }
    }
    public void ClearBoard()
    {
      
        foreach (TMP_InputField a in textList)
        {
            OneCell oneCell = a.GetComponent<OneCell>();
            a.text = "0";
            oneCell.DisableHighlight();
            a.textComponent.color = Settings.instance.defaultNumbersColor;
            a.readOnly = false;
            oneCell.isCurrentlyWrong = false;
            oneCell.ReadOnly.gameObject.SetActive(false);
            oneCell.Frame.color = Settings.instance.defaultCellFrameColor;
        }
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                SudokuBoard[i, j] = 0;
                originalSolution[i, j] = 0;
                backupBoard[i, j] = 0;
            }
        }
        ReadOnlyList.Clear(); 
        SaveListToArray(SudokuBoard, textList);
    }

   
   
  

    public void GenerateButton()
    {
        Stopwatch st = new Stopwatch();

        HighlightNumbersCurrentlyAllowed = false;

        ClearBoard();

        st.Start();

        solver.GenerateSudoku(SudokuBoard);
        Array.Copy(SudokuBoard, originalSolution, SudokuBoard.Length);

        int i = int.Parse(Difficulty.text);

        if (i <= 50 && i > 0)
        {            
            solver.RemoveNumbers(SudokuBoard, originalSolution, i);            
                          
            LoadListFromArray(SudokuBoard, textList);
            GetReadOnly();
            SetReadOnly();
            SwitchFrame();
        }
        else
        {
            MessageManager.instance.PrintMessage("Forbidden amount of empty spaces !", MessageType.important);
            UnityEngine.Debug.LogWarning("Forbidden amount of empty spaces !");  
        }
        HighlightNumbersCurrentlyAllowed = true;

        st.Stop();

        MessageManager.instance.PrintMessage($"Sudoku generated in : {st.ElapsedMilliseconds} ms", MessageType.not_important);

    }

    public void SolveButton()
    {
        if (!solver.CheckIfBoardIsEmpty(SudokuBoard))
        {
            HighlightNumbersCurrentlyAllowed = false;
            ClearCollors();
            ClearWrongnumbers();
            solver.SolveSudoku(SudokuBoard, allowedNumbers);
            LoadListFromArray(SudokuBoard, textList);
            HighlightNumbersCurrentlyAllowed = true;
        }
        else
        {
            MessageManager.instance.PrintMessage("Board is empty !", MessageType.important);
            UnityEngine.Debug.LogWarning("Board is empty !");
        }
    }

    public void CheckBoardButton()
    {
        if (!solver.CheckIfBoardIsEmpty(SudokuBoard))
        {
            SaveListToArray(backupBoard, textList);
            CheckBoard(backupBoard, textList);
        }
        else
        {
            MessageManager.instance.PrintMessage("Board is empty !", MessageType.important);
            UnityEngine.Debug.LogWarning("Board is empty !");
        }
        
    }

    private void GetReadOnly()
    {
        foreach (TMP_InputField a in textList)
        {
            if (a.text != "0")
            {
                ReadOnlyList.Add(a);
                
                
            }
        }
    }

    private void SetReadOnly()
    {
        foreach (TMP_InputField a in ReadOnlyList)
        {
            a.readOnly = true;
        }
    }

    public void SwitchFrame()
    {
        if (ReadOnlyList != null)
        {
            foreach (TMP_InputField a in ReadOnlyList)
            {
                if (Settings.instance.AllowFrameColorChanges)
                {
                    a.GetComponent<OneCell>().ReadOnly.gameObject.SetActive(true);
                    a.GetComponent<OneCell>().Frame.color = Settings.instance.ReadonlyFrameColor;
                }
                else
                {
                    a.GetComponent<OneCell>().ReadOnly.gameObject.SetActive(false);
                    a.GetComponent<OneCell>().Frame.color = Settings.instance.defaultCellFrameColor;
                }
            }
        }
       
    }
    
    public void ClearCollors()
    {
        foreach (TMP_InputField a in textList)
        {          
            a.GetComponent<OneCell>().DisableHighlight();
            a.textComponent.color = Settings.instance.defaultNumbersColor;
            a.GetComponent<OneCell>().OnDiselectNeighbor();
          
        }
    }
    public void ClearSameNumbers()
    {
        foreach (TMP_InputField a in textList)
        {       
            a.textComponent.color = Settings.instance.defaultNumbersColor;
        }
    }
    public void ClearWrongnumbers()
    {
        foreach (TMP_InputField a in textList)
        {
            a.GetComponent<OneCell>().isCurrentlyWrong = false;
        }
    }

    public void HighLightSameNumbers(string numberValue)
    {
        foreach (TMP_InputField a in textList)
        {
            if (a.text == numberValue && HighlightNumbersCurrentlyAllowed)
            {            
                a.textComponent.color = Settings.instance.sameNumberHighlight;
            }               
        }    
    }

    public void HighlightRowAndCol(int y, int x)
    {
        int k = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (i == y && j == x)
                {
                    textList[k].GetComponent<OneCell>().OnDiselectNeighbor();
                }
                else if (i == y || j == x)
                {
                    textList[k].GetComponent<OneCell>().OnSelectNeighbor();           
                }
                else
                {
                    textList[k].GetComponent<OneCell>().OnDiselectNeighbor();
                }
                k++;
            }
        }
        
    }

    public void CheckBoard(int[,] board, List<TMP_InputField> textList)
    {
       
        int k = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
             
                if (board[i, j] == originalSolution[i,j]  && !textList[k].readOnly)
                {
                    textList[k].GetComponent<OneCell>().OnCorrectCell();
                }
                else if (textList[k].readOnly || textList[k].text == "0")
                {
                    textList[k].GetComponent<OneCell>().DisableHighlight();
                }
                else 
                {
                    textList[k].GetComponent<OneCell>().OnIncorrectCell();
                }
                k++;
            }
        }   
    }
    public bool CheckIfPossibleNumber(int y, int x, int number)
    {
        SaveListToArray(backupBoard, textList);

        backupBoard[y, x] = 0;

        return solver.CheckAllConditions(y, x, number, backupBoard);
       
    }

    public void HandleInput()
    {
        if (currentCell != -1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (currentCell - 1 < 0)
                {
                    currentCell = 80;
                }
                else
                {
                    currentCell--;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (currentCell + 1 > 80)
                {
                    currentCell = 0;
                }
                else
                {
                    currentCell++;
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (currentCell - 9 < 0)
                {
                    currentCell += 72;
                }
                else
                {
                    currentCell -= 9;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                if (currentCell + 9 > 80)
                {
                    currentCell -= 72;
                }
                else
                {
                    currentCell += 9;
                }
            
            }
        }
    }

    
    public  bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeblock) // currently not used
    {
        try
        {
            Task task = Task.Factory.StartNew(() => codeblock());
            task.Wait(timeSpan);
            return task.IsCompleted;
        }
        catch (AggregateException ae)
        {
            MessageManager.instance.PrintMessage("Took too long. Trying again...", MessageType.important);
            throw ae.InnerExceptions[0];
        }
    }

    public List<int> ConvertBoardToListOfInt(int[,] board, List<int> list)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                list.Add(board[i, j]);
            }
        }
        return list;
    }

    public List<int> ConvertListOf_IF_ToIntList(List<TMP_InputField> IF_list, List<int> int_list)
    {
        foreach (TMP_InputField a in IF_list)
        {
            int_list.Add(int.Parse(a.text));
        }

        return int_list;
    }
   
    public int[,] ConvertListToIntArray(List<int> list, int[,] array)
    {
        int k = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                array[i, j] = list[k];
                k++;
            }
        }
       return array;
    }
  
}

