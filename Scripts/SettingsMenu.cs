using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public TMP_InputField timeToCountDown_IF;
    public TMP_InputField timeToCountUp_IF;

    public Button correctNumberInCellButton;
    public Button wrongNumberInCellButton;
    public Button sameNumberHighlightButton;
    public Button readOnlyFrameButton;
    public Button importantMessageButton;
    public Button notImportantMessageButton;

    public FlexibleColorPicker colorPicker;

   
    public void SetColor(Button button)
    {
        button.image.color = colorPicker.color;
    }
    public void OnTurnOff()
    {
        SetColors();
        SetTime();
    }


    public void OnTurnOn()
    {
        timeToCountDown_IF.text = (Settings.instance.TimeToCountDown / 60).ToString();
        timeToCountUp_IF.text = (Settings.instance.TimeToCountUp / 60).ToString();

        correctNumberInCellButton.image.color = Settings.instance.correctNumberInCell;
        wrongNumberInCellButton.image.color = Settings.instance.wrongNumberInCell;
        sameNumberHighlightButton.image.color = Settings.instance.sameNumberHighlight;
        readOnlyFrameButton.image.color = Settings.instance.ReadonlyFrameColor;
        importantMessageButton.image.color = Settings.instance.ImportantMessageColor;
        notImportantMessageButton.image.color = Settings.instance.NotimportantMessageColor;
    }

    private void SetColors()
    {
        try
        {

            Settings.instance.correctNumberInCell = correctNumberInCellButton.image.color;
            Settings.instance.wrongNumberInCell = wrongNumberInCellButton.image.color;
            Settings.instance.sameNumberHighlight = sameNumberHighlightButton.image.color;
            Settings.instance.ReadonlyFrameColor = readOnlyFrameButton.image.color;
            Settings.instance.ImportantMessageColor = importantMessageButton.image.color;
            Settings.instance.NotimportantMessageColor = notImportantMessageButton.image.color;

        }
        catch (Exception e)
        {
            Debug.LogException(e);
            MessageManager.instance.PrintMessage(e.ToString(), MessageType.important);
        }
    }
    private void SetTime()
    {
        try
        {
            float CountDownTime = float.Parse(timeToCountDown_IF.text);
            float CountUpTime = float.Parse(timeToCountUp_IF.text);

            if (CountDownTime > 0 && CountDownTime <= 99)
            {
                Settings.instance.TimeToCountDown = float.Parse(timeToCountDown_IF.text) * 60;
            }
            else if (CountDownTime <= 0 || CountDownTime > 99)
            {
                MessageManager.instance.PrintMessage("Time to countdown can only be a value between 1 - 99!", MessageType.important);
            }


            if (CountUpTime > 0 && CountUpTime <= 99)
            {
                Settings.instance.TimeToCountUp = float.Parse(timeToCountUp_IF.text) * 60;
            }

            else if (CountUpTime <= 0 || CountDownTime > 99)
            {
                MessageManager.instance.PrintMessage("Time to countup can only be a value between 1 - 99!", MessageType.important);
            }


            Timer.instance.resetTimer();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            MessageManager.instance.PrintMessage(e.ToString(), MessageType.important);
        }
    }
}
