using UnityEngine;
using UnityEngine.UI;
using ByteSizedAttributes;

/// <summary>
/// Universal timer script, the script allows for the
/// change in timer function, reset, pause, and unpause and text output.
/// </summary>

[DisallowMultipleComponent]
public class UniversalTimer : MonoBehaviour {
    #region Variables
    private enum TimerFunction { CountUp, CountDown }
    private enum TimerTextOutput { Minutes, Seconds }

    [Header("Timer Properties")]
    [Tooltip("Timer pause state.")]
    [ReadOnly] [SerializeField] bool timerPaused = true;
    [Space(5)] [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Determines if the type of timer.")]
    [SerializeField] TimerFunction function = TimerFunction.CountUp;
    [Tooltip("The total amount of time the timer has to count down in seconds.")]
    [ConditionalEnumHide("function", (int)TimerFunction.CountDown)][SerializeField] int allowedTime = 0;
    [Space(5)] [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Text field for the timer.")]
    [SerializeField] Text timerText = null;
    [Tooltip("Accuracy of timer text.")]
    [SerializeField] TimerTextOutput outputInclusion = TimerTextOutput.Minutes;

    float CurrentTime = 0;
    string Minutes, Seconds;

    [ExecuteInEditMode]
    private void OnValidate() {
        if (allowedTime < 0) {
            allowedTime = 0;
        }
    }
    #endregion

    #region Timer Functions
    /// <summary>
    /// Core timer function.
    /// </summary>
    protected void UpdateTimer() {
        if (!timerPaused) {
            switch (function) {
                case TimerFunction.CountUp:
                    CurrentTime += Time.deltaTime;

                    Minutes = ((int)CurrentTime / 60).ToString();
                    Seconds = (CurrentTime % 60).ToString("f2");
                    break;
                case TimerFunction.CountDown:
                    CurrentTime -= Time.deltaTime;

                    if (CurrentTime <= 0) {
                        CurrentTime = 0;
                    }

                    Minutes = ((int)CurrentTime / 60).ToString();
                    Seconds = (CurrentTime % 60).ToString("f2");
                    break;
            }

            if (timerText != null) {
                string ReturnText = null;
                switch (outputInclusion) {
                    case TimerTextOutput.Minutes:
                        ReturnText = Minutes;
                        break;
                    case TimerTextOutput.Seconds:
                        ReturnText = Minutes + ":" + Seconds;
                        break;
                }
                timerText.text = ReturnText;
            }
        }
    }

    /// <summary>
    /// Pause and Unpause timer.
    /// </summary>
    public void PauseUnpauseTimer() {
        timerPaused = !timerPaused;
    }

    /// <summary>
    /// Reset timer based on the current function.
    /// </summary>
    /// <param name="TotalTime"></param>
    public void ResetTimer(int TotalTime = 0) {
        timerPaused = true;

        switch (function) {
            case TimerFunction.CountUp:
                CurrentTime = 0;

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
            case TimerFunction.CountDown:
                if (TotalTime > 0) {
                    CurrentTime = TotalTime;
                }
                else {
                    CurrentTime = allowedTime;
                }

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
        }

        if (timerText != null) {
            string ReturnText = null;
            switch (outputInclusion) {
                case TimerTextOutput.Minutes:
                    ReturnText = Minutes;
                    break;
                case TimerTextOutput.Seconds:
                    ReturnText = Minutes + ":" + Seconds;
                    break;
            }
            timerText.text = ReturnText;
        }
    }

    /// <summary>
    /// Swap timer function.
    /// </summary>
    /// <param name="TotalTime"></param>
    public void SwitchTimerFunction(int TotalTime = 0) {
        timerPaused = true;

        switch (function) {
            case TimerFunction.CountUp:
                function = TimerFunction.CountDown;

                CurrentTime = TotalTime;

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
            case TimerFunction.CountDown:
                function = TimerFunction.CountUp;

                CurrentTime = 0;

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
        }

        if (timerText != null) {
            string ReturnText = null;
            switch (outputInclusion) {
                case TimerTextOutput.Minutes:
                    ReturnText = Minutes;
                    break;
                case TimerTextOutput.Seconds:
                    ReturnText = Minutes + ":" + Seconds;
                    break;
            }
            timerText.text = ReturnText;
        }
    }
    #endregion
}