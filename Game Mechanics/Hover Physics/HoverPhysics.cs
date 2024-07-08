using UnityEngine;
using ByteSizedAttributes;

/// <summary>
/// This script allows for any object with ot have
/// hover like physcis. The force applied on the 
/// object with vary depending on the force applied
/// on the object and the distance between the object
/// and ground.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public abstract class HoverPhysics : MonoBehaviour {
    #region Variables
    [Header("Hover Properties")]
    [Header("Rigidbody Properties")]
    [ReadOnly] [SerializeField] float totalMass = 0;
    [Tooltip("Speed of slowing down movement.")]
    [MinValue(0)] [SerializeField] float drag = 1;
    [Tooltip("Speed of slowing down rotational movement.")]
    [MinValue(0)] [SerializeField] float angularDrag = 1;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Header("Hover Physcis Properties")]
    [SerializeField] bool isHovering = true;
    [Tooltip("Locations of where the hover force is applied.")]
    [SerializeField] Transform[] hoverPoints;
    [Tooltip("Strength of the downward force.")]
    [AbsoluteValue] [SerializeField] float hoverForce = 200;
    [Tooltip("Distance between the ground and the object.")]
    [AbsoluteValue] [SerializeField] float hoverDistance = 0;

    float TotalMass;
    bool IsHovering;

    [ExecuteInEditMode]
    private void OnValidate() {
        Rigidbody HoverBody = GetComponent<Rigidbody>();

        IsHovering = isHovering;

        HoverBody.drag = drag;
        HoverBody.angularDrag = angularDrag;

        TotalMass = HoverBody.mass;
    }
    #endregion

    /// <Summary>
    /// Hover function must be called in FixedUpdate
    /// </Summary>
    protected void Hover() {
        if (isHovering) {
            RaycastHit hit;
            foreach (Transform HoverPoint in hoverPoints) {
                Vector3 DownwardForce;
                float ForcePercentage;

                if (Physics.Raycast(HoverPoint.position, HoverPoint.up * -1, out hit, hoverDistance)) {
                    // Determine the current distance between the object and ground
                    ForcePercentage = 1 - (hit.distance / hoverDistance);

                    // Calculate the amount of force to apply
                    DownwardForce = transform.up * hoverForce * ForcePercentage;

                    // Include the force applied on the object via mass
                    DownwardForce *= Time.deltaTime * TotalMass;

                    // Apply force
                    Rigidbody HoverBody = GetComponent<Rigidbody>();
                    HoverBody.AddForceAtPosition(DownwardForce, HoverPoint.position);
                }
            }
        }
    }

    public void AllowHover () {
        IsHovering = !IsHovering;
    }

    #region Getter and Setters
    /// <summary>
    /// Change the drag value on the rigidbody.
    /// </summary>
    /// <param name="NewDrag"></param>
    public void SetDrag(float NewDrag) {
        drag = NewDrag;
    }
    /// <summary>
    /// Get the drag value of the rigidboy.
    /// </summary>
    /// <returns></returns>
    public float GetDrag() {
        return drag;
    }
    /// <summary>
    /// Change the angular drag value of the rigidbody.
    /// </summary>
    public void SetAngularDrag(float NewAngularDrag) {
        angularDrag = NewAngularDrag;
    }
    /// <summary>
    /// Get the andgular drag value of the rigidbody
    /// </summary>
    public float GetAngularDrag() {
        return angularDrag;
    }
    /// <summary>
    /// Change the hover force value.
    /// </summary>
    /// <param name="NewHoverForce"></param>
    public void SetHoverForce(float NewHoverForce) {
        hoverForce = NewHoverForce;
    }
    /// <summary>
    /// Get the hover force value.
    /// </summary>
    /// <returns></returns>
    public float GetHoverForce() {
        return hoverForce;
    }
    /// <summary>
    /// Change the hover distance value between object and the ground.
    /// </summary>
    /// <param name="NewHoverDistance"></param>
    public void SetHoverDistance(float NewHoverDistance) {
        hoverDistance = NewHoverDistance;
    }
    /// <summary>
    /// Get the hover distance value between object and the ground.
    /// </summary>
    /// <returns></returns>
    public float GetHoverDistance() {
        return hoverDistance;
    }
    /// <summary>
    /// Returns the total mass of the hover object.
    /// </summary>
    /// <returns></returns>
    public float GetTotalMass() {
        return totalMass;
    }
    #endregion

    private void OnCollisionEnter(Collision Obj) {
        if (Obj.gameObject.GetComponent<Rigidbody>() != null) {
            TotalMass += Obj.gameObject.GetComponent<Rigidbody>().mass;
        }
    }

    private void OnCollisionExit(Collision Obj) {
        if (Obj.gameObject.GetComponent<Rigidbody>() != null) {
            TotalMass -= Obj.gameObject.GetComponent<Rigidbody>().mass;
        }
    }
}