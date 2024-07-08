using UnityEngine;
using ByteSizedAttributes;

/// <summary>
/// This is a simple script that allows any projectile based object
/// to be guided manually to a destination, set as a homing missle,
/// or even direct it manually with a laser.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class GuidedProjectile : MonoBehaviour {
    #region Variables
    private enum GuidanceSystem { Controlled, Homming, Laser }
    private enum ForwardAxis { X_Axis, Y_Axis, Z_Axis }

    [Header("Guided Projectile Properties")]
    [Tooltip("User guided / self tracking.")]
    [SerializeField] GuidanceSystem guidanceMethod = GuidanceSystem.Controlled;
    [Tooltip("Direction of the projectile (what is forward).")]
    [SerializeField] ForwardAxis forwardDirection = ForwardAxis.X_Axis;
    [Tooltip("Speed of the projectile's turn.")]
    [SerializeField] [Range(0, 1)] float turnRate = 0.5f;
    [Space(5)]
    [SerializeField] bool uniformVelocity = false;
    [Tooltip("Uniform projectile velocity.")]
    [SerializeField] [HideIf("uniformVelocity", true)] float projectileVelocity = 0;
    [Tooltip("Set the starting velocity.")]
    [SerializeField] [ConditionalHide("uniformVelocity", true)] float initalVelocity = 0;
    [Tooltip("Set the max velocity.")]
    [SerializeField] [ConditionalHide("uniformVelocity", true)] float maxVelocity = 0;
    [Tooltip("How fast it takes to get to max velocity.")]
    [SerializeField][ConditionalHide("uniformVelocity", true)] float accelerationTime = 0;
    [Space(5)]
    [Tooltip("Force stop the guided projectile at start.")]
    [SerializeField] bool isTracking = false;
    [Space(5)] [LineDivider(4, color: LineColors.Black)]
    [Header("Control Guided Values")]
    [Tooltip("Used for input control.")]
    [SerializeField][ConditionalEnumHide("guidanceMethod", (int)GuidanceSystem.Controlled)] float inputSmoothing = 0;
    [Tooltip("Used for input control.")]
    [SerializeField][ConditionalEnumHide("guidanceMethod", (int)GuidanceSystem.Controlled)] float inputSensitivity = 0;

    [ExecuteInEditMode]
    private void OnValidate() {
        if (initalVelocity < 0) {
            initalVelocity = 0;
        }
        if (initalVelocity < 1) {
            initalVelocity = 1;
        }
        if (inputSmoothing < 0) {
            inputSmoothing = 0;
        }
        if (inputSensitivity < 0) {
            inputSensitivity = 0;
        }

        if (!uniformVelocity) {
            CurrentVelocity = initalVelocity;
        }
    }

    Rigidbody ProjectileBody;
    GameObject TargetObj;
    Vector3 LaserTarget;
    bool IsTracking;

    Vector2 DefaultVector = Vector2.zero;
    Vector3 ControlledDirection, SmoothedVector;
    float CurrentVelocity;
    #endregion

    #region Getters & Setters
    /// <summary>
    /// Change the input smoothing.
    /// </summary>
    /// <param name="NewInputSmoothing"></param>
    public void SetInputSmoothing(float NewInputSmoothing) {
        inputSmoothing = NewInputSmoothing;
    }
    /// <summary>
    /// Get the input smoothing value.
    /// </summary>
    /// <returns></returns>
    public float GetInputSmoothing() {
        return inputSmoothing;
    }
    /// <summary>
    /// Change the input sensitivity.
    /// </summary>
    public void SetInputSensitivity(float NewInputSensitivity) {
        inputSensitivity = NewInputSensitivity;
    }
    /// <summary>
    /// Get the input sensitivity value.
    /// </summary>
    /// <returns></returns>
    public float GetInputSensitivity() {
        return inputSensitivity;
    }
    /// <summary>
    /// Change the turn speed.
    /// </summary>
    public void SetTurnSpeed(float NewTurnSpeed) {
        turnRate = NewTurnSpeed;
    }
    /// <summary>
    /// Get the turn rate for the projectile.
    /// </summary>
    /// <returns></returns>
    public float GetTurnSpeed() {
        return turnRate;
    }
    /// <summary>
    /// Set new uniform velocity.
    /// </summary>
    /// <param name="NewVelocity"></param>
    public void SetFixedProjectileVelocity(float NewVelocity) {
        projectileVelocity = NewVelocity;
    }
    /// <summary>
    /// Get unifor velocity.
    /// </summary>
    /// <returns></returns>
    public float GetFixedProjectileVelocity() {
        return projectileVelocity;
    }
    /// <summary>
    /// Set new itital velocity.
    /// </summary>
    /// <param name="NewInitalVelocity"></param>
    public void SetNewInitalVelocity(float NewInitalVelocity) {
        initalVelocity = NewInitalVelocity;
    }
    /// <summary>
    /// Get inital velocity.
    /// </summary>
    /// <returns></returns>
    public float GetInitalVelocity() {
        return initalVelocity;
    }
    /// <summary>
    /// Set new max velocity.
    /// </summary>
    /// <param name="NewMaxVelocity"></param>
    public void SetNewMaxVelocity(float NewMaxVelocity) {
        maxVelocity = NewMaxVelocity;
    }
    /// <summary>
    /// Get max velocity.
    /// </summary>
    /// <returns></returns>
    public float GetMaxVelocity() {
        return maxVelocity;
    }
    /// <summary>
    /// Set new acceleration time.
    /// </summary>
    /// <param name="NewAccelerationTime"></param>
    public void SetNewAccelerationTime(float NewAccelerationTime) {
        accelerationTime = NewAccelerationTime;
    }
    /// <summary>
    /// Get acceleration time.
    /// </summary>
    /// <returns></returns>
    public float GetAccelerationTime() {
        return accelerationTime;
    }
    /// <summary>
    /// Get the projectile's speed.
    /// </summary>
    /// <returns></returns>
    public float GetCurrentVelocity() {
        return ProjectileBody.velocity.magnitude;
    }

    /// <summary>
    /// Set target for projectile
    /// </summary>
    /// <param name="NewTarget"></param>
    public void SetTarget(GameObject NewTarget = null) {
        TargetObj = NewTarget;
    }
    /// <summary>
    /// Returns to targeted object
    /// </summary>
    /// <returns></returns>
    public GameObject GetTarget() {
        return TargetObj;
    }

    /// <summary>
    /// Set position to target for projectile
    /// </summary>
    /// <param name="TargetPos"></param>
    public void SetTargetPosition(Vector3 TargetPos) {
        LaserTarget = TargetPos;
    }
    /// <summary>
    /// Returns a vector 3 position for laser targeting
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTargetedPosition() {
        return LaserTarget;
    }

    /// <summary>
    /// Enable or disable the homing function.
    /// </summary>
    public void EnableTracking() {
        IsTracking = !IsTracking;
    }
    #endregion

    #region Guided Projectile Functions
    /// <summary>
    /// This must be called in the fixed update method and is the core function of the script.
    /// </summary>
    /// <param name="InputVector"></param>
    protected void MoveProjectile(Vector2 InputVector = default(Vector2)) {
        IsTracking = isTracking;
        if (uniformVelocity) { 
            CurrentVelocity = projectileVelocity;
        }
        else {
            CurrentVelocity = Mathf.Lerp(CurrentVelocity, maxVelocity, accelerationTime);
        }

        // Ensure that all the referances to the body is there
        if (ProjectileBody == null) {
            ProjectileBody = GetComponent<Rigidbody>();
            ProjectileBody.useGravity = false;
        }
        else
        {
            if (IsTracking)
            {
                switch (guidanceMethod)
                {
                    case GuidanceSystem.Homming:
                        Quaternion HomingDirection = Quaternion.LookRotation(TargetObj.transform.position - this.transform.position);

                        ApplyNewDirection(HomingDirection);
                        break;
                    case GuidanceSystem.Controlled:
                        var NewDirection = new Vector2(-InputVector.y, InputVector.x);

                        NewDirection = Vector2.Scale(NewDirection, new Vector2(inputSensitivity * inputSmoothing, inputSensitivity * inputSmoothing));
                        SmoothedVector.x = Mathf.Lerp(SmoothedVector.x, NewDirection.x, 1f / inputSmoothing);
                        SmoothedVector.y = Mathf.Lerp(SmoothedVector.y, NewDirection.y, 1f / inputSmoothing);
                        ControlledDirection += SmoothedVector;

                        ApplyNewDirection(Quaternion.Euler(ControlledDirection));
                        break;
                    case GuidanceSystem.Laser:
                        Quaternion LaserDirection = Quaternion.LookRotation(LaserTarget - this.transform.position);

                        ApplyNewDirection(LaserDirection);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Appiles the the forward movement for based on the object's forward direction.
    /// </summary>
    /// <param name="TargetRotation"></param>
    void ApplyNewDirection(Quaternion TargetRotation) {
        ProjectileBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, TargetRotation, turnRate));

        switch (forwardDirection) {
            case ForwardAxis.X_Axis:
                ProjectileBody.velocity = transform.right * CurrentVelocity;
                break;
            case ForwardAxis.Y_Axis:
                ProjectileBody.velocity = transform.up * CurrentVelocity;
                break;
            case ForwardAxis.Z_Axis:
                ProjectileBody.velocity = transform.forward * CurrentVelocity;
                break;
        }
    }
    #endregion
}