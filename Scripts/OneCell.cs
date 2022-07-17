using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OneCell : MonoBehaviour
{
    public Image Selected;
    public Image Highlight;
    public Image Lowlight;
    public Image ReadOnly;
    public Image Frame;

    private TMP_InputField inputField;
    private string value;

    public int y, x;
    public int ID;
    public bool isCurrentlyWrong;
    public bool isCurrentlyFocused;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    private void Start()
    {
        Sudoku.instance.currentCell = -1;
        //   OnChangedValue();
        CellPrepareOnStart();

        foreach (TMP_InputField a in Sudoku.instance.textList)
        {
            if (a == inputField)
            { 
                ID = Sudoku.instance.textList.IndexOf(a);
            }
        }
    }
    private void Update()
    {
        if (isCurrentlyWrong)
        {
            inputField.textComponent.color = Settings.instance.wrongNumberInCell;
        }
    }
    public void OnSelectCell()
    {
        isCurrentlyFocused = true;
        Selected.gameObject.SetActive(true);
        Sudoku.instance.current_x = x;
        Sudoku.instance.current_y = y;
        Sudoku.instance.currentCell = ID;

        if (Settings.instance.AllowHighlightRow_Col)
        {
            Sudoku.instance.HighlightRowAndCol(y, x);
        }


        if (Sudoku.instance.HighlightNumbersCurrentlyAllowed && Settings.instance.AllowSameNumbersHighlight)
        {
            value = inputField.text;
            Sudoku.instance.HighLightSameNumbers(value);



        }
    }
    public void OnDiselectCell()
    {


        if (inputField.text == "")
        {
            inputField.text = "0";
        }

        Sudoku.instance.currentCell = -1;
        isCurrentlyFocused = false;
        Selected.gameObject.SetActive(false);
        Sudoku.instance.ClearCollors();



    }
    public void OnSelectNeighbor()
    {
        Lowlight.gameObject.SetActive(true);

    }
    public void OnDiselectNeighbor()
    {
        Lowlight.gameObject.SetActive(false);
    }

    public void OnCorrectCell()
    {
        Highlight.color = Settings.instance.correctNumberInCell;
        Highlight.gameObject.SetActive(true);
    }
    public void OnIncorrectCell()
    {
        Highlight.color = Settings.instance.wrongNumberInCell;
        Highlight.gameObject.SetActive(true);
    }
    public void DisableHighlight()
    {
        Highlight.gameObject?.SetActive(false);
       
    }
    public void OnChangedValue()
    {
        Sudoku.instance.ClearSameNumbers();

        if (inputField.text == "0" || inputField.text == "")
        {
            inputField.pointSize = 0;
           
        }
        else
        {          
            inputField.pointSize = 115;

            if (Sudoku.instance.HighlightNumbersCurrentlyAllowed)
            {
                value = inputField.text;
                int number = int.Parse(value);

                if (Settings.instance.AllowSameNumbersHighlight)
                {
                    Sudoku.instance.HighLightSameNumbers(value);

                }

                if (!Sudoku.instance.CheckIfPossibleNumber(y, x, number))
                {
                    isCurrentlyWrong = true;
                }
                else
                {
                    isCurrentlyWrong = false;
                }
            }
           
          
        }
    }

    private void CellPrepareOnStart()  // same function as OnChangeValue but not tied to OnValueChanged event in TMP Input Field component therefore reducing calls
    {
        Sudoku.instance.ClearSameNumbers();

        if (inputField.text == "0" || inputField.text == "")
        {
            inputField.pointSize = 0;

        }
        else
        {
            inputField.pointSize = 115;

            if (Sudoku.instance.HighlightNumbersCurrentlyAllowed)
            {
                value = inputField.text;
                int number = int.Parse(value);

                if (Settings.instance.AllowSameNumbersHighlight)
                {
                    Sudoku.instance.HighLightSameNumbers(value);

                }

                if (!Sudoku.instance.CheckIfPossibleNumber(y, x, number))
                {
                    isCurrentlyWrong = true;
                }
                else
                {
                    isCurrentlyWrong = false;
                }
            }


        }
    }
}
