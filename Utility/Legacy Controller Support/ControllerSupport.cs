using UnityEngine;

/// <summary>
/// This script maps all input for Xbox and Playstation
/// controllers. This version of the Controler Support
/// script uses the old Unity input system.
/// </summary>

public static class ControllerSupport {
    #region Xbox Controls
    /// <summary>
    /// Left Xbox Joystick Input
    /// </summary>
    public static Vector2 Xbox_LeftStick() {
        float X = 0;
        X += Input.GetAxis("Xbox_LeftStickX");
        X =  Mathf.Clamp(X, -1, 1);

        float Y = 0;
        Y += Input.GetAxis("Xbox_LeftStickY");
        Y = Mathf.Clamp(Y, -1, 1);

        Vector2 Value = new Vector2(X, Y);
        return Value;
    }

    /// <summary>
    /// Right Xbox Joystick Input
    /// </summary>
    public static Vector2 Xbox_RightStick() {
        float X = 0;
        X += Input.GetAxis("Xbox_RightStickX");
        X = Mathf.Clamp(X, -1, 1);

        float Y = 0;
        Y += Input.GetAxis("Xbox_RightStickY");
        Y = Mathf.Clamp(Y, -1, 1);

        Vector2 Value = new Vector2(X, Y);
        return Value;
    }


    /// <summary>
    /// Holding Down Xbox Left Joystick
    /// </summary>
    public static bool Xbox_LeftStickHold() {
        return Input.GetButton("Xbox_LeftStick");
    }

    /// <summary>
    /// Pressing Down Xbox Left Joystick
    /// </summary>
    public static bool Xbox_LeftStickDown() {
        return Input.GetButtonDown("Xbox_LeftStick");
    }

    /// <summary>
    /// Release Xbox Left Joystick
    /// </summary>
    public static bool Xbox_LeftStickUp() {
        return Input.GetButtonUp("Xbox_LeftStick");
    }

    /// <summary>
    /// Holding Down Xbox Right Joystick
    /// </summary>
    public static bool Xbox_RightStickHold() {
        return Input.GetButton("Xbox_RightStick");
    }

    /// <summary>
    /// Pressing Down Xbox Right Joystick
    /// </summary>
    public static bool Xbox_RightStickDown() {
        return Input.GetButtonDown("Xbox_RightStick");
    }

    /// <summary>
    /// Release Xbox Right Joystick
    /// </summary>
    public static bool Xbox_RightStickUp() {
        return Input.GetButtonUp("Xbox_RightStick");
    }

    /// <summary>
    /// Xbox D-Pad Input
    /// </summary>
    public static Vector2 Xbox_Dpad() {
        float X = 0;
        X += Input.GetAxis("Xbox_DpadX");
        X = Mathf.Clamp(X, -1, 1);

        float Y = 0;
        Y += Input.GetAxis("Xbox_DpadY");
        Y = Mathf.Clamp(Y, -1, 1);

        Vector2 Value = new Vector2(X, Y);
        return Value;
    }

    /// <summary>
    /// Holding Down Left Xbox Bumper
    /// </summary>
    public static bool Xbox_LeftBumpHold() {
        return Input.GetButton("Xbox_LeftBump");
    }

    /// <summary>
    /// Pressing Down Left Xbox Bumper
    /// </summary>
    public static bool Xbox_LeftBumpDown() {
        return Input.GetButtonDown("Xbox_LeftBump");
    }

    /// <summary>
    /// Releasing Left Xbox Bumper
    /// </summary>
    public static bool Xbox_LeftBumpUp() {
        return Input.GetButtonUp("Xbox_LeftBump");
    }

    /// <summary>
    /// Holding Down Right Xbox Bumper
    /// </summary>
    public static bool Xbox_RightBumpHold() {
        return Input.GetButton("Xbox_RightBump");
    }

    /// <summary>
    /// Pressing Down Right Xbox Bumper
    /// </summary>
    public static bool Xbox_RightBumpDown() {
        return Input.GetButtonDown("Xbox_RightBump");
    }

    /// <summary>
    /// Releasing Right Xbox Bumper
    /// </summary>
    public static bool Xbox_RightBumpUp() {
        return Input.GetButtonUp("Xbox_RightBump");
    }

    /// <summary>
    /// Left Xbox Trigger Input
    /// </summary>
    public static float Xbox_LeftTrigger() {
        float X = 0;
        X += Input.GetAxis("Xbox_LeftTrigger");
        return Mathf.Clamp(X, 0, 1);
    }

    /// <summary>
    /// Right Xbox Trigger Input
    /// </summary>
    public static float Xbox_RightTrigger() {
        float X = 0;
        X += Input.GetAxis("Xbox_RightTrigger");
        return Mathf.Clamp(X, 0, 1);
    }

    /// <summary>
    /// Holding Down Xbox A Button
    /// </summary>
    public static bool Xbox_AButtonHold() {
        return Input.GetButton("Xbox_AButton");
    }

    /// <summary>
    /// Pressing Down Xbox A Button
    /// </summary>
    public static bool Xbox_AButtonDown() {
        return Input.GetButtonDown("Xbox_AButton");
    }

    /// <summary>
    /// Releasing Xbox A Button
    /// </summary>
    public static bool Xbox_AButtonUp() {
        return Input.GetButtonUp("Xbox_AButton");
    }

    /// <summary>
    /// Holding Down Xbox B Button
    /// </summary>
    public static bool Xbox_BButtonHold() {
        return Input.GetButton("Xbox_BButton");
    }

    /// <summary>
    /// Pressing Down Xbox B Button
    /// </summary>
    public static bool Xbox_BButtonDown() {
        return Input.GetButtonDown("Xbox_BButton");
    }

    /// <summary>
    /// Releasing Xbox B Button
    /// </summary>
    public static bool Xbox_BButtonUp() {
        return Input.GetButtonUp("Xbox_BButton");
    }

    /// <summary>
    /// Holding Down Xbox X Button
    /// </summary>
    public static bool Xbox_XButtonHold() {
        return Input.GetButton("Xbox_XButton");
    }

    /// <summary>
    /// Pressing Down Xbox X Button
    /// </summary>
    public static bool Xbox_XButtonDown() {
        return Input.GetButtonDown("Xbox_XButton");
    }

    /// <summary>
    /// Releasing Xbox X Button
    /// </summary>
    public static bool Xbox_XButtonUp() {
        return Input.GetButtonUp("Xbox_XButton");
    }

    /// <summary>
    /// Holding Down Xbox Y Button
    /// </summary>
    public static bool Xbox_YButtonHold() {
        return Input.GetButton("Xbox_YButton");
    }

    /// <summary>
    /// Pressing Down Xbox Y Button
    /// </summary>
    public static bool Xbox_YButtonDown() {
        return Input.GetButtonDown("Xbox_YButton");
    }

    /// <summary>
    /// Releasing Xbox Y Button
    /// </summary>
    public static bool Xbox_YButtonUp() {
        return Input.GetButtonUp("Xbox_YButton");
    }

    /// <summary>
    /// Holding Down Xbox Back Button
    /// </summary>
    public static bool Xbox_BackButtonHold() {
        return Input.GetButton("Xbox_StartButton");
    }

    /// <summary>
    /// Pressing Down Xbox Back Button
    /// </summary>
    public static bool Xbox_BackButtonDown() {
        return Input.GetButtonDown("Xbox_StartButton");
    }

    /// <summary>
    /// Releasing Xbox Back Button
    /// </summary>
    public static bool Xbox_BackButtonUp() {
        return Input.GetButtonUp("Xbox_StartButton");
    }

    /// <summary>
    /// Holding Down Xbox Start Button
    /// </summary>
    public static bool Xbox_StartButtonHold() {
        return Input.GetButton("Xbox_StartButton");
    }

    /// <summary>
    /// Pressing Down Xbox Start Button
    /// </summary>
    public static bool Xbox_StartButtonDown() {
        return Input.GetButtonDown("Xbox_StartButton");
    }

    /// <summary>
    /// Releasing Xbox Start Button
    /// </summary>
    public static bool Xbox_StartButtonUp() {
        return Input.GetButtonUp("Xbox_StartButton");
    }
    #endregion

    #region Playstation Controls
    /// <summary>
    /// Left Playstation Joystick Input
    /// </summary>
    public static Vector2 PlayStation_LeftStick() {
        float X = 0;
        X += Input.GetAxis("PlayStation_LeftStickX");
        X = Mathf.Clamp(X, -1, 1);

        float Y = 0;
        Y += Input.GetAxis("PlayStation_LeftStickY");
        Y = Mathf.Clamp(Y, -1, 1);

        Vector2 Value = new Vector2(X, Y);
        return Value;
    }

    /// <summary>
    /// Right Playstation Joystick Input
    /// </summary>
    public static Vector2 PlayStation_RightStick() {
        float X = 0;
        X += Input.GetAxis("PlayStation_RightStickX");
        X = Mathf.Clamp(X, -1, 1);

        float Y = 0;
        Y += Input.GetAxis("PlayStation_RightStickY");
        Y = Mathf.Clamp(Y, -1, 1);

        Vector2 Value = new Vector2(X, Y);
        return Value;
    }

    /// <summary>
    /// Holding Down Left Playstation Joystick 
    /// </summary>
    public static bool PlayStation_LeftStickHold() {
        return Input.GetButton("PlayStation_LeftStick");
    }

    /// <summary>
    /// Pressing Down Left Playstation Joystick 
    /// </summary>
    public static bool PlayStation_LeftStickDown() {
        return Input.GetButtonDown("PlayStation_LeftStick");
    }

    /// <summary>
    /// Releasing Left Playstation Joystick 
    /// </summary>
    public static bool PlayStation_LeftStickUp() {
        return Input.GetButtonUp("PlayStation_LeftStick");
    }

    /// <summary>
    /// Holding Down Right Playstation Joystick 
    /// </summary>
    public static bool PlayStation_RightStickHold() {
        return Input.GetButton("PlayStation_RightStick");
    }

    /// <summary>
    /// Pressing Down Right Playstation Joystick 
    /// </summary>
    public static bool PlayStation_RightStickDown() {
        return Input.GetButtonDown("PlayStation_RightStick");
    }

    /// <summary>
    /// Releasing Right Playstation Joystick 
    /// </summary>
    public static bool PlayStation_RightStickUp() {
        return Input.GetButtonUp("PlayStation_RightStick");
    }

    /// <summary>
    /// Playstation D-Pad Input 
    /// </summary>
    public static Vector2 PlayStation_Dpad() {
        float X = 0;
        X += Input.GetAxis("PlayStation_DpadX");
        X = Mathf.Clamp(X, -1, 1);

        float Y = 0;
        Y += Input.GetAxis("PlayStation_DpadY");
        Y = Mathf.Clamp(Y, -1, 1);

        Vector2 Value = new Vector2(X, Y);
        return Value;
    }

    /// <summary>
    /// Holding Down Left Playstation Bumper 
    /// </summary>
    public static bool PlayStation_LeftBumpHold() {
        return Input.GetButton("PlayStation_LeftBump");
    }

    /// <summary>
    /// Pressing Down Left Playstation Bumper 
    /// </summary>
    public static bool PlayStation_LeftBumpDown() {
        return Input.GetButtonDown("PlayStation_LeftBump");
    }

    /// <summary>
    /// Releasing Left Playstation Bumper 
    /// </summary>
    public static bool PlayStation_LeftBumpUp() {
        return Input.GetButtonUp("PlayStation_LeftBump");
    }

    /// <summary>
    /// Holding Down Right Playstation Bumper 
    /// </summary>
    public static bool PlayStation_RightBumpHold() {
        return Input.GetButton("PlayStation_RightBump");
    }

    /// <summary>
    /// Pressing Down Right Playstation Bumper 
    /// </summary>
    public static bool PlayStation_RightBumpDown() {
        return Input.GetButtonDown("PlayStation_RightBump");
    }

    /// <summary>
    /// Releasing Right Playstation Bumper 
    /// </summary>
    public static bool PlayStation_RightBumpUp() {
        return Input.GetButtonUp("PlayStation_RightBump");
    }

    /// <summary>
    /// Left Playstation Trigger Input 
    /// </summary>
    public static float PlayStation_LeftTrigger() {
        float X = 0;
        X += Input.GetAxis("PlayStation_LeftTrigger");
        return Mathf.Clamp(X, 0, 1);
    }

    /// <summary>
    /// Right Playstation Trigger Input 
    /// </summary>
    public static float PlayStation_RightTrigger() {
        float X = 0;
        X += Input.GetAxis("PlayStation_RightTrigger");
        return Mathf.Clamp(X, 0, 1);
    }

    /// <summary>
    /// Holding Down Playstation Square Button
    /// </summary>
    public static bool PlayStation_SButtonHold() {
        return Input.GetButton("PlayStation_SButton");
    }

    /// <summary>
    /// Pressing Down Playstation Square Button
    /// </summary>
    public static bool PlayStation_SButtonDown() {
        return Input.GetButtonDown("PlayStation_SButton");
    }

    /// <summary>
    /// Releasing Playstation Square Button
    /// </summary>
    public static bool PlayStation_SButtonUp() {
        return Input.GetButtonUp("PlayStation_SButton");
    }

    /// <summary>
    /// Holding Down Playstation Cross Button
    /// </summary>
    public static bool PlayStation_XButtonHold() {
        return Input.GetButton("PlayStation_XButton");
    }

    /// <summary>
    /// Pressing Down Playstation Cross Button
    /// </summary>
    public static bool PlayStation_XButtonDown() {
        return Input.GetButtonDown("PlayStation_XButton");
    }

    /// <summary>
    /// Releasing Playstation Cross Button
    /// </summary>
    public static bool PlayStation_XButtonUp() {
        return Input.GetButtonUp("PlayStation_XButton");
    }

    /// <summary>
    /// Holding Down Playstation Circle Button
    /// </summary>
    public static bool PlayStation_OButtonHold() {
        return Input.GetButton("PlayStation_OButton");
    }

    /// <summary>
    /// Pressing Down Playstation Circle Button
    /// </summary>
    public static bool PlayStation_OButtonDown() {
        return Input.GetButtonDown("PlayStation_OButton");
    }

    /// <summary>
    /// Releasing Playstation Circle Button
    /// </summary>
    public static bool PlayStation_OButtonUp() {
        return Input.GetButtonUp("PlayStation_OButton");
    }

    /// <summary>
    /// Holding Down Playstation Triangle Button
    /// </summary>
    public static bool PlayStation_TButtonHold() {
        return Input.GetButton("PlayStation_TButton");
    }

    /// <summary>
    /// Pressing Down Playstation Triangle Button
    /// </summary>
    public static bool PlayStation_TButtonDown() {
        return Input.GetButtonDown("PlayStation_TButton");
    }

    /// <summary>
    /// Releasing Down Playstation Triangle Button
    /// </summary>
    public static bool PlayStation_TButtonUp() {
        return Input.GetButtonUp("PlayStation_TButton");
    }

    /// <summary>
    /// Holding Down Playstation Share Button
    /// </summary>
    public static bool PlayStation_ShareButtonHold() {
        return Input.GetButton("PlayStation_ShareButton");
    }

    /// <summary>
    /// Pressing Down Playstation Share Button
    /// </summary>
    public static bool PlayStation_ShareButtonDown() {
        return Input.GetButtonDown("PlayStation_ShareButton");
    }

    /// <summary>
    /// Releasing Playstation Share Button
    /// </summary>
    public static bool PlayStation_ShareButtonUp() {
        return Input.GetButtonUp("PlayStation_ShareButton");
    }

    /// <summary>
    /// Holding Down Playstation Options Button
    /// </summary>
    public static bool PlayStation_OptionsButtonHold() {
        return Input.GetButton("PlayStation_OptionsButton");
    }

    /// <summary>
    /// Pressing Down Playstation Options Button
    /// </summary>
    public static bool PlayStation_OptionsButtonDown() {
        return Input.GetButtonDown("PlayStation_OptionsButton");
    }

    /// <summary>
    /// Releasing Playstation Options Button
    /// </summary>
    public static bool PlayStation_OptionsButtonUp() {
        return Input.GetButtonUp("PlayStation_OptionsButton");
    }

    /// <summary>
    /// Holding Down Playstation Home Button
    /// </summary>
    public static bool PlayStation_HomeButtonHold() {
        return Input.GetButton("PlayStation_HomeButton");
    }

    /// <summary>
    /// Pressing Down Playstation Home Button
    /// </summary>
    public static bool PlayStation_HomeButtonDown() {
        return Input.GetButtonDown("PlayStation_HomeButton");
    }

    /// <summary>
    /// Releasing Playstation Home Button
    /// </summary>
    public static bool PlayStation_HomeButtonUp() {
        return Input.GetButtonUp("PlayStation_HomeButton");
    }
    #endregion
}