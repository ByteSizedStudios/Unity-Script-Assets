using UnityEngine;
using ByteSizedAttributes;
using System.Collections;

/// <summary>
/// This script allows for the manipulation of the
/// time scale modifier in Unity. This script will
/// only have one instance ever loaded in a scene.
/// </summary>

[DisallowMultipleComponent]
public class TimeManipulator : DontDestroy {
    #region Variables
    [Header("Time Manipulator Properties")]
    [Tooltip("Current time scale in Unity.")]
    [SerializeField] [ReadOnly] float currentTimeScale = 1;
    [Space] [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Enable upper and lower limit for time scale speeds.")]
    [SerializeField] bool timeScaleLimits = false;
    [Tooltip("Slowest possible time scale speed.")]
    [SerializeField] [ConditionalHide("timeScaleLimits", true)] float lowestTimeScale = 0;
    [Tooltip("Fastest possible time scale speed.")]
    [SerializeField] [ConditionalHide("timeScaleLimits", true)] float highestTimeScale = 1;

    static TimeManipulator instance;

    [ExecuteInEditMode]
    private void OnValidate() {
        if (lowestTimeScale < 0) {
            lowestTimeScale = 0;
        }
        else if (lowestTimeScale >= 1) {
            lowestTimeScale = 1;
        }
        if (highestTimeScale < 1) {
            highestTimeScale = 1;
        }
    }
    #endregion

    #region Time Manipulator Funcitons
    /// <summary>
    /// Instantly set new time scale speed.
    /// </summary>
    /// <param name="NewTimeScale"></param>
    public void SetTimeScale(float NewTimeScale) {
        currentTimeScale = NewTimeScale;
        if (currentTimeScale < 0) {
            currentTimeScale = 0;
        }
        if (timeScaleLimits) {
            if (currentTimeScale > highestTimeScale) {
                currentTimeScale = highestTimeScale;
            }
        }
        Time.timeScale = currentTimeScale;
    }

    /// <summary>
    /// Get the current time scale speed.
    /// </summary>
    /// <returns></returns>
    public float GetTimeScale() {
        return currentTimeScale;
    }

    /// <summary>
    /// Reset the time scale speed to 1, default speed.
    /// </summary>
    public void ResetTimeScale() {
        currentTimeScale = 1;
        Time.timeScale = currentTimeScale;
    }

    /// <summary>
    /// Slowly change the time scale to the desired new time scale speed.
    /// </summary>
    /// <param name="SmoothSpeed"></param>
    /// <param name="NewTimeScale"></param>
    /// <returns></returns>
    public IEnumerator LerpTimeScale(float SmoothSpeed, float NewTimeScale) {
        float Elapse = 0;
        float TempTimeScale = currentTimeScale;

        while (Elapse < SmoothSpeed) {
            TempTimeScale = Mathf.Lerp(TempTimeScale, NewTimeScale, SmoothSpeed * Time.deltaTime);
            Time.timeScale = TempTimeScale;

            Elapse += Time.deltaTime;
            yield return null;
        }

        currentTimeScale = TempTimeScale;
    }
    #endregion
}