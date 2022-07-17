using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timerDurationOnCountDown; //* 60f;
    private float timerDurationOnCountUp;
    private float timer;

    private float flashTimer;
    private float flashDuration = 1f;

    private bool isPaused = false;

    public bool CountDown = true;
   
    public TextMeshProUGUI leftMinute;
    public TextMeshProUGUI rightMinute;

    public TextMeshProUGUI LeftSecond;
    public TextMeshProUGUI RightSecond;

    public TextMeshProUGUI seperator;

    public Sprite PausedSprite;
    public Sprite CountingSprite;

    public Sprite CountingDownSprite;
    public Sprite CountingUpSprite;

    public Button pauseButton;
    public Button CountModeButton;
    public TMP_Text countDownToggleLabel;
    public TMP_Text pauseLabelText;

    public static Timer instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        timerDurationOnCountDown = Settings.instance.TimeToCountDown;
        timerDurationOnCountUp = Settings.instance.TimeToCountUp;
        resetTimer();
        SetPause(true);
        SetCountDownText();

    }

    private void Update()
    {
        if (!isPaused)
        {

            if (CountDown && timer > 0)
            {
                timer -= Time.deltaTime;
                UpdateTimerDisplay(timer);
            }
            else if (!CountDown && timer < timerDurationOnCountUp)
            {
                timer += Time.deltaTime;
                UpdateTimerDisplay(timer);
            }

            else
            {
                Flash();
            }
        }
    }

    public void resetTimer()
    {
        timerDurationOnCountDown = Settings.instance.TimeToCountDown;
        timerDurationOnCountUp = Settings.instance.TimeToCountUp;
        SetPause(true);
        if (CountDown)
        {
            timer = timerDurationOnCountDown;
        }
        else
        {
            timer = 0;
        }
        SetTextDispaly(true);
        UpdateTimerDisplay(timer);
      
    }
    private void UpdateTimerDisplay(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
        leftMinute.text = currentTime[0].ToString();
        rightMinute.text = currentTime[1].ToString();
        LeftSecond.text = currentTime[2].ToString();
        RightSecond.text = currentTime[3].ToString();
    }
    private void Flash()
    {
        if (CountDown && timer != 0)
        {
            timer = 0;
            UpdateTimerDisplay(timer);
        }
        else if (!CountDown && timer < timerDurationOnCountUp)
        {
            timer = timerDurationOnCountUp;
            UpdateTimerDisplay(timer);
        }


        if (flashTimer <= 0)
        {
            flashTimer = flashDuration;
        }
        else if (flashTimer >= flashDuration / 2)
        { 
            flashTimer -= Time.deltaTime;
            SetTextDispaly(false);
        }
        else 
        {
            flashTimer -= Time.deltaTime;
            SetTextDispaly(true);
        }
        
    }

    private void SetTextDispaly(bool enabled)
    { 
        leftMinute.enabled = enabled;
        rightMinute.enabled = enabled;
        seperator.enabled = enabled;
        LeftSecond.enabled = enabled;
        RightSecond.enabled = enabled;
    }

    public void SetPause(bool variable)
    {
        if (variable)
        {
            isPaused = variable;
        }
        else
        {
            isPaused = !isPaused;
        }
      
        if (!isPaused)
        {
            pauseButton.image.sprite = PausedSprite;
            pauseLabelText.text = "Pause";
        }
        else
        {
            pauseButton.image.sprite = CountingSprite;
            pauseLabelText.text = "Resume";
        }
    }
    public void SetCountMode()
    {
       
        SetPause(true);   
        CountDown = !CountDown;
        if (CountDown)
        {
            CountModeButton.image.sprite = CountingDownSprite;
        }
        else
        {
            CountModeButton.image.sprite = CountingUpSprite;
        }
        resetTimer();
        SetCountDownText();
    }
    private void SetCountDownText()
    {
        if (CountDown)
        {
            countDownToggleLabel.text = "Down";
        }
        else
        {
            countDownToggleLabel.text = "Up";
        }
    }
}
