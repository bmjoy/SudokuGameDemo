using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class SudokuSolver 
{
    public bool CheckRow(int y, int PotentialNumber, int[,] board)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[y, i] == PotentialNumber)
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckColumn(int x, int PotentialNumber, int[,] board)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[i, x] == PotentialNumber)
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckSquare(int y, int x, int PotentialNumber, int[,] board)
    {
        int square_x = x / 3 * 3;
        int sxuare_y = y / 3 * 3;

        for (int i = sxuare_y; i < sxuare_y + 3; i++)
        {
            for (int j = square_x; j < square_x + 3; j++)
            {

                if (board[i, j] == PotentialNumber)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public bool CheckAllConditions(int y, int x, int PotentialNumber, int[,] board)
    {
        if (CheckRow(y, PotentialNumber, board) && CheckColumn(x, PotentialNumber, board) && CheckSquare(y, x, PotentialNumber, board))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SolveOneCell(int y, int x, int[,] board, List<int> Numbers)
    {
        foreach (int n in Numbers)
        {
            if (CheckAllConditions(y, x, n, board))
            {
                board[y, x] = n;
                if (SolveSudoku(board, Numbers))
                {
                    return true;
                }
            }
        }
        board[y, x] = 0;
        return false;
    }
    public bool SolveSudoku(int[,] board, List<int> numbersToWrite)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] == 0)
                {
                    return SolveOneCell(i, j, board, numbersToWrite);
                }
            }
        }

        return true;
    }
    public bool GenerateSudoku(int[,] board)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] == 0)
                {
                    return FillOneCell(i, j, board);
                }
            }
        }
        return true;
    }
    public bool FillOneCell(int y, int x, int[,] board)
    {
        foreach (int n in Sudoku.instance.allowedNumbers)
        {
            int a = UnityEngine.Random.Range(0, 9);

            if (CheckAllConditions(y, x, Sudoku.instance.allowedNumbers[a], board))
            {
                board[y, x] = Sudoku.instance.allowedNumbers[a];
                if (GenerateSudoku(board))
                {
                    return true;
                }
            }
        }
        board[y, x] = 0;
        return false;
    }
    /*
    public bool AmountToZero(int[,] board, int amount)
    {
        int k = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] == 0)
                {
                    k++;
                }
            }
        }
        if (k == amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */

    public bool RemoveNumbers(int[,] board, int[,] originalBoard, int emptySpaces)
    {
        int[,] backup = new int[9, 9];

        while (!ZeroAmount(board, emptySpaces))
        {
            Array.Copy(board, backup, board.Length);

            int i, j;
            int number;
            i = UnityEngine.Random.Range(0, 9);
            j = UnityEngine.Random.Range(0, 9);

            number = board[i, j];
            backup[i, j] = 0;

            if (CheckNumberOfSolutions(backup))
            {
                board[i, j] = 0;
            }
            else
            {
                backup[i, j] = number;
            }

        }


        return true;
    }
    public bool ZeroAmount(int[,] board, int amount)
    {
        int k = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] == 0)
                {
                    k++;
                }
            }
        }
        if (k == amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckNumberOfSolutions(int[,] board)
    {
        int numberOfSolutions = 0;
        int[,] otherSolution = new int[9, 9];
        int[,] secondarSolution = new int[9, 9];
        int[,] originalSolution = new int[9, 9];
        int[,] originalBoard = new int[9, 9];

        Array.Copy(board, originalBoard, board.Length);
        while (numberOfSolutions < 4)
        {
            Array.Copy(originalBoard, board, board.Length);

            if (numberOfSolutions == 0)
            {
                SolveSudoku(board, Sudoku.instance.allowedNumbersRandomized);
                Array.Copy(board, originalSolution, board.Length);
            }

            else if (numberOfSolutions == 1)
            {
                SolveSudoku(board, Sudoku.instance.allowedNumbersReversed);
                Array.Copy(board, secondarSolution, board.Length);
            }

            else if (numberOfSolutions > 1)
            {
                SolveSudoku(board, Sudoku.instance.allowedNumbers);
                Array.Copy(board, otherSolution, board.Length);
            }

            if (!CompareSolutions(originalSolution, secondarSolution) && numberOfSolutions == 1)
            {
                return false;
            }

            if (!CompareSolutions(originalSolution, otherSolution) && numberOfSolutions == 2)
            {
                return false;
            }

            numberOfSolutions++;
        }
        return true;

    }

    public bool CompareSolutions(int[,] originalBoard, int[,] newBoard)
    {
        bool equal = newBoard.Rank == originalBoard.Rank &&
                    Enumerable.Range(0, newBoard.Rank).All(dimension => newBoard.GetLength(dimension) == originalBoard.GetLength(dimension)) &&
                    newBoard.Cast<int>().SequenceEqual(originalBoard.Cast<int>());

        if (equal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIfBoardIsEmpty(int[,] board)
    {
        int k = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] == 0)
                {
                    k++;
                }
            }
        }
        if (k == 81)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
