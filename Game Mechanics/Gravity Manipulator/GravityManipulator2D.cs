using UnityEngine;
using ByteSizedAttributes;
using System.Collections.Generic;

/// <summary>
/// This script manipulates the forces applied on any
/// game objects with the "GravityBody" script. The 
/// script will be dynamic enough to change both gravity
/// direction and force along with the effected area.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class GravityManipulator2D : MonoBehaviour {
    #region Gravity Manipulator Values
    private enum Gravity { Directional, Spherical }
    public enum ForceAxis { X, Y }

    [Header("Gravity Manipulation Properties")]
    [Tooltip("Plantary gravity (Spherical), Specific direction (Directional)")]
    [SerializeField] Gravity gravityType = Gravity.Directional;
    [Tooltip("Gravity strength.")]
    [SerializeField] float gravityForce = 9.81f;
    [Tooltip("The speed of the object rotatating upwards relative to gravity direction.")]
    [SerializeField] float rotationSpeed = 50f;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Limited Gravity Range.")]
    [SerializeField] bool limitedGravityArea = true;
    [Space(10)]
    [Tooltip("Direction of gravity.")]
    [SerializeField] [ConditionalEnumHide("gravityType", (int)Gravity.Directional)] GravityAxis gravityAxis = null;
    [Tooltip("Area for gravity.")]
    [SerializeField] [ConditionalHide("limitedGravityArea", true)] DirectionalGravity directionalGravity = null;
    [Tooltip("Area for gravity.")]
    [SerializeField] [ConditionalHide("limitedGravityArea", true)] SphericalGravity sphericalGravity = null;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Gravity manipulator affects only listed objects with selective tags.")]
    [SerializeField] bool selectiveObjects = false;
    [Tooltip("All tags that will be affect by the gravity manipulator.")]
    [SerializeField] List<SelectiveTags> objectTags = null;

    [System.Serializable]
    protected class GravityAxis {
        [EnumFlags] public ForceAxis axis = ForceAxis.X;
    }

    [System.Serializable]
    protected class DirectionalGravity {
        [ConditionalEnumHideAttribute("gravityType", (int)Gravity.Directional)] public Vector2 gravitySize = new Vector2(1, 1);
    }

    [System.Serializable]
    protected class SphericalGravity {
        [ConditionalEnumHideAttribute("gravityType", (int)Gravity.Spherical)] public float gravityRadius = 1;
    }

    [System.Serializable]
    protected class SelectiveTags {
        [Tag] public string objectTag = null;
    }

    float CurrentGravity = 0;
    GravityBody2D[] tempBodies;
    Vector2 GravityDirection;
    Vector2 ObjectUp;
    Quaternion ObjectRotation;

    [ExecuteInEditMode]
    private void OnValidate() {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;

        if (gravityType == Gravity.Directional) {
            boxCollider.enabled = true;
            circleCollider.enabled = false;
        }
        else {
            boxCollider.enabled = false;
            circleCollider.enabled = true;
        }

        boxCollider.size = directionalGravity.gravitySize;
        circleCollider.radius = sphericalGravity.gravityRadius;

        if (directionalGravity.gravitySize.x < 0) {
            directionalGravity.gravitySize.x = 0;
        }
        if (directionalGravity.gravitySize.y < 0) {
            directionalGravity.gravitySize.y = 0;
        }
        if (sphericalGravity.gravityRadius < 0) {
            sphericalGravity.gravityRadius = 0;
        }
    }
    #endregion

    // Start is called before the first frame update
    protected virtual void Start() {
        CurrentGravity = gravityForce;
    }

    /// <summary>
    /// Update all Gravity Body scripts with this manipulator.
    /// This method should be placed in FixedUpdate()
    /// </summary>
    protected void GravityUpdate()
    {
        if (!limitedGravityArea)
        {
            // Finds every object with "GravityBody"
            tempBodies = FindObjectsOfType<GravityBody2D>();
            foreach (GravityBody2D i in tempBodies)
            {
                if (selectiveObjects)
                {
                    for (int a = 0; a < objectTags.Count; a++)
                    {
                        if (i.gameObject.CompareTag(objectTags[a].objectTag))
                        {
                            i.AddManipulator(this);
                        }
                    }
                }
                else
                {
                    i.AddManipulator(this);
                }
            }
        }
    }

    /// <summary>
    /// Applies the gravity force on objects with the GravBody script.
    /// </summary>
    public void ApplyGravity(Rigidbody2D ObjectBody, Transform ObjectTransform) {
        switch (gravityType) {
            case Gravity.Directional:
                // Apply gravitational force by axis
                if (gravityAxis.axis.HasFlag(ForceAxis.X)) {
                    ObjectBody.AddForce(ObjectTransform.right * -CurrentGravity);
                }
                if (gravityAxis.axis.HasFlag(ForceAxis.Y)) {
                    ObjectBody.AddForce(ObjectTransform.up * -CurrentGravity);
                }

                // Rotate object to align with the gravity source
                ObjectTransform.rotation = Quaternion.Slerp(ObjectTransform.rotation, transform.localRotation, rotationSpeed * Time.deltaTime);
                break;
            case Gravity.Spherical:
                // Determine the direction of the gravity and object "up" direction
                GravityDirection = (ObjectTransform.localPosition - transform.localPosition).normalized;
                ObjectUp = ObjectTransform.up;

                // Apply gravity
                ObjectBody.AddForce(GravityDirection * -CurrentGravity);

                // Rotate the object based on the position realative to the gravity source
                ObjectRotation = Quaternion.FromToRotation(ObjectUp, GravityDirection) * ObjectTransform.rotation;
                ObjectTransform.rotation = Quaternion.Slerp(ObjectTransform.rotation, ObjectRotation, rotationSpeed * Time.deltaTime);
                break;
        }
    }

    #region Getters and Setters
    /// <summary>
    /// Change the gravitational force
    /// </summary>
    public void ChangeGravityForce(float NewGravForce = 0) {
        CurrentGravity = NewGravForce;
    }
    /// <summary>
    /// Manipulate the gravitational force.
    /// </summary>
    public void AdjustGravityForce(float GravAdjustment = 0) {
        CurrentGravity += GravAdjustment;
    }
    /// <summary>
    /// Change the dimensions of the area which the gravity is manipulated.
    /// </summary>
    public void ChangeGravityRangeBox(Vector3 BoxSize) {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = directionalGravity.gravitySize;
    }
    /// <summary>
    /// Change the dimensions of the area which the gravity is manipulated.
    /// </summary>
    public Vector3 GetGravityRangeBox() {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        return boxCollider.size;
    }
    /// <summary>
    /// Change the radius of the area which the gravity is manipulated.
    /// </summary>
    public void ChangeGravityRadius(float SphereRadius) {
        CircleCollider2D sphereCollider = GetComponent<CircleCollider2D>();
        sphereCollider.radius = SphereRadius;
    }
    /// <summary>
    /// Get the radius of the manipulator's range.
    /// </summary>
    public float GetGravityRadius() {
        CircleCollider2D sphereCollider = GetComponent<CircleCollider2D>();
        return sphereCollider.radius;
    }
    #endregion

    #region Gravity manipulator Collision Triggers
    protected virtual void OnTriggerEnter(Collider obj) {
        if (selectiveObjects) {
            for (int a = 0; a < objectTags.Count; a++) {
                if (obj.gameObject.CompareTag(objectTags[a].objectTag)) {
                    obj.GetComponent<GravityBody2D>().AddManipulator(this);
                }
            }
        }
        else {
            obj.GetComponent<GravityBody2D>().AddManipulator(this);
        }
    }
    protected virtual void OnTriggerExit(Collider obj) {
        obj.GetComponent<GravityBody2D>().RemoveManipulator(this);
    }
    #endregion
}