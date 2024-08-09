using UnityEngine;
using ByteSizedAttributes;

/// <summary>
/// This script allows for an object to have
/// the movement and chacteristics of a drone.
/// Taking in input, the script will manipulate
/// position, rotation and even audio, similar
/// to a flying drone. Any method that is called
/// must be used in the FixedUpdate method.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class DroneController : MonoBehaviour {
    #region Drone Controller Variables
    [Header("Drone Controller Properties")]
    [Header("Drone Components")]
    [Tooltip("Enable uniform tilt.")]
    [SerializeField] protected bool uniformTilt = true;
    [Tooltip("Drone Parts that will be tilting.")]
    [SerializeField] protected TiltComponents tiltComponents = null;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Header("Drone Movement")]
    [Tooltip("Drone movement speed and tilt for forward and strafe movement is the same.")]
    [SerializeField] bool uniformMovement = true;
    [Tooltip("Enable correction of drone's up force while moving.")]
    [SerializeField] bool correctUpForce = true;
    [Tooltip("Drone's movement speed for forward and strafe.")]
    [SerializeField][ConditionalHide("uniformMovement", true)] float movementSpeed = 0;
    [Tooltip("Drone's movement speed for forward movement.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float forwardSpeed = 0;
    [Tooltip("Drone's movement speed for strafe movement.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float strafeSpeed = 0;
    [Space(10)]
    [Tooltip("Acceleration speed of the drone 'x', and the deceleration speed of the the drone 'y'.")]
    [SerializeField][AbsoluteValue] Vector2 accelNDecelSpeed = Vector2.zero;
    [Space(20)]
    [Tooltip("Allow to change the elevation of the drone by input.")]
    [SerializeField] bool enableElevationChange = true;
    [Tooltip("The speed of the drone moving up and down, 'x' is the speed going up, and 'y' is the speed going down.")]
    [SerializeField][ConditionalHide("enableElevationChange", true)] Vector2 elevationSpeed = Vector2.zero;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Tooltip("The amount the drone rotates left and right.")]
    [SerializeField][AbsoluteValue] float droneRotationAmount = 0f;
    [Tooltip("The speed of the drone rotation.")]
    [SerializeField][AbsoluteValue] private float droneRotationSpeed = 0f;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Drone's movement tilt for all directional movement.")]
    [SerializeField][ConditionalHide("uniformMovement", true)] float tiltAmount = 0;
    [Tooltip("Speed of drone's tilt when moving.")]
    [SerializeField][ConditionalHide("uniformMovement", true)] float tiltSpeed = 0;
    [Tooltip("Drone's movement tilt for forward movement.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float forwardTilt = 0;
    [Tooltip("Drone's movement tilt for strafe movement.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float strafeTilt = 0;
    [Space(5)]
    [Tooltip("Speed of drone's tilt when moving forward.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float forwardTiltSpeed = 0;
    [Tooltip("Speed of drone's tilt when strafing.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float strafeTiltSpeed = 0;

    [System.Serializable]
    protected class TiltComponents {
        [SerializeField] public Transform[] forwardTiltComponents = null;
        [SerializeField] public Transform[] strafeTiltComponents = null;
    }

    [SerializeField][HideInInspector] private bool NotUniform = false;

    [ExecuteInEditMode]
    private void OnValidate() {
        NotUniform = !uniformMovement;

        // Ensure all movement values are positive
        if (movementSpeed < 0) {
            movementSpeed = 0;
        }
        if (forwardSpeed < 0) {
            forwardSpeed = 0;
        }
        if (strafeSpeed < 0) {
            strafeSpeed = 0;
        }
        if (elevationSpeed.x < 0) {
            elevationSpeed.x = 0;
        }
        if (elevationSpeed.y < 0) {
            elevationSpeed.y = 0;
        }

        if (droneRotationAmount < 0) {
            droneRotationAmount = 0;
        }
        if (droneRotationSpeed > 1) {
            droneRotationSpeed = 1;
        }

        if (tiltAmount < 0) {
            tiltAmount = 0;
        }
        if (forwardTilt < 0) {
            forwardTilt = 0;
        }
        if (strafeTilt < 0) {
            strafeTilt = 0;
        }

        if (tiltSpeed < 0) {
            tiltSpeed = 0;
        }
        else if (tiltSpeed > 1) {
            tiltSpeed = 1;
        }
        if (forwardTiltSpeed < 0) {
            forwardTiltSpeed = 0;
        }
        else if (forwardTiltSpeed > 1) {
            forwardTiltSpeed = 1;
        }
        if (strafeTiltSpeed < 0) {
            strafeTiltSpeed = 0;
        }
        else if (strafeTiltSpeed > 1) {
            strafeTiltSpeed = 1;
        }
    }
    
    //Private values
    Rigidbody DroneBody;
    AudioSource DroneSound;
    float SplitSpeedLimit;
    float E_Input, F_Input, S_Input, R_Input;
    float UpForce;
    float ForwardTilt, StrafeTilt;
    float TiltRefForward, TiltRefStrafe;
    float DroneRotation;
    float DroneRotationRef;
    float DesiredYRotation;
    #endregion

    // Awake is called when the script instance is being loaded
    void Awake() {
        DroneBody = GetComponent<Rigidbody>();
        DroneSound = GetComponent<AudioSource>();

        SplitSpeedLimit = new Vector2(forwardSpeed, strafeSpeed).magnitude;
    }

    /// <summary>
    /// Change drone elevation, must be called during FixedUpdate
    /// </summary>
    /// <param name="ElevationInput"></param>
    protected void ElevationChange(float ElevationInput) {
        // Up force correction during movement
        if (correctUpForce) {
            if (Mathf.Abs(F_Input) != 0 || Mathf.Abs(S_Input) != 0) {
                if (E_Input != 0) {
                    DroneBody.velocity = DroneBody.velocity;
                }
                else {
                    DroneBody.velocity = new Vector3(DroneBody.velocity.x, -DroneBody.velocity.y, DroneBody.velocity.z);
                }

            }
        }

        // Changing drone elevation
        if (enableElevationChange) {
            E_Input = Mathf.Clamp(ElevationInput, -1, 1);

            if (E_Input != 0) {
                if (E_Input > 0) {
                    UpForce = elevationSpeed.x * DroneBody.mass;

                    // Go up, upwards speed will be faster than going down
                    DroneBody.AddRelativeForce(Vector3.up * UpForce * (E_Input * 2));
                }
                else {
                    UpForce = elevationSpeed.y * DroneBody.mass;

                    // Go down, downwards speed will be slower that going up
                    DroneBody.AddRelativeForce(Vector3.up * UpForce * (E_Input / 4));
                }
            }
            else {
                // Hover
                DroneBody.AddRelativeForce(Vector3.up * Vector3.Magnitude(Physics.gravity) * DroneBody.mass);
            }
        }

        DroneAudio();
    }

    /// <summary>
    /// Drone forward movement, must be called during FixedUpdate
    /// </summary>
    /// <param name="ForwardInput"></param>
    protected void ForwardMovement(float ForwardInput) {
        F_Input = Mathf.Clamp(ForwardInput, -1, 1);

        // Movement and tilt calculation
        switch (uniformMovement) {
            case true:
                DroneBody.AddRelativeForce(Vector3.forward * F_Input * movementSpeed);
                ForwardTilt = Mathf.SmoothDamp(ForwardTilt, tiltAmount * F_Input, ref TiltRefForward, tiltSpeed);
                break;

            case false:
                DroneBody.AddRelativeForce(Vector3.forward * F_Input * forwardSpeed);
                ForwardTilt = Mathf.SmoothDamp(ForwardTilt, forwardTilt * F_Input, ref TiltRefForward, forwardTiltSpeed);
                break;
        }

        DroneTilt();
        SpeedClamp();
        DroneAudio();
    }

    /// <summary>
    /// Drone strafe movement, must be called during FixedUpdate
    /// </summary>
    /// <param name="StrafeInput"></param>
    protected void StrafeMovement(float StrafeInput) {
        S_Input = Mathf.Clamp(StrafeInput, -1, 1);

        // Movement and tilt calculation
        switch (uniformMovement) {
            case true:
                DroneBody.AddRelativeForce(Vector3.right * S_Input * movementSpeed);
                StrafeTilt = Mathf.SmoothDamp(StrafeTilt, tiltAmount * -S_Input, ref TiltRefStrafe, tiltSpeed);
                break;

            case false:
                DroneBody.AddRelativeForce(Vector3.right * S_Input * strafeSpeed);
                StrafeTilt = Mathf.SmoothDamp(StrafeTilt, strafeTilt * -S_Input, ref TiltRefStrafe, strafeTiltSpeed);
                break;
        }

        DroneTilt();
        SpeedClamp();
        DroneAudio();
    }

    /// <summary>
    /// Drone Rotation
    /// </summary>
    /// <param name="RotateInput"></param>
    protected void RotateDrone(float RotateInput) {
        R_Input = Mathf.Clamp(RotateInput, -1, 1);

        if (R_Input != 0) {
            DesiredYRotation +=(droneRotationAmount * R_Input);
        }

        DroneRotation = Mathf.SmoothDamp(DroneRotation, DesiredYRotation, ref DroneRotationRef, droneRotationSpeed);
    }

    /// <summary>
    /// Drone tilt and rotation depending on input
    /// </summary>
    private void DroneTilt() {
        switch (uniformTilt) {
            case true:
                DroneBody.rotation = Quaternion.Euler(ForwardTilt, DroneRotation, StrafeTilt);
                break;

            case false:
                DroneBody.rotation = Quaternion.Euler(DroneBody.rotation.x, DroneRotation, DroneBody.rotation.z);

                foreach (Transform F_Tilit in tiltComponents.forwardTiltComponents) {
                    F_Tilit.localRotation = Quaternion.Euler(ForwardTilt, F_Tilit.rotation.y, F_Tilit.rotation.z);
                }

                foreach (Transform S_Tilit in tiltComponents.strafeTiltComponents) {
                    S_Tilit.localRotation = Quaternion.Euler(S_Tilit.rotation.x, S_Tilit.rotation.y, StrafeTilt);
                }
                break;
        }
    }

    /// <summary>
    /// Limit speed of drone
    /// </summary>
    private void SpeedClamp() {
        switch (uniformMovement) {
            case true:
                if (Mathf.Abs(F_Input) != 0 && Mathf.Abs(S_Input) != 0) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, movementSpeed, Time.deltaTime * accelNDecelSpeed.x));
                }
                else if (Mathf.Abs(F_Input) != 0 && Mathf.Abs(S_Input) < 0.2) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, movementSpeed, Time.deltaTime * accelNDecelSpeed.x));
                }
                else if (Mathf.Abs(F_Input) == 0 && Mathf.Abs(S_Input) != 0) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, movementSpeed, Time.deltaTime * accelNDecelSpeed.x));
                }
                else{
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, 0, Time.deltaTime * accelNDecelSpeed.y));
                }
                break;

            case false:
                if (Mathf.Abs(F_Input) != 0 && Mathf.Abs(S_Input) != 0) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, SplitSpeedLimit, Time.deltaTime * accelNDecelSpeed.x));
                }
                else if (Mathf.Abs(F_Input) != 0 && Mathf.Abs(S_Input) == 0) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, forwardSpeed, Time.deltaTime * accelNDecelSpeed.x));
                }
                else if (Mathf.Abs(F_Input) == 0 && Mathf.Abs(S_Input) != 0) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, strafeSpeed, Time.deltaTime * accelNDecelSpeed.x));
                }
                else {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, 0, Time.deltaTime * accelNDecelSpeed.y));
                }
                break;
        }
    }

    /// <summary>
    /// Audio during movement
    /// </summary>
    private void DroneAudio() {
        DroneSound.pitch = 1 + (DroneBody.velocity.magnitude / 100);
    }
}