using UnityEngine;
using UnityEngine.UI;
using ByteSizedAttributes;

/// <summary>
/// This script manipulates UI elements similar
/// to a health bar seen in various games. This 
/// script only allows for the manipulation of
/// vertical and horizontal bars in a single 
/// direction.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Slider))]
public class UniversalUIBar : MonoBehaviour {
    public enum FillDirection { Right2Left, Left2Right, Up2Down, Down2Up}

    #region Universal UI Bar Properties
    [Header("UI Bar Components")]
    [Tooltip("Sprite for the fill bar.")]
    [SerializeField] [NotNullable] Sprite fillSprite = null;
    [Tooltip("Sprite for the holder bar.")]
    [SerializeField] [NotNullable] Sprite containerSprite = null;
    [Space(10)]
    [Tooltip("Primary sprite for the UI Bar.")]
    [SerializeField] [NotNullable] RectTransform fillRect = null;
    [Tooltip("Secondary sprite that contains the fill bar.")]
    [SerializeField] [NotNullable] RectTransform containerRect = null;
    [Space(15)] [LineDivider(4, color: LineColors.Black)]
    [Header("UI Fill Properties")]
    [Tooltip("Color of fill bar based on fill amount.")]
    [SerializeField] Gradient fillBarGradient = null;
    [Space(10)]
    [Tooltip("Smoothing speed to fill the UI bar.")]
    [SerializeField] [MinValue(0)] float fillSpeed = 0;
    [SerializeField] [Range(0f, 1f)] float fillRatio = 1;
    [Tooltip("Direction of how the UI bar will be filled.")]
    [SerializeField] FillDirection fillDirection = FillDirection.Right2Left;

    Slider SliderComponent;

    [ExecuteAlways]
    private void OnValidate() {
        SliderComponent = GetComponent<Slider>();

        SliderComponent.interactable = false;
        SliderComponent.transition = Selectable.Transition.None;
        SliderComponent.navigation = Navigation.defaultNavigation;
        switch (fillDirection) { 
            case FillDirection.Up2Down:
                SliderComponent.direction = Slider.Direction.BottomToTop;
                break;
            case FillDirection.Down2Up:
                SliderComponent.direction = Slider.Direction.TopToBottom;
                break;
            case FillDirection.Right2Left:
                SliderComponent.direction = Slider.Direction.LeftToRight;
                break;
            case FillDirection.Left2Right:
                SliderComponent.direction = Slider.Direction.RightToLeft;
                break;
        }

        SliderComponent.fillRect = fillRect;
        fillRect.GetComponent<Image>().sprite = fillSprite;
        fillRect.GetComponent<Image>().type = Image.Type.Sliced;
        containerRect.GetComponent<Image>().sprite = containerSprite;

        SliderComponent.value = fillRatio;
        Color FillColor = fillBarGradient.Evaluate(fillRatio);
        fillRect.GetComponent<Image>().color = FillColor;
    }
    #endregion

    #region Getters and Setters
    /// <summary>
    /// Get the fill direction of the UI bar.
    /// </summary>
    /// <returns></returns>
    public FillDirection GetFillDirection() {
        return fillDirection;
    }
    /// <summary>
    /// Set the fill direction of the UI bar.
    /// </summary>
    /// <param name="FillDirection"></param>
    public void SetFillDirection(FillDirection FillDirection) {
        fillDirection = FillDirection;
        switch (fillDirection) {
            case FillDirection.Up2Down:
                SliderComponent.direction = Slider.Direction.BottomToTop;
                break;
            case FillDirection.Down2Up:
                SliderComponent.direction = Slider.Direction.TopToBottom;
                break;
            case FillDirection.Right2Left:
                SliderComponent.direction = Slider.Direction.LeftToRight;
                break;
            case FillDirection.Left2Right:
                SliderComponent.direction = Slider.Direction.RightToLeft;
                break;
        }
    }

    /// <summary>
    /// Get the smooth fill speed of the UI bar.
    /// </summary>
    /// <returns></returns>
    public float GetFillSpeed() { 
        return fillSpeed;
    }
    /// <summary>
    /// Set the smooth fill speed of the UI bar.
    /// </summary>
    /// <param name="FillSpeed"></param>
    public void SetFillSpeed(float FillSpeed) { 
        fillSpeed = FillSpeed;
    }

    /// <summary>
    /// Get the fill ratio for the UI bar and color.
    /// </summary>
    /// <returns></returns>
    public float GetFillRatio() {
        return fillRatio;
    }
    /// <summary>
    /// Set the fill ratio for the UI bar and color.
    /// </summary>
    /// <param name="Fill_Ratio"></param>
    public void SetFillRatio(float Fill_Ratio) {
        fillRatio = Fill_Ratio;
        if (fillRatio > 1) {
            fillRatio = 1;
        }
        else if (fillRatio < 0) {
            fillRatio = 0;
        }
    }
    #endregion

    /// <summary>
    /// Instantiate and define the script, this must be called during Start.
    /// </summary>
    protected void Instantiate_UIBar() {
        SliderComponent = GetComponent<Slider>();

        SliderComponent.interactable = false;
        SliderComponent.transition = Selectable.Transition.None;
        SliderComponent.navigation = Navigation.defaultNavigation;
        switch (fillDirection) {
            case FillDirection.Up2Down:
                SliderComponent.direction = Slider.Direction.BottomToTop;
                break;
            case FillDirection.Down2Up:
                SliderComponent.direction = Slider.Direction.TopToBottom;
                break;
            case FillDirection.Right2Left:
                SliderComponent.direction = Slider.Direction.LeftToRight;
                break;
            case FillDirection.Left2Right:
                SliderComponent.direction = Slider.Direction.RightToLeft;
                break;
        }

        SliderComponent.fillRect = fillRect;
        fillRect.GetComponent<Image>().sprite = fillSprite;
        fillRect.GetComponent<Image>().type = Image.Type.Sliced;
        containerRect.GetComponent<Image>().sprite = containerSprite;

        SliderComponent.value = fillRatio;
        Color FillColor = fillBarGradient.Evaluate(fillRatio);
        fillRect.GetComponent<Image>().color = FillColor;
    }

    /// <summary>
    /// Updates the fill bar and color of bar, this must be called during update.
    /// </summary>
    protected void UpdateFill_UI() {
        Color FillColor = fillBarGradient.Evaluate(fillRatio);
        SliderComponent.value = Mathf.Lerp(SliderComponent.value, fillRatio, fillSpeed * Time.deltaTime);
        fillRect.GetComponent<Image>().color = Color.Lerp(fillRect.GetComponent<Image>().color, FillColor, fillSpeed * Time.deltaTime);
    }
}