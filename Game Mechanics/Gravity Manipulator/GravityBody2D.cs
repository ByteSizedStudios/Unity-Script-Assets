using UnityEngine;
using System.Collections.Generic;

/// <summary> 
/// This script provides any information that the 
/// Gravity Manipulator script needs to apply the
/// physics upon object.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class GravityBody2D : MonoBehaviour {
    #region Gravity Body 2D Properties
    public bool enableGravity = true;

    Rigidbody2D ObjectBody;
    Transform ObjectTransform;
    public List<GravityManipulator2D> GravityManipulators = new List<GravityManipulator2D>();
    #endregion

    /// <summary>
    /// Initalize with Start().
    /// </summary>
    public void InitalizeGravBody() {
        ObjectBody = GetComponent<Rigidbody2D>();
        ObjectBody.simulated = false;
        ObjectTransform = GetComponent<Transform>();
    }

    /// <summary>
    /// Must be used during FixedUpdate() for gravitation force to take affect.
    /// </summary>
    public void GravBodyFixedUpdate() {
        if (enableGravity && GravityManipulators != null) {
            foreach (GravityManipulator2D manipulator in GravityManipulators) {
                manipulator.ApplyGravity(ObjectBody, ObjectTransform);
            }
        }
    }

    #region Add and Remove GravityManipulators to and from the list
    public void AddManipulator(GravityManipulator2D GravitySource) {
        if (!GravityManipulators.Contains(GravitySource)) {
            GravityManipulators.Add(GravitySource);
        }
    }
    public void RemoveManipulator(GravityManipulator2D GravitySource) {
        GravityManipulators.Remove(GravitySource);
    }
    #endregion
}