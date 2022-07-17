using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public Color32 correctNumberInCell = new Color32(69, 205, 50, 242);
    public Color32 wrongNumberInCell = new Color32(205, 50, 69, 242);
    public Color32 sameNumberHighlight = new Color32(0, 10, 255, 255);
    public Color32 defaultNumbersColor = new Color32(0, 0, 0, 255);
    public Color32 defaultCellFrameColor = new Color32(255, 255, 255, 255);
    public Color32 ReadonlyFrameColor = new Color32(197, 146, 255, 255);
    public Color32 ImportantMessageColor = new Color32(20, 255, 20, 255);
    public Color32 NotimportantMessageColor = new Color32(255, 255, 255, 255);


    public float TimeToCountUp = 60;
    public float TimeToCountDown = 60;


    public bool AllowHighlightRow_Col = true;
    public bool AllowFrameColorChanges = true;
    public bool AllowSameNumbersHighlight = true;


   
    private void Awake()
    {
        instance = this;
        instance.TimeToCountUp = 60;
        instance.TimeToCountDown = 60;
    }

    public void SetAllowHighLightRow_Col()
    {
        AllowHighlightRow_Col = !AllowHighlightRow_Col;
    }

    public void SetAllowFrameChanges()
    {
        AllowFrameColorChanges = !AllowFrameColorChanges;
        Sudoku.instance.SwitchFrame();
    }

    public void SetSameNumberHighlight()
    {
        AllowSameNumbersHighlight = !AllowSameNumbersHighlight;
    }
  
  

}
